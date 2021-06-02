using System.Collections.Generic;
using System.Threading.Tasks;
using Zembil.Models;
using Zembil.Utils;

namespace Zembil.Repositories
{
    public interface IShopRepository : IRepositoryBase<Shop>
    {
        public Task FollowShop(ShopFollow shopFollow);
        public Task RetractFollow(int userid, int shopId);
        public bool FollowExists(int userid, int shopId);
        Task<int> GetFollow(int shopId);
        Task<List<Shop>> FilterProducts(QueryFilterParams queryParams);
        Task<List<Shop>> SearchShops(ShopSearchQuery queryParams);
        Task<Shop> GetShopWithLocation(int shopId);
        Task<List<Product>> GetAllProductsOfShop(int shopId);
        Task<List<Shop>> GetShopsByOwner(int ownerId);
        Task<List<ShopFollow>> GetUsersFollowing(int shopId);
        Task<bool> IsuserFollwing(int userid,int shopId);
    }
}