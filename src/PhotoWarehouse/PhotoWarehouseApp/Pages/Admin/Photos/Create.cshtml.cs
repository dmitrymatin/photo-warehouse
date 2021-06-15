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
using Microsoft.Extensions.Configuration;
using PhotoWarehouse.Data;
using PhotoWarehouse.Data.Repositories;
using PhotoWarehouse.Domain.Photos;
using PhotoWarehouseApp.Services;

namespace PhotoWarehouseApp.Pages.Admin.Photos
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;
        private readonly IPhotoRepository _photoRepository;

        public CreateModel(
            ApplicationDbContext context,
            IWebHostEnvironment webHostEnvironment,
            IConfiguration configuration,
            IPhotoRepository photoRepository)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
            _photoRepository = photoRepository;
        }

        [TempData]
        public string ImageError { get; set; }

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
                IFormFile formPhoto = Request.Form.Files[0];

                string filename = ContentDispositionHeaderValue.Parse(formPhoto.ContentDisposition).FileName;
                string path = FileService.EnsureCorrectPathAndFileName(_webHostEnvironment, _configuration, filename);
                string extension = FileService.GetExtension(filename);

                if (!formPhoto.IsImage(extension))
                {
                    TempData["ImageError"] = "Image file not correct";
                    return Page();
                }

                var photoItem = new PhotoItem
                {
                    DateUploaded = DateTimeOffset.UtcNow,
                    Path = path,
                    Photo = Photo,

                };
                //Photo.PhotoItems = new List<PhotoItem> { new PhotoItem { } }

                await _photoRepository.AddPhotoItemAsync(formPhoto.OpenReadStream(), new PhotoItem { }, path);

                _context.FileFormats.FirstOrDefault(f => f.Name == "gif");
                //Photo.PhotoItems.ToList().Add(new PhotoItem() { DateUploaded = DateTimeOffset.UtcNow });
            }
            _context.Photos.Add(Photo);
            //await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
