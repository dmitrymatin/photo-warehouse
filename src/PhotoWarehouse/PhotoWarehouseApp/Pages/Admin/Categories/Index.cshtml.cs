﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PhotoWarehouse.Data;
using PhotoWarehouse.Domain.Photos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhotoWarehouseApp.Pages.Admin.Categories
{
    [Authorize(Roles = "Administrator")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<PhotoCategory> PhotoCategory { get; set; }

        public async Task OnGetAsync()
        {
            PhotoCategory = await _context.PhotoCategories.ToListAsync();
        }
    }
}
