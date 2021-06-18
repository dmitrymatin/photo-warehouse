using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
using SixLabors.ImageSharp;

namespace PhotoWarehouseApp.Pages.Admin.Photos
{
    public class CreateMultipleModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IPhotoRepository _photoRepository;
        private readonly IFileFormatRepository _fileFormatRepository;
        private readonly IPhotoSizeRepository _photoSizeRepository;

        public CreateMultipleModel(ApplicationDbContext context,
            IConfiguration configuration,
            IWebHostEnvironment webHostEnvironment,
            IPhotoRepository photoRepository,
            IFileFormatRepository fileFormatRepository,
            IPhotoSizeRepository photoSizeRepository)
        {
            _context = context;
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
            _photoRepository = photoRepository;
            _fileFormatRepository = fileFormatRepository;
            _photoSizeRepository = photoSizeRepository;
        }

        public class InputModel
        {
            [Display(Name = "Choose category that will be applied to all photos")]
            public int CommonCategoryId { get; set; }

            [Display(Name = "Date on which the photos were taken. This will be applied to all photos")]
            public DateTimeOffset? CommonDateTaken { get; set; }

            public IEnumerable<SelectListItem> Categories { get; set; }

            //[Display(Name = "Upload a photo")]
            //public IList<IFormFile> Photos { get; set; }

            //public Photo Photo { get; set; } // to use in razor to define fields
        }


        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        [TempData]
        public string ImageError { get; set; }

        [TempData]
        public string NoFilesAddedError { get; set; }

        public IActionResult OnGet()
        {

            Input.Categories = new SelectList(_context.PhotoCategories, "Id", "Name");
            return Page();
        }

        //[BindProperty]
        //public Photo Photo { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Request.Form.Files.Count == 0)
            {
                TempData["NoFilesAddedError"] = "On this page you are required to provide at least one photo.";
                return RedirectToPage();
            }

            foreach (var formFile in Request.Form.Files)
            {
                if (Request.Form.Files.Count > 0)
                {
                    // test
                }
                string formFileName = ContentDispositionHeaderValue.Parse(formFile.ContentDisposition).FileName;
                formFileName = FileService.EnsureCorrectFileName(formFileName);
                string extension = FileService.GetFileExtension(formFileName);

                string filename = $"{Guid.NewGuid()}{extension}";
                string absolutePath = FileService.GetUserImageAbsolutePath(_webHostEnvironment, _configuration, filename);

                if (!formFile.IsImage(extension))
                {
                    TempData["ImageError"] = "The file you submitted is not an image. Only image files can be uploaded.";
                    return RedirectToPage();
                }

                var fileFormat = _fileFormatRepository.GetByName(extension);
                if (fileFormat is null)
                {
                    fileFormat = new FileFormat { Name = extension };
                    _fileFormatRepository.Add(fileFormat);
                    _fileFormatRepository.Commit();
                }

                using var image = Image.Load(formFile.OpenReadStream());

                var photoSize = _photoSizeRepository.GetByDimensions(image.Width, image.Height);
                if (photoSize is null)
                {
                    photoSize = new PhotoSize { Width = image.Width, Height = image.Height };
                    _photoSizeRepository.Add(photoSize);
                    _photoSizeRepository.Commit();
                }

                var photo = new Photo
                {
                    CategoryId = Input.CommonCategoryId,
                    InitialUploadDate = DateTimeOffset.UtcNow,
                    DateTaken = Input.CommonDateTaken ?? DateTimeOffset.UtcNow, // consider DateTaken to be nullable
                };

                var photoItem = new PhotoItem
                {
                    DateUploaded = DateTimeOffset.UtcNow,
                    Path = filename,
                    Photo = photo,
                    Size = photoSize,
                    FileFormat = fileFormat
                };

                photo.PhotoItems = new List<PhotoItem> { photoItem };

                await _photoRepository.AddPhotoItemAsync(formFile.OpenReadStream(), absolutePath, photoItem);

                _photoRepository.Add(photo);


            }
            _photoRepository.Commit();

            return RedirectToPage("/Index");

        }
    }
}
