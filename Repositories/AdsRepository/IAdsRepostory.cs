using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zembil.Models;

namespace Zembil.Repositories
{
    public interface IAdsRepository : IRepositoryBase<Ads>
    {
        Task<List<Ads>> GetAdsWithShops();
        Task<List<Ads>> GetAdsForShop(int shopId);
        Task<List<Ads>> GetActiveAds();
        Task<List<Ads>> GetInActiveAds();
    }

}