using Microsoft.EntityFrameworkCore;
using System;
using Zembil.Models;

namespace Zembil.DbContexts
{
    public class ZembilContext : DbContext
    {
        public ZembilContext(DbContextOptions<ZembilContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ShopFollow> ShopFollow { get; set; }
        public DbSet<WishListItem> WishList { get; set; }
        public DbSet<Ads> Ads { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Review>()
               .HasKey(r => new { r.ProductId, r.UserId });        
            
            // seed the database with dummy data
            modelBuilder.Entity<User>().HasData(
                    new User
                    {
                        UserId = 1,
                        Username = "Kidus",
                        Email = "se.kidus.yoseph@gmail.com",
                        Password = "$2a$11$iIJq.LUUPeCxoG9gNKL6uuUbcXTjeQapIUgSB5k4kXx5iKgGiSt4q",
                        Role = "Admin",
                        Phone = "+251972476097"
                    }
                );

            //modelBuilder.Entity<Shop>().HasData(
            //        new Shop
            //        {
            //            ShopId = 1,
            //            BuildingName = "Ayat",
            //            PhoneNumber1 = "+251972476907",
            //            PhoneNumber2 = null,
            //            OwnerId = 1,
            //            CategoryId = 1,
            //            ShopLocationId = 1,
            //            Description = "You can find world class watches in our shop"
            //        }
            //    );

            base.OnModelCreating(modelBuilder);
        }
    }
}