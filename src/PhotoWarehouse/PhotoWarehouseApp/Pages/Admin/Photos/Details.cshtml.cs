using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PhotoWarehouse.Data;
using PhotoWarehouse.Domain.Photos;

namespace PhotoWarehouseApp.Pages.Admin.Photos
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Photo Photo { get; set; }

        public async Task<IActionResult> OnGetAsync(int photoId)
        {
            Photo = await _context.Photos
                    .Include(p => p.Category)
                    .Include(p => p.PhotoItems)
                        .ThenInclude(pi => pi.FileFormat)
                    .Include(p => p.PhotoItems)
                        .ThenInclude(pi => pi.Size)
                .FirstOrDefaultAsync(m => m.Id == photoId);

            if (Photo == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
