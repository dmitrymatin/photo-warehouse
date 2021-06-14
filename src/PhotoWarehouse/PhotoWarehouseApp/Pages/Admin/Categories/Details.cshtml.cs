﻿using System;
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
    public class DetailsModel : PageModel
    {
        private readonly PhotoWarehouse.Data.ApplicationDbContext _context;

        public DetailsModel(PhotoWarehouse.Data.ApplicationDbContext context)
        {
            _context = context;
        }

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
    }
}
