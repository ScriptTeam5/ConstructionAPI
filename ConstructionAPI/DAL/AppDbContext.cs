
using ConstructionAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ConstructionAPI.DAL
{
    public class AppDbContext : IdentityDbContext<User, Role, int>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductAttribute> ProductAttributes { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Favorite> Favorites { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductImage>()
               .HasOne(pi => pi.Product)
               .WithMany(p => p.Images)
               .HasForeignKey(pi => pi.ProductId)
               .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Favorite>().HasKey(f => new { f.UserId, f.ProductId });

            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.User)
                .WithMany(u => u.Favorites)
                .HasForeignKey(f => f.UserId);

            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.Product)
                .WithMany(p => p.Favorites)
                .HasForeignKey(f => f.ProductId);
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin", NormalizedName = "ADMIN" },
                  new Role { Id = 2, Name = "User", NormalizedName = "USER" }
              );
            var hasher = new PasswordHasher<User>();
            modelBuilder.Entity<User>().HasData(
              new User
              {
                  Id = 1,
                  UserName = "ilkin.admin",
                  PasswordHash = hasher.HashPassword(null, "Admin.1234"),
                  Email = "inovruzov2004@gmail.com",
                  EmailConfirmed = true,
                  NormalizedUserName = "ILKIN.ADMIN",
                  NormalizedEmail = "INOVRUZOV2004@GMAIL.COM",
                  LockoutEnabled = true,
                  SecurityStamp = Guid.NewGuid().ToString()
              }
              );

            modelBuilder.Entity<IdentityUserRole<int>>().HasData(
       new IdentityUserRole<int> { UserId = 1, RoleId = 1 }
   );
            base.OnModelCreating(modelBuilder);
        }
    }
}
