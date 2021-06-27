using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PhotoWarehouse.Data;
using PhotoWarehouse.Domain.Orders;
using PhotoWarehouse.Domain.Users;
using PhotoWarehouseApp.Areas.Identity;
using PhotoWarehouseApp.Services;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace PhotoWarehouseApp.Pages.Photos.Orders
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration configuration;

        public DetailsModel(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            this.context = context;
            this.userManager = userManager;
            this.configuration = configuration;
        }

        [BindProperty]
        public Order Order { get; set; }

        public async Task<IActionResult> OnGet(int orderId)
        {
            var user = await userManager.Users
               .Include(u => u.Orders)
               .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            Order order = null;

            if (await userManager.IsInRoleAsync(user, Roles.Client.ToString()))
            {
                order = user.Orders.FirstOrDefault(o => o.Id == orderId);
            }
            else if (await userManager.IsInRoleAsync(user, Roles.Administrator.ToString()))
            {
                order = context.Orders.Find(orderId);
            }

            if (order is null)
            {
                return RedirectToPage("/Photos/Orders/List");
            }

            Order = await context.Orders.AsNoTracking()
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Photo)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Size)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.FileFormat)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            foreach (var orderItem in Order.OrderItems.ToList())
            {
                orderItem.RelativePath = FileService.GetUserImageContentPath(configuration, orderItem.FileName);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostDownloadAsync(int? itemId)
        {
            string userName = User.Identity?.Name;

            if (itemId is null
                || userName is null
                || !ModelState.IsValid
                || !User.IsInRole(Roles.Client.ToString()))
            {
                return RedirectToPage("/Error");
            }

            var user = await userManager.FindByNameAsync(userName);

            var userEntry = context.Users
                .Include(u => u.Orders)
                    .ThenInclude(o => o.OrderItems)
                        .ThenInclude(pi => pi.FileFormat)
                .Include(u => u.Orders)
                    .ThenInclude(o => o.OrderItems)
                        .ThenInclude(pi => pi.Photo)
                .FirstOrDefault(u => u.Id == user.Id);

            var userOrder = userEntry.Orders.FirstOrDefault(o => o.Id == Order.Id);

            var userOrderItem = userOrder?.OrderItems.FirstOrDefault(pi => pi.Id == itemId);

            if (userOrderItem is not null)
            {
                string path = FileService.GetUserImageContentPath(configuration, userOrderItem.FileName);
                return File(path, MediaTypeNames.Application.Octet, $"{userOrderItem.Photo.Name}{userOrderItem.FileFormat}");
            }

            return RedirectToPage("/Error");
        }
    }
}
