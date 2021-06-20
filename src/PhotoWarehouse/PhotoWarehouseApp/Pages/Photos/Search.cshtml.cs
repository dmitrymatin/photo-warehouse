using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PhotoWarehouse.Data.Repositories;
using PhotoWarehouse.Domain.Photos;

namespace PhotoWarehouseApp.Pages.Photos
{
    public class SearchModel : PageModel
    {
        private readonly IPhotoRepository _photoRepository;

        public SearchModel(IPhotoRepository photoRepository)
        {
            _photoRepository = photoRepository;
        }

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public IEnumerable<Photo> FoundPhotos { get; set; }

        public void OnGet()
        {
            FoundPhotos = _photoRepository.GetPhotosAsync(SearchTerm).ToList();
        }
    }
}
