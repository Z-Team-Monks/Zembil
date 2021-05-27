using System.Collections.Generic;
using System.Threading.Tasks;
using Zembil.Models;
using Zembil.Utils;

namespace Zembil.Repositories
{
    public interface IProductRepository : IRepositoryBase<Product>
    {
        //Task<Review> AddReview(Review review);
        //IEnumerable<Review> GetReviewes(int productId);
        //void DeleteAReview(int reviewId);
        Task<List<Product>> FilterProducts(QueryParams queryParams);
        Task<List<Product>> SearchProducts(QueryParams queryParams);
    }
}