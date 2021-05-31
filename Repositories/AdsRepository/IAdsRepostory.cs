using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zembil.Models;

namespace Zembil.Repositories
{
    public interface IAdsRepository : IRepositoryBase<Ads>
    {
        Task<List<Ads>> GetAdsWithShops();
    }
}