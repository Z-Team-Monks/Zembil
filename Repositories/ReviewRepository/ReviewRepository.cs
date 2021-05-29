using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zembil.DbContexts;
using Zembil.Models;

namespace Zembil.Repositories
{
    public class ReviewRepository : RepositoryBase<Review>, IReviewRepository
    {
        public ReviewRepository(ZembilContext context) : base(context)
        {

        }

        public async Task<List<Review>> GetReviewesOfProduct(int productId)
        {
            var reviewes = await _databaseContext.Reviews.Where(r => r.ProductId == productId).ToListAsync();
            return reviewes;
        }
    }
}
