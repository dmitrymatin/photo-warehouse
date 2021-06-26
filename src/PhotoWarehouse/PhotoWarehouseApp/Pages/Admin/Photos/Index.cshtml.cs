using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PhotoWarehouse.Data;
using PhotoWarehouse.Domain.Photos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhotoWarehouseApp.Pages.Admin.Photos
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Photo> Photo { get; set; }

        public async Task OnGetAsync()
        {
            Photo = await _context.Photos
                .Include(p => p.Category).ToListAsync();
        }
    }
}
