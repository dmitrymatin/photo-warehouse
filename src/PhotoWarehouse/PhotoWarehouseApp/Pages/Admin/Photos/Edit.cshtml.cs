using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PhotoWarehouse.Data;
using PhotoWarehouse.Domain.Photos;
using PhotoWarehouseApp.Services;

namespace PhotoWarehouseApp.Pages.Admin.Photos
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public EditModel(ApplicationDbContext context,
            IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public class InputModel
        {
            public Photo Photo { get; set; }

            public IEnumerable<SelectListItem> Categories { get; set; }

            [TempData]
            public string ImageError { get; set; }
        }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public async Task<IActionResult> OnGetAsync(int photoId)
        {
            Input.Photo = await _context.Photos
                 .Include(p => p.Category)
                 .Include(p => p.PhotoItems)
                     .ThenInclude(pi => pi.FileFormat)
                 .Include(p => p.PhotoItems)
                     .ThenInclude(pi => pi.Size)
                 .FirstOrDefaultAsync(p => p.Id == photoId);

            Input.Categories = new SelectList(_context.PhotoCategories, "Id", "Name");

            if (Input.Photo == null)
            {
                return NotFound();
            }

            foreach (var photoItem in Input.Photo.PhotoItems ?? Enumerable.Empty<PhotoItem>())
            {
                photoItem.RelativePath = FileService
                    .GetUserImageContentPath(_configuration, photoItem.Path);
            }

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (await TryUpdateModelAsync<Photo>(
                Input.Photo,
                "Photo",
                i => i.PhotoItems, i => i.Name
                ))
            {
                _context.Attach(Input.Photo).State = EntityState.Modified;

            }
            

            _context.Attach(Input.Photo.PhotoItems).State = EntityState.Deleted;



            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhotoExists(Input.Photo.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool PhotoExists(int id)
        {
            return _context.Photos.Any(e => e.Id == id);
        }
    }
}
