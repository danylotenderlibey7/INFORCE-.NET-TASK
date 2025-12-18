using INFORCE_.NET_TASK.Data;
using INFORCE_.NET_TASK.Models;
using INFORCE_.NET_TASK.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace INFORCE_.NET_TASK.Controllers
{
    public class AboutController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthService _authService;

        public AboutController(
            ApplicationDbContext context,
            IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<IActionResult> Index()
        {
            var about = await _context.Abouts.FirstOrDefaultAsync();
            if (about == null)
            {
                about = new About
                {
                    Title = "URL Shortener Algorithm",
                    Description = "...",
                    LastUpdated = DateTime.UtcNow
                };
                _context.Abouts.Add(about);
                await _context.SaveChangesAsync();
            }

            ViewBag.CanEdit = _authService.CanEditAbout();
            return View(about);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(About model)
        {
            if (!_authService.CanEditAbout())
                return Forbid();

            if (ModelState.IsValid)
            {
                var about = await _context.Abouts.FirstOrDefaultAsync();
                if (about != null)
                {
                    about.Title = model.Title;
                    about.Description = model.Description;
                    about.LastUpdated = DateTime.UtcNow;
                    about.UpdatedByUserId = (await _authService.GetCurrentUserAsync())?.Id;

                    _context.Update(about);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.CanEdit = true;
            return View("Index", model);
        }
    }
}
