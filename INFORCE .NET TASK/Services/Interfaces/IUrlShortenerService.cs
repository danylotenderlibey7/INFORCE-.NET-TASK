using INFORCE_.NET_TASK.Models;

namespace INFORCE_.NET_TASK.Services.Interfaces
{
    public interface IUrlShortenerService
    {
        Task<ShortUrl> CreateShortUrlAsync(string originalUrl, Guid userId);
        Task<string> RedirectAsync(string shortCode);
        Task<bool> DeleteShortUrlAsync(Guid urlId, Guid userId);
        Task<IEnumerable<ShortUrl>> GetAllUrlsAsync();
        Task<ShortUrl?> GetByIdUrl(Guid urlId);

    }
}
