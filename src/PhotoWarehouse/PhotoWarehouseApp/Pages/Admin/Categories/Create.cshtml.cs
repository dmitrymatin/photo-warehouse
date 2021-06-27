using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PhotoWarehouse.Data;
using PhotoWarehouse.Domain.Photos;
using System.Threading.Tasks;

namespace PhotoWarehouseApp.Pages.Admin.Categories
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
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
