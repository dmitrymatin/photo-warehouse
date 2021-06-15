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
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IPhotoRepository _photoRepository;
        private readonly IFileFormatRepository _fileFormatRepository;

        public CreateModel(
            ApplicationDbContext context,
            IConfiguration configuration,
            IWebHostEnvironment webHostEnvironment,
            IPhotoRepository photoRepository,
            IFileFormatRepository fileFormatRepository)
        {
            _context = context;
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
            _photoRepository = photoRepository;
            _fileFormatRepository = fileFormatRepository;
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
                filename = FileService.EnsureCorrectFileName(filename);
                string extension = FileService.GetFileExtension(filename);
                
                filename = $"{Guid.NewGuid()}{extension}";
                string absolutePath = FileService.GetUserImageAbsolutePath(_webHostEnvironment, _configuration, filename);

                if (!formPhoto.IsImage(extension))
                {
                    TempData["ImageError"] = "Image file is not correct";
                    return Page();
                }

                var fileFormat = _fileFormatRepository.GetByName(extension);
                if (fileFormat is null)
                {
                    fileFormat = new FileFormat { Name = extension };
                    _fileFormatRepository.Add(fileFormat);
                    _fileFormatRepository.Commit();
                }

                var photoItem = new PhotoItem
                {
                    DateUploaded = DateTimeOffset.UtcNow,
                    Path = filename,
                    Photo = Photo,
                    // size???
                    FileFormat = fileFormat
                };
                Photo.PhotoItems = new List<PhotoItem> { photoItem };

                await _photoRepository.AddPhotoItemAsync(formPhoto.OpenReadStream(), absolutePath, photoItem);
            }
            _photoRepository.Add(Photo);

            _photoRepository.Commit();

            return RedirectToPage("/Index");
        }
    }
}
