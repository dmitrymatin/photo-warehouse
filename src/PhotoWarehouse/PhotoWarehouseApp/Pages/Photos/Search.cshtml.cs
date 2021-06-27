using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using PhotoWarehouse.Data.Repositories;
using PhotoWarehouse.Domain.Photos;
using PhotoWarehouseApp.Services;

namespace PhotoWarehouseApp.Pages.Photos
{
    public class SearchModel : PageModel
    {
        private readonly IPhotoRepository _photoRepository;
        private readonly IConfiguration _configuration;

        public SearchModel(IPhotoRepository photoRepository, IConfiguration configuration)
        {
            _photoRepository = photoRepository;
            _configuration = configuration;
        }

        public class ProjectionModel
        {
            public Photo Photo { get; set; }
            public PhotoItem PhotoItemFirst { get; set; }
        }

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public IEnumerable<ProjectionModel> FoundPhotos { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            FoundPhotos = (await _photoRepository.GetPhotosAsync(SearchTerm))
                .Select(p => new ProjectionModel { Photo = p, PhotoItemFirst = p.PhotoItems.FirstOrDefault() });

            foreach (var photo in FoundPhotos)
            {
                photo.PhotoItemFirst.RelativePath = FileService.GetUserImageContentPath(_configuration, photo.PhotoItemFirst.FileName);
            }

            if (!FoundPhotos.Any())
            {
                ViewData["NoSearchResultsMessage"] = "ѕо данному поисковому запросу ничего не найдено";
            }


            return Page();
        }

        public async Task<IActionResult> OnGetCategoryAsync()
        {
            FoundPhotos = (await _photoRepository.GetPhotosInCategoryAsync(SearchTerm))
                .Select(p => new ProjectionModel { Photo = p, PhotoItemFirst = p.PhotoItems.FirstOrDefault() });

            foreach (var photo in FoundPhotos)
            {
                photo.PhotoItemFirst.RelativePath = FileService.GetUserImageContentPath(_configuration, photo.PhotoItemFirst.FileName);
            }

            if (!FoundPhotos.Any())
            {
                ViewData["NoSearchResultsMessage"] = "ѕо данному поисковому запросу ничего не найдено";
            }


            return Page();
        }
    }
}
