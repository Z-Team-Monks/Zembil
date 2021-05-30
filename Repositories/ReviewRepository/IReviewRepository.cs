using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zembil.Models;

namespace Zembil.Repositories
{
    public interface IReviewRepository : IRepositoryBase<Review>
    {
        Task<List<Review>> GetReviewesOfProduct(int productId);
        Task<Review> GetRevieweById(int reviewId);
        Task<Review> GetRevieweByUserAndProduct(int reviewId, int userId);
    }
}
