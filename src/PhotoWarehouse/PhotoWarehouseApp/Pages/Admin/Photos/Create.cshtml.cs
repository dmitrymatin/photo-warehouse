using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PhotoWarehouse.Data;
using PhotoWarehouse.Domain.Photos;

namespace PhotoWarehouseApp.Pages.Admin.Photos
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CreateModel(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult OnGet()
        {
            ViewData["Category"] = new SelectList(_context.PhotoCategories, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Photo Photo { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Photo.InitialUploadDate = DateTimeOffset.UtcNow;
            if (Request.Form.Files.Count > 0)
            {
                IFormFile photoItem = Request.Form.Files[0];
                string filename = ContentDispositionHeaderValue.Parse(photoItem.ContentDisposition).FileName.Trim('"');
                filename = EnsureCorrectFilename(filename);
                using (FileStream output = System.IO.File.Create(this.GetPathAndFilename(filename)))
                {
                    await photoItem.CopyToAsync(output);
                }

                _context.FileFormats.FirstOrDefault(f => f.Name == "gif");
                //Photo.PhotoItems.ToList().Add(new PhotoItem() { DateUploaded = DateTimeOffset.UtcNow });
            }
            _context.Photos.Add(Photo);
            //await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        private string EnsureCorrectFilename(string filename)
        {
            if (filename.Contains("\\"))
                filename = filename.Substring(filename.LastIndexOf("\\") + 1);

            return filename;
        }

        private string GetPathAndFilename(string filename)
        {
            return _webHostEnvironment.WebRootPath + "\\userImages\\" + filename;
        }
    }
}
