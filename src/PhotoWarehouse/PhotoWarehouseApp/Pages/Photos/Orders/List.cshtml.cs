using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PhotoWarehouse.Data;
using PhotoWarehouse.Domain.Orders;
using PhotoWarehouse.Domain.Users;
using PhotoWarehouseApp.Areas.Identity;

namespace PhotoWarehouseApp.Pages.Photos.Orders
{
    public class ListModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext context;

        public ListModel(UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            this.userManager = userManager;
            this.context = context;
        }

        public IEnumerable<Order> Orders { get; set; }

        public async Task<IActionResult> OnGet()
        {
            string userName = User.Identity?.Name;
            if (userName is null)
            {
                return RedirectToPage("/Error");
            }

            var user = await userManager.FindByNameAsync(userName);


            if (await userManager.IsInRoleAsync(user, Roles.Administrator.ToString()))
            {
                Orders = await context.Orders
                    .Include(o => o.OrderItems)
                    .Include(o => o.Customer)
                    .ToListAsync();

                return Page();
            }

            Orders = await context.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.Customer.Id == user.Id)
                .ToListAsync();

            return Page();
        }
    }
}
