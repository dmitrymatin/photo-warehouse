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

        public IndexModel(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.context = context;
            this.configuration = configuration;
        }

        //public class InputModel
        //{
        //    //public Photo Photo { get; set; }


        //}

        //[BindProperty]
        //public InputModel Input { get; set; }


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

        //public ApplicationUser userEntry { get; set; }

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
                PhotoItemsToChoose = pi.Photo.PhotoItems.Select(pi => new PhotoItemToChoose
                {
                    PhotoItemId = pi.Id,
                    SizeAndFormat = $"{pi.Size} ({pi.FileFormat})",
                }),
                ChosenPhotoItem = pi,
                PhotoItemStatus = PhotoItemStatus.Unmodified

                //SizeAndFormatSelectList = new SelectList(PhotoItemsT, "Id", "")
                //SizeAndFormatSelectList = new SelectList(pi.Photo.PhotoItems.Select(pi =>
                //    new ChosenPhotoItem 
                //    { 
                //        PhotoItemId = pi.Id, SizeAndFormat = $"{pi.Size} ({pi.FileFormat})" 
                //    }), "Id", "SizeAndFormat"
                //),
            }).ToList();

            foreach (var inputItem in Input)
            {
                inputItem.SizeAndFormatSelectList = new SelectList(inputItem.PhotoItemsToChoose, "PhotoItemId", "SizeAndFormat");
            }

            if (!Input.Any())
            {
                ViewData["EmptyBasketMessage"] = "На данный момент корзина пуста";
            }



            //PhotoItemsInputModels = Photo.PhotoItems
            //    .Select(pi => new PhotoItemsInputModel
            //    {
            //        Id = pi.Id,
            //        SizeAndFormat = $"{pi.Size} ({pi.FileFormat})"
            //    });

            //IEnumerable<PhotoItem> basketItems = userEntry.PhotoItemsInBasket;

        }
    }
}
