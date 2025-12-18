using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using INFORCE_.NET_TASK.Data;
using INFORCE_.NET_TASK.Models;
using INFORCE_.NET_TASK.Services;
using INFORCE_.NET_TASK.Services.Interfaces;
using INFORCE_.NET_TASK.Models.ViewModels;

namespace INFORCE_.NET_TASK.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUrlShortenerService _urlService;
        private readonly IAuthService _authService;

        public HomeController(
            ApplicationDbContext context,
            IUrlShortenerService urlService,
            IAuthService authService)
        {
            _context = context;
            _urlService = urlService;
            _authService = authService;
        }

        public async Task<IActionResult> Index()
        {
            var urls = await _urlService.GetAllUrlsAsync();
            var currentUser = await _authService.GetCurrentUserAsync();

            var viewModels = urls.Select(url => new ShortUrlViewModel
            {
                Id = url.Id,
                OriginalUrl = url.OriginalUrl,
                ShortCode = url.ShortCode,
                CreatedDate = url.CreatedDate,
                CreatedByUsername = url.CreatedBy?.Username ?? "Unknown",
                CanDelete = _authService.CanDeleteUrl(url)
            }).ToList();

            ViewBag.CanAdd = _authService.IsAuthenticated();

            return View(viewModels);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string originalUrl)
        {
            if (!_authService.IsAuthenticated())
                return RedirectToAction("Login", "Account");

            var currentUser = await _authService.GetCurrentUserAsync();
            if (currentUser == null)
                return RedirectToAction("Login", "Account");

            try
            {
                await _urlService.CreateShortUrlAsync(originalUrl, currentUser.Id);
                TempData["Success"] = "URL shortened successfully!";
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!_authService.IsAuthenticated())
                return RedirectToAction("Login", "Account");

            var currentUser = await _authService.GetCurrentUserAsync();
            if (currentUser == null)
                return RedirectToAction("Login", "Account");

            var result = await _urlService.DeleteShortUrlAsync(id, currentUser.Id);

            if (result)
                TempData["Success"] = "URL deleted successfully!";
            else
                TempData["Error"] = "Failed to delete URL or permission denied";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("r/{shortCode}")]
        public async Task<IActionResult> RedirectUrl(string shortCode)
        {
            try
            {
                var originalUrl = await _urlService.RedirectAsync(shortCode);
                return Redirect(originalUrl);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}