using Microsoft.AspNetCore.Identity;
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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>(entity =>
                entity.Property(p => p.Id).HasMaxLength(85));
            builder.Entity<IdentityRole>(entity =>
                entity.Property(p => p.Id).HasMaxLength(85));

            builder.Entity<IdentityUserClaim<string>>(entity =>
                entity.Property(p => p.Id).HasMaxLength(85));
            builder.Entity<IdentityRoleClaim<string>>(entity =>
                entity.Property(p => p.Id).HasMaxLength(85));

            builder.Entity<IdentityUserLogin<string>>(entity =>
                entity.Property(p => p.LoginProvider).HasMaxLength(85));
            builder.Entity<IdentityUserLogin<string>>(entity =>
                entity.Property(p => p.ProviderKey).HasMaxLength(85));

            builder.Entity<IdentityUserToken<string>>(entity =>
                entity.Property(p => p.LoginProvider).HasMaxLength(85));
            builder.Entity<IdentityUserToken<string>>(entity =>
                entity.Property(p => p.Name).HasMaxLength(85));

            base.OnModelCreating(builder);
        }
    }
}
