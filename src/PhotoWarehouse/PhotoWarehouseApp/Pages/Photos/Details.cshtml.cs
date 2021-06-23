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

        public Photo Photo { get; set; }
        public PhotoItem PhotoItemFirst { get; set; }
        public SelectList AvailableFormats { get; set; }
        public SelectList AvailableSizes { get; set; }

        public async Task<IActionResult> OnGet(int photoId)
        {
            Photo = await _photoRepository.GetPhotoAsync(photoId, true, true, true);
            PhotoItemFirst = Photo.PhotoItems?.FirstOrDefault();

            if (Photo is null || PhotoItemFirst is null)
            {
                ViewData["NoSearchResultsMessage"] = "Sorry, we couldn't find anything that fit your search";
                return Page();
            }

            AvailableFormats = new SelectList(Photo.PhotoItems.Select(pi => pi.FileFormat), "Id", "Name");
            AvailableSizes = new SelectList(Photo.PhotoItems.Select(pi => pi.Size), "Id", "");

            PhotoItemFirst.RelativePath = FileService.GetUserImageContentPath(_configuration, PhotoItemFirst.FileName);

            return Page();
        }
    }
}
