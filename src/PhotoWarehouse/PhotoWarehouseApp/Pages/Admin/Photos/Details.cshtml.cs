using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PhotoWarehouse.Data;
using PhotoWarehouse.Domain.Photos;
using PhotoWarehouseApp.Services;

namespace PhotoWarehouseApp.Pages.Admin.Photos
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public DetailsModel(ApplicationDbContext context,
            IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public Photo Photo { get; set; }
        IEnumerable<string> PhotoItemPaths { get; set; }

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

            foreach (var photoItem in Photo.PhotoItems ?? Enumerable.Empty<PhotoItem>())
            {
                photoItem.RelativePath = FileService
                    .GetUserImageContentPath(_configuration, photoItem.Path);
            }


            return Page();
        }
    }
}
