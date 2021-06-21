using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PhotoWarehouse.Data;
using PhotoWarehouse.Data.Repositories;
using PhotoWarehouse.Domain.Photos;
using PhotoWarehouseApp.Services;
using SixLabors.ImageSharp;

namespace PhotoWarehouseApp.Pages.Admin.Photos
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFileFormatRepository _fileFormatRepository;
        private readonly IPhotoSizeRepository _photoSizeRepository;
        private readonly IPhotoRepository _photoRepository;

        public EditModel(ApplicationDbContext context,
            IConfiguration configuration,
            IWebHostEnvironment webHostEnvironment,
            IFileFormatRepository fileFormatRepository,
            IPhotoSizeRepository photoSizeRepository,
            IPhotoRepository photoRepository)
        {
            _context = context;
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
            _fileFormatRepository = fileFormatRepository;
            _photoSizeRepository = photoSizeRepository;
            _photoRepository = photoRepository;
        }

        public class InputModel
        {
            public Photo Photo { get; set; }

            public IList<PhotoItemData> PhotoItemData { get; set; }

            public IEnumerable<SelectListItem> Categories { get; set; }
        }

        public enum PhotoItemStatus { Unmodified = 0, Deleted = 1 }

        public class PhotoItemData
        {
            public PhotoItem PhotoItem { get; set; }
            public PhotoItemStatus PhotoItemStatus { get; set; }
        }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        [TempData]
        public string ImageError { get; set; }

        public async Task<IActionResult> OnGetAsync(int photoId)
        {
            Input.Photo = await _photoRepository.GetPhotoAsync(photoId,
                includeCategory: true,
                includeFileFormat: true,
                includeSize: true);

            Input.Categories = new SelectList(_context.PhotoCategories, "Id", "Name");

            if (Input.Photo == null)
            {
                return NotFound();
            }

            var photoItems = Input.Photo.PhotoItems ?? Enumerable.Empty<PhotoItem>();

            Input.PhotoItemData = new List<PhotoItemData>(photoItems.Count());

            foreach (var photoItem in photoItems)
            {
                photoItem.RelativePath = FileService
                    .GetUserImageContentPath(_configuration, photoItem.Path);

                Input.PhotoItemData.Add(new PhotoItemData
                {
                    PhotoItem = photoItem,
                    PhotoItemStatus = PhotoItemStatus.Unmodified
                });
            }

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            bool photoExists = _photoRepository.PhotoExists(Input.Photo.Id);
            if (!photoExists)
            {
                TempData["ImageError"] = "An error occured while saving changes: photo entry not found";
                return RedirectToPage();
            }

            _photoRepository.UpdatePhoto(Input.Photo);

            var photoItems = await _photoRepository.GetPhotoItemsAsync(Input.Photo.Id);
            Input.Photo.PhotoItems = photoItems;

            if (Input.PhotoItemData is null)
            {
                _photoRepository.DeletePhotoItems(photoItems.ToArray());
            }
            else
            {
                foreach (var oldPhotoItem in photoItems.ToList())
                {
                    var matchingPhotoItemData = Input.PhotoItemData
                        .First(photoItemData => photoItemData.PhotoItem.Id == oldPhotoItem.Id);

                    if (matchingPhotoItemData.PhotoItemStatus == PhotoItemStatus.Deleted)
                    {
                        _photoRepository.DeletePhotoItems(oldPhotoItem);
                    }
                }
            }

            var photoItemWithGreatestCount = photoItems.OrderBy(pi => pi.Path).LastOrDefault();
            string initialFileNameWithoutExtension = FileService.GetFileNameWithoutExtension(photoItemWithGreatestCount?.Path);

            int photoItemNameCount = 1;


            if (initialFileNameWithoutExtension is null)
            {
                initialFileNameWithoutExtension = Guid.NewGuid().ToString();
            }
            else
            {
                int bracketStart = initialFileNameWithoutExtension.LastIndexOf('(');
                int bracketEnd = initialFileNameWithoutExtension.LastIndexOf(')');

                if (bracketStart != -1 && bracketEnd != -1)
                {
                    string countString = initialFileNameWithoutExtension.Substring(bracketStart + 1, bracketEnd - bracketStart - 1);
                    initialFileNameWithoutExtension = initialFileNameWithoutExtension.Substring(0, bracketStart);
                    photoItemNameCount = int.Parse(countString) + 1;
                }
            }

            foreach (var formFile in Request.Form.Files)
            {
                string formFileName = ContentDispositionHeaderValue.Parse(formFile.ContentDisposition).FileName;
                formFileName = FileService.EnsureCorrectFileName(formFileName);
                string extension = FileService.GetFileExtension(formFileName);

                if (!formFile.IsImage(extension))
                {
                    TempData["ImageError"] = $"The file {formFileName} you submitted is not an image. Only image files can be uploaded.";
                    return RedirectToPage();
                }

                string filename = $"{initialFileNameWithoutExtension}({photoItemNameCount++}){extension}";

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

                var photoItem = new PhotoItem
                {
                    DateUploaded = DateTimeOffset.UtcNow,
                    Path = filename,
                    Photo = Input.Photo,
                    Size = photoSize,
                    FileFormat = fileFormat
                };

                photoItems.Add(photoItem);

                string absolutePath = FileService.GetUserImageAbsolutePath(_webHostEnvironment, _configuration, filename);
                await _photoRepository.AddPhotoItemAsync(formFile.OpenReadStream(), absolutePath, photoItem);
            }

            _photoRepository.Commit();

            return RedirectToPage(new { photoId = Input.Photo.Id });
        }
    }
}
