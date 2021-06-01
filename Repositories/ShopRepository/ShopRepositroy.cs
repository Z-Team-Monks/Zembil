using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using Zembil.DbContexts;
using Zembil.Models;
using Zembil.Utils;
using Zembil.Views;

namespace Zembil.Repositories
{
    public class ShopRepository : RepositoryBase<Shop>, IShopRepository
    {
        public ShopRepository(ZembilContext context) : base(context)
        {

        }
        public async Task<Shop> GetShopWithLocation(int shopId)
        {
            var shop = await _databaseContext.Set<Shop>().FirstAsync(x => x.ShopId == shopId);
            var location = await _databaseContext.Set<ShopLocation>().FirstAsync(x => x.LocationId == shop.ShopLocationId);
            var locationDto = new LocationDto() { LocationId = location.LocationId, LocationName = location.LocationName, Longitude = location.GeoLoacation.Coordinate.Y, Latitude = location.GeoLoacation.Coordinate.X };
            shop.ShopLocation = locationDto;
            Console.WriteLine(locationDto.Latitude);
            return shop;

        }
        public async Task<List<ShopFollow>> GetUsersFollowing(int shopId)
        {
            var following = await _databaseContext.Set<ShopFollow>().Where(s => s.ShopId == shopId).ToListAsync();
            return following;
        }

        public async Task<List<Product>> GetAllProductsOfShop(int shopId)
        {
            var shopProducts = await _databaseContext.Set<Product>().Where(x => x.ShopId == shopId).ToListAsync();
            return shopProducts;

        }
        public async Task<List<Shop>> GetShopsByOwner(int ownerId)
        {
            var shop = await _databaseContext.Set<Shop>().Where(x => x.OwnerId == ownerId).ToListAsync();
            return shop;

        }
        public async Task<int> GetFollow(int shopId)
        {
            var follow = await _databaseContext.Set<ShopFollow>().ToListAsync();
            follow = follow.Where(l => l.ShopId == shopId).ToList();
            return follow.Count();

        }
        public async Task FollowShop(ShopFollow shopFollow)
        {
            _databaseContext.Set<ShopFollow>().Add(shopFollow);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task RetractFollow(int userid, int shopId)
        {
            var followToRetract = _databaseContext.ShopFollow.SingleOrDefault(s => s.ShopId == shopId && s.UserId == userid);
            if (followToRetract != null)
            {
                _databaseContext.ShopFollow.Remove(followToRetract);
                await _databaseContext.SaveChangesAsync();
            }
        }

        public bool FollowExists(int userid, int shopId)
        {
            var followToRetract = _databaseContext.ShopFollow.SingleOrDefault(s => s.ShopId == shopId && s.UserId == userid);
            return followToRetract == null ? false : true;
        }
        public async Task<List<Shop>> FilterProducts(QueryFilterParams queryParams)
        {
            List<Shop> shops = await FilterModels(queryParams);

            string Sort = queryParams.Sort;

            Console.WriteLine($"sort: {Sort}");

            if (!string.IsNullOrEmpty(Sort))
            {
                if (Sort.ToLower().Equals("name"))
                {
                    shops.Sort(delegate (Shop sh1, Shop sh2) { return sh1.BuildingName.CompareTo(sh2.BuildingName); });
                }
            }

            return shops;
        }

        public async Task<List<Shop>> SearchShops(ShopSearchQuery queryParams)
        {
            List<Shop> Shops = await _databaseContext.Set<Shop>().ToListAsync();
            int TotalShopsCount = Shops.Count();
            Console.WriteLine($"total: {TotalShopsCount}");

            string Building = queryParams.Building;
            string Name = queryParams.Name;
            int Category = queryParams.Category;
            NearByLocation shopLocation = queryParams.NearBy;

            if (!string.IsNullOrEmpty(Name))
            {
                Shops = Shops.Where(p => p.ShopName.ToLower().StartsWith(Name.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(Building))
            {
                Shops = Shops.Where(p => p.BuildingName.ToLower().Contains(Building.ToLower())).ToList();
            }
            if (Category != 0)
            {
                Shops = Shops.Where(x => x.CategoryId == Category).ToList();
            }
            if (shopLocation != null)
            {
                var locationMaker = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
                var loc = locationMaker.CreatePoint(new Coordinate(shopLocation.Latitude, shopLocation.Longitude));

                var nearLocat = await _databaseContext.Set<ShopLocation>()
                                                       .Select(t => new { Location = t, Distance = t.GeoLoacation.Distance(loc) })
                                                       .Where(x => x.Distance <= shopLocation.Radius)
                                                       .OrderBy(x => x.Distance).Take(9).ToListAsync();

                var nearLocations = await _databaseContext.Set<ShopLocation>()
                                        .Select(t => new { Location = t, Distance = t.GeoLoacation.Distance(loc) })
                                        .Where(x => x.Distance <= shopLocation.Radius)
                                        .OrderBy(x => x.Distance).Take(9).Select(x => x.Location).ToListAsync();

                foreach (var item in nearLocat)
                {
                    Console.WriteLine("distances:{0} => {1}", item.Location.LocationName, item.Distance);
                }

                Shops = nearLocations.Select(x => Shops.Find(y => y.ShopLocationId == x.LocationId)).ToList();
                foreach (var item in Shops)
                {
                    Console.WriteLine("distances: {0}", item.ShopName);
                }
            }

            return Shops;
        }
    }
}