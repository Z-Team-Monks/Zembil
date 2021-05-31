using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Zembil.DbContexts;
using Zembil.Models;

namespace Zembil.Repositories
{
    public class AdsRepository : RepositoryBase<Ads>, IAdsRepository
    {
        public AdsRepository(ZembilContext context) : base(context)
        {
        }

        public async Task<List<Ads>> GetAdsWithShops()
        {
            var fullAdsList = await _databaseContext.Set<Ads>().Include(x => x.AdsShop).ToListAsync();
            return fullAdsList;
        }
        public async Task<List<Ads>> GetAdsForShop(int shopId)
        {
            var fullAdsList = await _databaseContext.Set<Ads>().Include(x => x.AdsShop).ToListAsync();
            fullAdsList = fullAdsList.Where(x => x.ShopId == shopId).ToList();
            return fullAdsList;
        }
        public async Task<List<Ads>> GetActiveAds()
        {
            var fullAdsList = await _databaseContext.Set<Ads>().Include(x => x.AdsShop).ToListAsync();
            fullAdsList = fullAdsList.Where(x => x.IsActive).ToList();
            return fullAdsList;
        }
        public async Task<List<Ads>> GetInActiveAds()
        {
            var fullAdsList = await _databaseContext.Set<Ads>().Include(x => x.AdsShop).ToListAsync();
            fullAdsList = fullAdsList.Where(x => !x.IsActive).ToList();
            return fullAdsList;
        }
    }
}