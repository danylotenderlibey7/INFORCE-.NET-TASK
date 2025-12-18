using INFORCE_.NET_TASK.Data;
using INFORCE_.NET_TASK.Models;
using INFORCE_.NET_TASK.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace INFORCE_.NET_TASK.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private const string UserIdKey = "UserId";
        private const string UsernameKey = "Username";
        private const string UserRoleKey = "UserRole";
        public AuthService(
            ApplicationDbContext context,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public bool CanDeleteUrl(ShortUrl url)
        {
            if (url == null) return false;

            var currentUser = GetCurrentUserAsync().Result;
            if (currentUser == null) return false;

            if (currentUser.Role == UserRole.Admin) return true;

            return url.CreatedByUserId == currentUser.Id;
        }

        public bool CanEditAbout()
        {
            return IsAdmin();
        }

        public async Task<User?> GetCurrentUserAsync()
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue) return null;

            return await _context.Users.FindAsync(userId.Value);
        }

        public bool IsAdmin()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if(httpContext == null) return false;

            var role = httpContext.Session.GetInt32(UserRoleKey);
            return role.HasValue && role.Value == (int)UserRole.Admin;
        }

        public bool IsAuthenticated()
        {
            return GetCurrentUserId().HasValue;
        }

        public async Task<User?> LoginAsync(string username, string password)
        {
            var passwordHash = HashPassword(password);
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username && u.PasswordHash == passwordHash);

            if (user != null)
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext != null)
                {
                    httpContext.Session.SetString(UserIdKey, user.Id.ToString());
                    httpContext.Session.SetString(UsernameKey, user.Username);
                    httpContext.Session.SetInt32(UserRoleKey, (int)user.Role);
                }
            }
            return user;
        }

        public void Logout()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                httpContext.Session.Clear();
            }
        }

        public Guid? GetCurrentUserId()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null) return null;

            var userIdString = httpContext.Session.GetString(UserIdKey);
            if (Guid.TryParse(userIdString, out var userId))
                return userId;

            return null;
        }

        private static string HashPassword(string password)
        {
            return Convert.ToBase64String(
                System.Security.Cryptography.SHA256.HashData(
                    System.Text.Encoding.UTF8.GetBytes(password)));
        }

    }
}
