using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PhotoWarehouse.Data;
using PhotoWarehouse.Domain.Photos;

namespace PhotoWarehouseApp.Pages.Admin.Categories
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<PhotoCategory> PhotoCategory { get;set; }

        public async Task OnGetAsync()
        {
            PhotoCategory = await _context.PhotoCategories.ToListAsync();
        }
    }
}
