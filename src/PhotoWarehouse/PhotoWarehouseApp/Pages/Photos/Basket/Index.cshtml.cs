using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
using PhotoWarehouseApp.Services;

namespace PhotoWarehouseApp.Pages.Photos.Basket
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext context;
        private readonly IConfiguration configuration;
        private readonly IPhotoRepository photoRepository;

        public IndexModel(UserManager<
            ApplicationUser> userManager,
            ApplicationDbContext context,
            IConfiguration configuration,
            IPhotoRepository photoRepository)
        {
            this.userManager = userManager;
            this.context = context;
            this.configuration = configuration;
            this.photoRepository = photoRepository;
        }

        public enum PhotoItemStatus { Unmodified = 0, Deleted = 1 }

        public class PhotoItemsInputModel
        {
            public Photo Photo { get; set; } // or PHOTO ITEM
            public string ThumbnailPath { get; set; }
            public IEnumerable<PhotoItemToChoose> PhotoItemsToChoose { get; set; }
            public SelectList SizeAndFormatSelectList { get; set; }
            public PhotoItem ChosenPhotoItem { get; set; }
            public PhotoItemStatus PhotoItemStatus { get; set; }
        }

        public class PhotoItemToChoose
        {
            public int PhotoItemId { get; set; }
            public string SizeAndFormat { get; set; }
        }

        [TempData]
        public string ErrorMessage { get; set; }

        [TempData]
        public string SuccessMessage { get; set; }

        [BindProperty]
        public IList<PhotoItemsInputModel> Input { get; set; }

        public async Task OnGetAsync()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);

            var userEntry = await context.Users
                .Include(u => u.PhotoItemsInBasket)
                    .ThenInclude(pi => pi.Size)
                .Include(u => u.PhotoItemsInBasket)
                    .ThenInclude(pi => pi.FileFormat)
                .Include(u => u.PhotoItemsInBasket)
                    .ThenInclude(pi => pi.Photo)
                        .ThenInclude(p => p.Category)
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            Input = userEntry.PhotoItemsInBasket.Select(pi => new PhotoItemsInputModel
            {
                Photo = pi.Photo,
                ThumbnailPath = FileService.GetUserImageContentPath(configuration, pi.FileName),
                //PhotoItemsToChoose = pi.Photo.PhotoItems.Select(pi => new PhotoItemToChoose
                PhotoItemsToChoose = photoRepository.GetPhotoItems(pi.Photo.Id).Select(pi => new PhotoItemToChoose
                {
                    PhotoItemId = pi.Id,
                    SizeAndFormat = $"{pi.Size} ({pi.FileFormat})",
                }),
                ChosenPhotoItem = pi,
                PhotoItemStatus = PhotoItemStatus.Unmodified
            }).ToList();

            foreach (var inputItem in Input)
            {
                inputItem.SizeAndFormatSelectList = new SelectList(inputItem.PhotoItemsToChoose, "PhotoItemId", "SizeAndFormat");
            }

            if (!Input.Any())
            {
                ViewData["EmptyBasketMessage"] = "�� ������ ������ ������� �����";
            }
        }

        public async Task<IActionResult> OnPostSaveAsync()
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "��� ���������� ��������� � ������� �������� ������. ��������� ������������ ��������� ������ � ���������� �����";
                return RedirectToPage();
            }

            var postData = Input.Select(x =>
                new { ChosenPhotoItemId = x.ChosenPhotoItem.Id, x.PhotoItemStatus });

            var user = await userManager.Users
                .Include(u => u.PhotoItemsInBasket)
                .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            //context.Entry(user).State = EntityState.Detached;

            //var userEntry = await context.Users
            //    .AsNoTracking()
            //    .Include(u => u.PhotoItemsInBasket)
            //    .FirstOrDefaultAsync(u => u.Id == user.Id);

            //var userPhotoItems = await photoRepository.GetUserPhotoItemsAsync(user.Id);
            //user.PhotoItemsInBasket = userPhotoItems;

            foreach (var postDataItem in postData)
            {
                var matchingPhotoItem = user.PhotoItemsInBasket.FirstOrDefault(pi => pi.Id == postDataItem.ChosenPhotoItemId);

                if (matchingPhotoItem is not null)
                {
                    if (postDataItem.PhotoItemStatus == PhotoItemStatus.Deleted)
                    {
                        //var deletedPhotoItem = context.PhotoItems.Attach(photoItem);
                        var matchingPhotoItemEntry = context.Attach(matchingPhotoItem);
                        matchingPhotoItemEntry.State = EntityState.Deleted;
                        //user.PhotoItemsInBasket.Remove(matchingPhotoItem);
                    }
                }
                else
                {
                    // ChosenPhotoItem was added as another version of picture
                    
                    var chosenPhotoItem = context.PhotoItems
                        .AsNoTracking()
                        .Include(pi => pi.Photo)
                        .FirstOrDefault(pi => pi.Id == postDataItem.ChosenPhotoItemId);
                    var chosenPhoto = chosenPhotoItem.Photo;

                    var relatedPhotoItems = context.PhotoItems
                        .AsNoTracking()
                        .Include(pi => pi.Photo)
                        .Where(pi => pi.PhotoId == chosenPhoto.Id)
                        .ToList();

                    var photoItemToAdd = relatedPhotoItems.FirstOrDefault(pi => pi.Id == postDataItem.ChosenPhotoItemId);
                    if (photoItemToAdd is not null)
                    {
                        matchingPhotoItem = photoItemToAdd;
                    }
                    else
                    {

                    }
                }

                if (matchingPhotoItem is null)
                {
                    TempData["ErrorMessage"] = "��� ���������� ��������� � ������� �������� ������. ��������� ������������ ��������� ������ � ���������� �����";
                    return RedirectToPage();
                }

                user.PhotoItemsInBasket.Add(matchingPhotoItem);
            }

            context.SaveChanges();

            TempData["SuccessMessage"] = "��������� ������� ���������.";

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostOrderAsync()
        {
            return Page();
        }
    }
}
