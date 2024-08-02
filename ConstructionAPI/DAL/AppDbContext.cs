
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
        public DbSet<Favorite> Favorites { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Category>()
               .HasMany(c => c.SubCategories)
               .WithOne(c => c.ParentCategory)
               .HasForeignKey(c => c.ParentId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProductImage>()
               .HasOne(pi => pi.Product)
               .WithMany(p => p.Images)
               .HasForeignKey(pi => pi.ProductId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductAttribute>()
            .HasOne(pa => pa.Product)
            .WithMany(p => p.Attributes)
            .HasForeignKey(pa => pa.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
           .HasOne(u => u.Shop)
           .WithOne(s => s.User)
           .HasForeignKey<User>(u => u.ShopId)
           .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Products)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
           .HasOne(p => p.Shop)
           .WithMany(s => s.Products)
           .HasForeignKey(p => p.ShopId)
           .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Favorite>().HasKey(f => new { f.UserId, f.ProductId });

            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.User)
                .WithMany(u => u.Favorites)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.Product)
                .WithMany(p => p.Favorites)
                .HasForeignKey(f => f.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Like>().HasKey(l => new { l.UserId, l.ProductId });

            modelBuilder.Entity<Like>()
                .HasOne(l => l.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Like>()
                .HasOne(l => l.Product)
                .WithMany(p => p.Likes)
                .HasForeignKey(l => l.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

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
                  PhoneNumber = "1234567890",
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
