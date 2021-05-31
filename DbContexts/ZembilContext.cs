using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using NetTopologySuite.Geometries;
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
        public DbSet<ShopLocation> ShopLocations { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ShopFollow> ShopFollow { get; set; }
        public DbSet<WishListItem> WishList { get; set; }
        public DbSet<Ads> Ads { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

            modelBuilder.HasPostgresExtension("postgis");
            modelBuilder.Entity<Review>()
              .HasKey(r => new { r.ProductId, r.UserId });
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

            //modelBuilder.Entity<Category>().HasData(
            //        new Category
            //        {
            //            CategoryId = 1,
            //            CategoryName = "Electrnoics"
            //        }
            // );
            // modelBuilder.Entity<ShopLocation>().HasData(
            //             new ShopLocation
            //             {
            //                 LocationName = "Piassa",
            //                 LocationDescription = "near amazing place",
            //                 GeoLoacation = geometryFactory.CreatePoint(new Coordinate(35.929673, -78.948237))

            //             },
            //             new ShopLocation
            //             {
            //                 LocationName = "Piassa",
            //                 LocationDescription = "near amazing place",
            //                 GeoLoacation = geometryFactory.CreatePoint(new Coordinate(38.889510, -77.032000))
            //             }
            //         );

            base.OnModelCreating(modelBuilder);
        }
    }
}