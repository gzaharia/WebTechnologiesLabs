using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using xTremeShop.Models;

namespace xTremeShop.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<MobileApp> MobileApps { get; set; }
        public DbSet<UserLibrary> UserLibraries { get; set; }
        public DbSet<LibraryApps> LibraryApps { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<LibraryApps>().HasKey(t => new { t.LibaryId, t.AppId });

            builder.Entity<LibraryApps>()
                .HasOne(f => f.App)
                .WithMany(f => f.LibraryApps)
                .HasForeignKey(f => f.AppId);

            builder.Entity<LibraryApps>()
                .HasOne(f => f.Library)
                .WithMany(f => f.LibraryApps)
                .HasForeignKey(f => f.LibaryId);

            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
