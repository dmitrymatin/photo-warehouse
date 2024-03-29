using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PhotoWarehouse.Data;
using PhotoWarehouse.Data.Repositories;
using PhotoWarehouse.Domain.Photos;
using PhotoWarehouse.Domain.Users;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoWarehouseApp.Pages.Photos.Basket
{
    public class AddModel : PageModel
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IPhotoRepository photoRepository;

        public AddModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IPhotoRepository photoRepository)
        {
            this.context = context;
            this.userManager = userManager;
            this.photoRepository = photoRepository;
        }

        [BindProperty]
        public Photo Photo { get; set; } // TODO: improve bindings by only binding Id's instead of creating new objects

        [BindProperty]
        public PhotoItem PhotoItemChosen { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return RedirectToPage("/Error");
            }

            string userName = User.Identity?.Name;
            if (userName is null)
            {
                return RedirectToPage("/Error");
            }

            var user = await userManager.FindByNameAsync(userName);


            var userEntry = context.Users
                .Include(u => u.PhotoItemsInBasket)
                .FirstOrDefault(u => u.Id == user.Id);

            var chosenPhotoItemEntry = (await photoRepository
                .GetPhotoItemsAsync(Photo.Id)).FirstOrDefault(pi => pi.Id == PhotoItemChosen.Id);
            userEntry.PhotoItemsInBasket.Add(chosenPhotoItemEntry);
            context.SaveChanges();

            TempData["AddedToBasketMessage"] = "����������� ��������� � �������.";

            return RedirectToPage("/Photos/Details", new { photoId = Photo.Id });
        }
    }
}
