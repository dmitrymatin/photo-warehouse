using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PhotoWarehouse.Data;
using PhotoWarehouse.Domain.Photos;

namespace PhotoWarehouseApp.Pages.Admin.Categories
{
    public class CreateModel : PageModel
    {
        private readonly PhotoWarehouse.Data.ApplicationDbContext _context;

        public CreateModel(PhotoWarehouse.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public PhotoCategory PhotoCategory { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.PhotoCategories.Add(PhotoCategory);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
