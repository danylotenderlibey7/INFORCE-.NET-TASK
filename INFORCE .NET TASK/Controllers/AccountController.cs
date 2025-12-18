using Microsoft.AspNetCore.Mvc;
using INFORCE_.NET_TASK.Services;
using INFORCE_.NET_TASK.Services.Interfaces;

namespace INFORCE_.NET_TASK.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountController(
            IAuthService authService,
            IHttpContextAccessor httpContextAccessor)
        {
            _authService = authService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _authService.LoginAsync(username, password);
            if (user == null)
            {
                ViewBag.Error = "Invalid username or password";
                return View();
            }


            var httpContext = _httpContextAccessor.HttpContext 
                ?? throw new InvalidOperationException("HttpContext is null");

            httpContext.Session.SetString("UserId", user.Id.ToString());
            httpContext.Session.SetString("Username", user.Username);
            httpContext.Session.SetInt32("UserRole", (int)user.Role);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            _authService.Logout();
            return RedirectToAction("Index", "Home");
        }
    }
}