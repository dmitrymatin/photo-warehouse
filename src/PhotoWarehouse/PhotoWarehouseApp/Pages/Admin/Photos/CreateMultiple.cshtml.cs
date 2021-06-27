using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Administrator")]
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
            [Display(Name = "Выберите категорию, которая будет применена ко всем загружаемым фотографиям")]
            public int CommonCategoryId { get; set; }

            [Display(Name = "Дата создания фотографий. При указании применяется ко всем загружаемым фотографиям")]
            public DateTimeOffset? CommonDateTaken { get; set; }

            public IEnumerable<SelectListItem> Categories { get; set; }
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

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Request.Form.Files.Count == 0)
            {
                TempData["NoFilesAddedError"] = "Необходимо добавить хотя бы одну фотографию.";
                return RedirectToPage();
            }

            foreach (var formFile in Request.Form.Files)
            {
                string formFileName = ContentDispositionHeaderValue.Parse(formFile.ContentDisposition).FileName;
                formFileName = FileService.EnsureCorrectFileName(formFileName);
                string extension = FileService.GetFileExtension(formFileName);

                string filename = $"{Guid.NewGuid()}{extension}";
                string absolutePath = FileService.GetUserImageAbsolutePath(_webHostEnvironment, _configuration, filename);

                if (!formFile.IsImage(extension))
                {
                    TempData["ImageError"] = $"Загруженный файл ({formFileName}) не является изображением.";
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
                    InitialUploadDate = DateTimeOffset.Now,
                    DateTaken = Input.CommonDateTaken,
                };

                var photoItem = new PhotoItem
                {
                    DateUploaded = DateTimeOffset.Now,
                    FileName = filename,
                    Photo = photo,
                    Size = photoSize,
                    FileFormat = fileFormat
                };

                photo.PhotoItems = new List<PhotoItem> { photoItem };

                await _photoRepository.AddPhotoItemAsync(formFile.OpenReadStream(), absolutePath, photoItem);

                _photoRepository.AddPhoto(photo);


            }
            _photoRepository.Commit();

            return RedirectToPage("/Index");

        }
    }
}
