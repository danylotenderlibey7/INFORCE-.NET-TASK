using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using INFORCE_.NET_TASK.Data;
using INFORCE_.NET_TASK.Models;

namespace INFORCE_.NET_TASK.Views.About
{
    public class IndexModel : PageModel
    {
        private readonly INFORCE_.NET_TASK.Data.ApplicationDbContext _context;

        public IndexModel(INFORCE_.NET_TASK.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Models.About> About { get;set; } = default!;

        public async Task OnGetAsync()
        {
            About = await _context.Abouts
                .Include(a => a.UpdatedBy).ToListAsync();
        }

    }
}
