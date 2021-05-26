using System.Collections.Generic;
using System.Threading.Tasks;
using Zembil.Models;
using Zembil.Utils;

namespace Zembil.Repositories
{
    public interface IShopRepository : IRepositoryBase<Shop>
    {
        public Task LikeShop(ShopLike shoplike);
        public Task RetractLike(int userid, int shopId);
        public bool LikeExists(int userid, int shopId);
        Task<List<Shop>> FilterProducts(QueryParams queryParams);
    }
}