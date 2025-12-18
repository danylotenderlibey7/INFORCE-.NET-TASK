using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using INFORCE_.NET_TASK.Services;
using INFORCE_.NET_TASK.Models;
using INFORCE_.NET_TASK.Data;
using INFORCE_.NET_TASK.Services.Interfaces;

namespace INFORCE_.NET_TASK.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ApplicationDbContext _context;

        public AuthController(
            IAuthService authService,
            ApplicationDbContext context)
        {
            _authService = authService;
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _authService.LoginAsync(request.Username, request.Password);

            if (user == null)
                return Unauthorized(new { error = "Invalid username or password" });


            return Ok(new
            {
                success = true,
                user = new
                {
                    id = user.Id,
                    username = user.Username,
                    role = user.Role.ToString()
                }
            });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            _authService.Logout();
            return Ok(new { success = true });
        }

        [HttpGet("currentuser")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await _authService.GetCurrentUserAsync();

            if (user == null)
                return Unauthorized(new { isAuthenticated = false });

            return Ok(new
            {
                isAuthenticated = true,
                user = new
                {
                    id = user.Id,
                    username = user.Username,
                    role = user.Role.ToString()
                }
            });
        }

        [HttpGet("check")]
        public IActionResult CheckAuth()
        {
            return Ok(new { isAuthenticated = _authService.IsAuthenticated() });
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}