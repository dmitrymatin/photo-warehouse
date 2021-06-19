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

            [TempData]
            public string ImageError { get; set; }
        }

        public enum PhotoItemStatus { Unmodified = 0, Deleted = 1 }

        public class PhotoItemData
        {
            public PhotoItem PhotoItem { get; set; }
            public PhotoItemStatus PhotoItemStatus { get; set; }
        }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

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

            //var photoToUpdate = await _context.Photos
            //    .Include(p => p.PhotoItems)
            //    .AsNoTracking()
            //    .FirstOrDefaultAsync(p => p.Id == Input.Photo.Id);

            bool photoExists = _photoRepository.PhotoExists(Input.Photo.Id);
            if (!photoExists)
            {
                TempData["ImageError"] = "An error occured while saving changes: photo entry not found";
                return RedirectToPage();
            }

            _photoRepository.UpdatePhoto(Input.Photo);

            var photoItems = await _photoRepository.GetPhotoItemsAsync(Input.Photo.Id);
            Input.Photo.PhotoItems = photoItems;


            //_photoRepository.UpdatePhoto(Input.Photo);

            if (Input.PhotoItemData is null)
            {
                _photoRepository.DeletePhotoItems(photoItems.ToArray());
                //photoItems.Clear();
            }
            else
            {
                foreach (var oldPhotoItem in photoItems.ToList() /* ?? Enumerable.Empty<PhotoItem>()*/)
                {
                    var matchingPhotoItemData = Input.PhotoItemData
                        .First(photoItemData => photoItemData.PhotoItem.Id == oldPhotoItem.Id);

                    if (matchingPhotoItemData.PhotoItemStatus == PhotoItemStatus.Deleted)
                    {
                        _photoRepository.DeletePhotoItems(oldPhotoItem);
                        //photoItems.Remove(oldPhotoItem);
                    }
                }
            }

            var initialPhotoName = photoItems.FirstOrDefault();
            var photoItemsCount = photoItems.Count;
            foreach (var formFile in Request.Form.Files)
            {
                string formFileName = ContentDispositionHeaderValue.Parse(formFile.ContentDisposition).FileName;
                formFileName = FileService.EnsureCorrectFileName(formFileName);
                string extension = FileService.GetFileExtension(formFileName);

                string filename = initialPhotoName is not null
                    ? $"{initialPhotoName}-{++photoItemsCount}{extension}"
                    : $"{Guid.NewGuid()}{extension}";

                string absolutePath = FileService.GetUserImageAbsolutePath(_webHostEnvironment, _configuration, filename);

                if (!formFile.IsImage(extension))
                {
                    TempData["ImageError"] = $"The file {formFileName} you submitted is not an image. Only image files can be uploaded.";
                    return RedirectToPage();
                }

                var fileFormat = _fileFormatRepository.GetByName(extension);
                if (fileFormat is null)
                {
                    fileFormat = new FileFormat { Name = extension };
                    _fileFormatRepository.Add(fileFormat);
                    _fileFormatRepository.Commit(); // consider one call to commit() call
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

                await _photoRepository.AddPhotoItemAsync(formFile.OpenReadStream(), absolutePath, photoItem);
            }
            //Input.Photo.PhotoItems = photoItems;
            //_photoRepository.UpdatePhoto(Input.Photo);
            _photoRepository.Commit();

            return RedirectToPage(new { photoId = Input.Photo.Id });
        }
    }
}
