using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    }
}
