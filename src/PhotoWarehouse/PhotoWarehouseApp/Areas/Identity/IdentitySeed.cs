using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using PhotoWarehouse.Domain.Users;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoWarehouseApp.Areas.Identity
{
    public static class IdentitySeed
    {
        public static async Task SeedRolesAndUsersAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            if (!await roleManager.RoleExistsAsync(Roles.Administrator.ToString()))
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Administrator.ToString()));
            }

            if (!await roleManager.RoleExistsAsync(Roles.Client.ToString()))
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Client.ToString()));
            }

            await SeedAdminAsync(userManager, roleManager, configuration);
        }

        private static async Task SeedAdminAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            var administrator = new ApplicationUser
            {
                UserName = "administrator",
                Email = configuration["AdministratorEmail"],
                DateJoined = DateTimeOffset.UtcNow,
                EmailConfirmed = true
            };

            bool administratorExists = userManager.Users.Any(user => user.UserName == administrator.UserName);

            if (!administratorExists)
            {
                await userManager.CreateAsync(administrator, configuration["AdministratorPassword"]);
                await userManager.AddToRoleAsync(administrator, Roles.Administrator.ToString());
            }
        }
    }
}
