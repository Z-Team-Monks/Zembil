using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Zembil.DbContexts;
using Zembil.Models;
using Zembil.Utils;

namespace Zembil.Repositories
{
    public class ShopRepository : RepositoryBase<Shop>, IShopRepository
    {
        public ShopRepository(ZembilContext context) : base(context)
        {

        }
        public async Task<Shop> GetShopWithLocation(int shopId)
        {
            var shop = await _databaseContext.Set<Shop>().Include(s => s.ShopLocation).FirstAsync(x => x.ShopId == shopId);
            return shop;

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
        public async Task<List<Shop>> FilterProducts(QueryParams queryParams)
        {
            List<Shop> shops = await _databaseContext.Set<Shop>().ToListAsync();
            int TotalshopsCount = shops.Count();
            Console.WriteLine($"total: {TotalshopsCount}");

            int Limit = queryParams.Limit;
            string Sort = queryParams.Sort;
            int Pagination = queryParams.Pagination;
            Console.WriteLine($"Limit: {Limit} sort: {Sort} pagination: {Pagination}");
            if (Limit <= 0)
            {
                Limit = 10;
            }
            if (Pagination <= 0)
            {
                Pagination = 1;
            }

            var startIndex = (Pagination - 1) * Limit;
            var count = TotalshopsCount - startIndex;
            var endIndex = Limit < count ? Limit : count;

            if (Limit < TotalshopsCount && startIndex < TotalshopsCount)
            {
                shops = shops.GetRange(startIndex, endIndex);

            }
            else if (Limit < TotalshopsCount)
            {
                shops = shops.GetRange(0, Limit);
            }
            if (!string.IsNullOrEmpty(Sort))
            {
                if (Sort.ToLower().Equals("name"))
                {
                    shops.Sort(delegate (Shop sh1, Shop sh2) { return sh1.BuildingName.CompareTo(sh2.BuildingName); });
                }
            }

            return shops;
        }

        public async Task<List<Shop>> SearchShops(QueryParams queryParams)
        {
            List<Shop> Shops = await _databaseContext.Set<Shop>().ToListAsync();
            int TotalShopsCount = Shops.Count();
            Console.WriteLine($"total: {TotalShopsCount}");
            string Building = queryParams.BuildingName;

            if (!string.IsNullOrEmpty(Building))
            {
                Shops = Shops.Where(p => p.BuildingName.ToLower().Contains(Building.ToLower())).ToList();
            }

            return Shops;
        }
    }
}