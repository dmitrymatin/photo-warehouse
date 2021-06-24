using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using PhotoWarehouse.Data.Repositories;
using PhotoWarehouse.Domain.Photos;
using PhotoWarehouseApp.Services;

namespace PhotoWarehouseApp.Pages.Photos
{
    public class DetailsModel : PageModel
    {
        private readonly IPhotoRepository _photoRepository;
        private readonly IConfiguration _configuration;

        public DetailsModel(IPhotoRepository photoRepository, IConfiguration configuration)
        {
            _photoRepository = photoRepository;
            _configuration = configuration;
        }

        public class PhotoItemsInputModel
        {
            public int Id { get; set; }
            public string SizeAndFormat { get; set; }
        }

        public Photo Photo { get; set; }
        public PhotoItem PhotoItemFirst { get; set; }
        public PhotoItem PhotoItemChosen { get; set; }
        public IEnumerable<PhotoItemsInputModel> PhotoItemsInputModels { get; set; }
        public SelectList PhotoItemsList { get; set; }

        public async Task<IActionResult> OnGet(int photoId)
        {
            Photo = await _photoRepository.GetPhotoAsync(photoId, true, true, true);
            PhotoItemFirst = Photo?.PhotoItems?.FirstOrDefault();

            if (Photo is null || PhotoItemFirst is null)
            {
                ViewData["PhotoNotFoundMessage"] = "Sorry, we couldn't find the photo or it is not fully set up";
                return Page();
            }

            PhotoItemsInputModels = Photo.PhotoItems
                .Select(pi => new PhotoItemsInputModel
                {
                    Id = pi.Id,
                    SizeAndFormat = $"{pi.Size} ({pi.FileFormat})"
                });


            PhotoItemsList = new SelectList(PhotoItemsInputModels, "Id", "SizeAndFormat");

            PhotoItemFirst.RelativePath = FileService.GetUserImageContentPath(_configuration, PhotoItemFirst.FileName);

            return Page();
        }
    }
}
