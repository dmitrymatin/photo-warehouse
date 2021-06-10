using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PhotoWarehouse.Domain.Feedbacks;
using PhotoWarehouse.Domain.Orders;
using PhotoWarehouse.Domain.Photos;
using PhotoWarehouse.Domain.Users;

namespace PhotoWarehouse.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Photo> Photos { get; set; }
        public DbSet<PhotoItem> PhotoItems { get; set; }
        public DbSet<PhotoCategory> PhotoCategories { get; set; }
        public DbSet<PhotoSize> PhotoSizes { get; set; }
        public DbSet<FileFormat> FileFormats { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }

        public DbSet<Feedback> Feedbacks { get; set; }
    }
}
