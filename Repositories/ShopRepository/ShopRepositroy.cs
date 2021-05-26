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

        public async Task LikeShop(ShopLike shoplike)
        {
            _databaseContext.Set<ShopLike>().Add(shoplike);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task RetractLike(int userid, int shopId)
        {
            var likeToRetract = _databaseContext.ShopLikes.SingleOrDefault(s => s.ShopId == shopId && s.UserId == userid);
            if (likeToRetract != null)
            {
                _databaseContext.ShopLikes.Remove(likeToRetract);
                await _databaseContext.SaveChangesAsync();
            }
        }

        public bool LikeExists(int userid, int shopId)
        {
            var likeToRetract = _databaseContext.ShopLikes.SingleOrDefault(s => s.ShopId == shopId && s.UserId == userid);
            return likeToRetract == null ? false : true;
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
            if (Sort != null && Sort.Length > 0)
            {
                if (Sort.ToLower().Equals("name"))
                {
                    shops.Sort(delegate (Shop sh1, Shop sh2) { return sh1.BuildingName.CompareTo(sh2.BuildingName); });
                }
            }

            return shops;
        }
    }
}