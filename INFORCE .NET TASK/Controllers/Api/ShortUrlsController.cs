using INFORCE_.NET_TASK.Models.DTO;
using INFORCE_.NET_TASK.Models;
using INFORCE_.NET_TASK.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace INFORCE_.NET_TASK.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShortUrlsController : ControllerBase
    {
        private readonly IUrlShortenerService _urlService;
        private readonly IAuthService _authService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ShortUrlsController(
            IUrlShortenerService urlService,
            IAuthService authService,
            IHttpContextAccessor httpContextAccessor)
        {
            _urlService = urlService;
            _authService = authService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShortUrlResponse>>> GetAll()
        {
            var urls = await _urlService.GetAllUrlsAsync();
            var currentUser = await _authService.GetCurrentUserAsync();
            
            var response = urls.Select(url => MapToResponse(url, currentUser)).ToList();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ShortUrlResponse>> GetById(Guid id)
        {
            if (!_authService.IsAuthenticated())
                return Unauthorized(new { error = "Authentication required" });

            var url = await _urlService.GetByIdUrl(id);
            if (url == null)
                return NotFound();

            var currentUser = await _authService.GetCurrentUserAsync();
            return Ok(MapToResponse(url, currentUser));
        }

        [HttpPost]
        public async Task<ActionResult<ShortUrlResponse>> Create([FromBody] CreateShortUrlRequest request)
        {
            if (!_authService.IsAuthenticated())
                return Unauthorized(new { error = "Authentication required" });

            var currentUser = await _authService.GetCurrentUserAsync(); 
            if (currentUser == null)
                return Unauthorized();

            try
            {
                var shortUrl = await _urlService.CreateShortUrlAsync(request.OriginalUrl, currentUser.Id);
                return CreatedAtAction(
                    nameof(GetById), 
                    new { id = shortUrl.Id }, 
                    MapToResponse(shortUrl, currentUser));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!_authService.IsAuthenticated())
                return Unauthorized(new { error = "Authentication required" });

            var currentUser = await _authService.GetCurrentUserAsync();
            if (currentUser == null)
                return Unauthorized();

            var result = await _urlService.DeleteShortUrlAsync(id, currentUser.Id);
            if (!result)
                return NotFound(new { error = "URL not found or you don't have permission to delete it" });

            return NoContent();
        }

        [HttpGet("redirect/{shortCode}")]
        public async Task<IActionResult> RedirectToOriginal(string shortCode)
        {
            try
            {
                var originalUrl = await _urlService.RedirectAsync(shortCode);
                return await RedirectToOriginal(originalUrl);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { error = "Short URL not found" });
            }
        }

        private ShortUrlResponse MapToResponse(ShortUrl url, User? currentUser)
        {
            var request = _httpContextAccessor.HttpContext?.Request;
            var baseUrl = $"{request?.Scheme}://{request?.Host}";
            
            return new ShortUrlResponse
            {
                Id = url.Id,
                OriginalUrl = url.OriginalUrl,
                ShortCode = url.ShortCode,
                ShortUrl = $"{baseUrl}/api/shorturls/redirect/{url.ShortCode}",
                CreatedDate = url.CreatedDate,
                CreatedByUserId = url.CreatedByUserId,
                CreatedByUsername = url.CreatedBy?.Username ?? "Unknown",
                CanDelete = _authService.CanDeleteUrl(url)
            };
        }
    }
}
