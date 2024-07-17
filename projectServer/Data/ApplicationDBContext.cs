using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using projectServer.Models.Account;
using projectServer.Models.Image;

namespace projectServer.Data
{
    public class ApplicationDBContext : IdentityDbContext<UserModel>
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions)
            : base(dbContextOptions)
        {
                
        }

        public DbSet<SampleImageModel> ImagesUpload { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            /*
            builder.Entity<Protfolio>(x => x.HasKey(p => new { p.AppUserId, p.StockId }));

            builder.Entity<Protfolio>()
                .HasOne(u => u.AppUser)
                .WithMany(u => u.protfolios)
                .HasForeignKey(p => p.AppUserId);

            builder.Entity<Protfolio>()
                .HasOne(u => u.Stock)
                .WithMany(u => u.protfolios)
                .HasForeignKey(p => p.StockId);
            */

            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "USER"
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}

