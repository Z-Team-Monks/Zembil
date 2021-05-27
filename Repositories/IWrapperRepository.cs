
using System.Threading.Tasks;

namespace Zembil.Repositories
{
    public interface IRepositoryWrapper
    {
        IUserRepository UserRepo { get; }
        IProductRepository ProductRepo { get; }
        IShopRepository ShopRepo { get; }
        IWishListRepository WishListRepo { get; }
        IReviewRepository ReviewRepo { get; }
        Task SaveAsync();
    }
}