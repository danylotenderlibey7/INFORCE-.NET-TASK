using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using INFORCE_.NET_TASK.Data;
using INFORCE_.NET_TASK.Models;
using INFORCE_.NET_TASK.Services;
using INFORCE_.NET_TASK.Services.Interfaces;

namespace INFORCE_.NET_TASK.Controllers
{
    public class ShortUrlsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthService _authService;

        public ShortUrlsController(
            ApplicationDbContext context,
            IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shortUrl = await _context.ShortUrls
                .Include(s => s.CreatedBy)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (shortUrl == null)
            {
                return NotFound();
            }

            return View(shortUrl);
        }
    }
}