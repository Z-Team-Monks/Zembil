using System.Collections.Generic;
using System.Threading.Tasks;
using Zembil.Models;
using Zembil.Utils;

namespace Zembil.Repositories
{
    public interface IProductRepository : IRepositoryBase<Product>
    {
        Task<Review> AddReview(Review review);
        public IEnumerable<Review> GetReviewes(int productId);
        Task<List<Product>> FilterProducts(QueryParams queryParams);

    }
}