using INFORCE_.NET_TASK.Data;
using INFORCE_.NET_TASK.Models;
using INFORCE_.NET_TASK.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace INFORCE_.NET_TASK.Services.Implementations
{
    public class UrlShortenerService : IUrlShortenerService
    {
        private readonly ApplicationDbContext _context;
        private const string Alphabet = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const int ShortCodeLength = 10;
        private static readonly Random _random = new Random();


        public UrlShortenerService(ApplicationDbContext context)
        {
            _context = context;
        }
        private string GenerateShortCode()
        {
            var chars = new char[ShortCodeLength];

            for (int i = 0; i < ShortCodeLength; i++)
            {
                chars[i] = Alphabet[_random.Next(Alphabet.Length)];
            }

            return new string(chars);
        }

        public async Task<ShortUrl> CreateShortUrlAsync(string originalUrl, Guid userId)
        {
            var existingUrl = await _context.ShortUrls
                .FirstOrDefaultAsync(u => u.OriginalUrl == originalUrl);

            if (existingUrl != null)
                throw new InvalidOperationException("URL already exists");

            string shortCode;
            do
            {
                shortCode = GenerateShortCode();
            } while (await _context.ShortUrls.AnyAsync(u => u.ShortCode == shortCode));

            var shortUrl = new ShortUrl
            {
                OriginalUrl = originalUrl,
                ShortCode = shortCode,
                CreatedByUserId = userId
            };

            _context.ShortUrls.Add(shortUrl);
            await _context.SaveChangesAsync();

            return shortUrl;
        }

        public async Task<string> RedirectAsync(string shortCode)
        {
            var shortUrl = await _context.ShortUrls
                .FirstOrDefaultAsync(u => u.ShortCode == shortCode);

            if(shortUrl == null)
            {
                throw new KeyNotFoundException("Short URL not found");
            }

            return shortUrl.OriginalUrl;
        }

        public async Task<bool> DeleteShortUrlAsync(Guid urlId, Guid userId)
        {
            var shortUrl = await _context.ShortUrls
                .Include(u => u.CreatedBy)
                .FirstOrDefaultAsync(u => u.Id == urlId);

            if (shortUrl == null) return false;

            var currentUser = await _context.Users.FindAsync(userId);
            if (currentUser == null) return false;

            if (currentUser.Role != UserRole.Admin &&
               shortUrl.CreatedByUserId != userId)
                return false;

            _context.ShortUrls.Remove(shortUrl);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<ShortUrl>> GetAllUrlsAsync()
        {
            return await _context.ShortUrls
                .Include(u => u.CreatedBy)
                .ToListAsync();
        }

        public async Task<ShortUrl?> GetByIdUrl(Guid urlId)
        {
            return await _context.ShortUrls
                .Include(u => u.CreatedBy)
                .FirstOrDefaultAsync(u => u.Id == urlId);
        }
    }
}
