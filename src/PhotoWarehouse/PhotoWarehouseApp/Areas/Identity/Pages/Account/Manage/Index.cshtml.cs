using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PhotoWarehouse.Domain.Users;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace PhotoWarehouseApp.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Введите имя пользователя")]
            [Display(Name = "Имя пользователя")]
            public string Username { get; set; }

            [Required(ErrorMessage = "Введите адрес электронной почтыпользователя")]
            [EmailAddress(ErrorMessage = "Введенный адрес не является допустимым")]
            [Display(Name = "Адрес электронной почты")]
            public string UserEmail { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var userEmail = await _userManager.GetEmailAsync(user);

            Input = new InputModel
            {
                Username = userName,
                UserEmail = userEmail
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Невозможно загрузить профиль пользователя с ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Невозможно загрузить профиль пользователя с ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var userName = await _userManager.GetUserNameAsync(user);
            var userEmail = await _userManager.GetEmailAsync(user);
            if (Input.Username != userName)
            {
                var setUserNameResult = await _userManager.SetUserNameAsync(user, Input.Username);
                if (!setUserNameResult.Succeeded)
                {
                    StatusMessage = "При сохранении имени пользователя возникла ошибка.";
                    return RedirectToPage();
                }
            }

            if (Input.UserEmail != userEmail)
            {
                var setUserEmailResult = await _userManager.SetEmailAsync(user, Input.UserEmail);
                if (!setUserEmailResult.Succeeded)
                {
                    StatusMessage = "При сохранении адреса электронной почты возникла ошибка.";
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Изменения успешно сохранены!";
            return RedirectToPage();
        }
    }
}
