using System.Collections.Generic;
using System.Threading.Tasks;
using Zembil.Models;
using Zembil.Utils;

namespace Zembil.Repositories
{
    public interface IProductRepository : IRepositoryBase<Product>
    {
        Task<List<Product>> FilterProducts(QueryFilterParams queryParams);
        Task<List<Product>> SearchProducts(ProductSearchQuery queryParams);
        Task<List<Product>> GetTrendingProducts(TrendingQuery queryParams);
        Task<Product> GetProductWithReviewes(int productId);
    }
}