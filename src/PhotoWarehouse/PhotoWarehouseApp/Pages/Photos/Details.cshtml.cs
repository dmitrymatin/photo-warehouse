using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PhotoWarehouse.Data;
using PhotoWarehouse.Data.Repositories;
using PhotoWarehouse.Domain.Photos;
using PhotoWarehouse.Domain.Users;
using PhotoWarehouseApp.Areas.Identity;
using PhotoWarehouseApp.Services;

namespace PhotoWarehouseApp.Pages.Photos
{
    public class DetailsModel : PageModel
    {
        private readonly IPhotoRepository _photoRepository;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DetailsModel(IPhotoRepository photoRepository,
            IConfiguration configuration,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            IWebHostEnvironment webHostEnvironment)
        {
            _photoRepository = photoRepository;
            _configuration = configuration;
            _userManager = userManager;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public class PhotoItemsInputModel
        {
            public int Id { get; set; }
            public string SizeAndFormat { get; set; }
        }

        public Photo Photo { get; set; }
        public PhotoItem PhotoItemFirst { get; set; }

        [BindProperty]
        public PhotoItem PhotoItemChosen { get; set; }
        public IEnumerable<PhotoItemsInputModel> PhotoItemsInputModels { get; set; }
        public SelectList PhotoItemsList { get; set; }

        public async Task<IActionResult> OnGet(int photoId)
        {
            Photo = await _photoRepository.GetPhotoAsync(photoId, true, true, true);
            PhotoItemFirst = Photo?.PhotoItems?.FirstOrDefault();

            bool userIsClient = User.IsInRole(Roles.Client.ToString());

            if (Photo is null || (userIsClient && PhotoItemFirst is null))
            {
                ViewData["PhotoNotFoundMessage"] = "«апрошенна€ фотографи€ не существует или не полностью опубликована.";
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

            if (userIsClient)
            {
                var photo = await _photoRepository.GetPhotoAsync(photoId, false, false, false);
                photo.ViewCount++;
                var photoEntry = _context.Entry(photo).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }

            return Page();
        }

        public IActionResult OnPostDownload()
        {
            if (!ModelState.IsValid || !User.IsInRole(Roles.Administrator.ToString()))
            {
                return RedirectToPage("/Error");
            }

            var chosenPhotoItemId = PhotoItemChosen.Id;
            var chosenPhotoItem = _context.PhotoItems.Find(chosenPhotoItemId);
            string path = FileService.GetUserImageContentPath(_configuration, chosenPhotoItem.FileName);
            return File(path, MediaTypeNames.Application.Octet, chosenPhotoItem.FileName);
        }
    }
}
