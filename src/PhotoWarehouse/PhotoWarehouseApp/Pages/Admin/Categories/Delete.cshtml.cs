using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PhotoWarehouse.Data;
using PhotoWarehouse.Domain.Photos;
using System.Threading.Tasks;

namespace PhotoWarehouseApp.Pages.Admin.Categories
{
    [Authorize(Roles = "Administrator")]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public PhotoCategory PhotoCategory { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PhotoCategory = await _context.PhotoCategories.FirstOrDefaultAsync(m => m.Id == id);

            if (PhotoCategory == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PhotoCategory = await _context.PhotoCategories.FindAsync(id);

            if (PhotoCategory != null)
            {
                _context.PhotoCategories.Remove(PhotoCategory);
                try
                {
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateException)
                {
                    return RedirectToPage("/Error");
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
