using INFORCE_.NET_TASK.Models;

namespace INFORCE_.NET_TASK.Services.Interfaces
{
    public interface IAuthService
    {
        Task<User?> LoginAsync(string username, string password);
        void Logout();
        bool IsAuthenticated();
        bool IsAdmin();
        Task<User?> GetCurrentUserAsync();
        bool CanDeleteUrl(ShortUrl url);
        bool CanEditAbout();

        Guid? GetCurrentUserId();
    }
}
