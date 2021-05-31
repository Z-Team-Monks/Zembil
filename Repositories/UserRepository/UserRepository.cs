using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Zembil.DbContexts;
using Zembil.Models;

namespace Zembil.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(ZembilContext context) : base(context)
        {
        }

        public async Task<Object> GetZembilStatus()
        {
            var shopsCount = await _databaseContext.Set<Shop>().CountAsync();
            var usersCount = await _databaseContext.Set<User>().CountAsync();
            var productsPerCategory = await _databaseContext.Set<Product>()
                                .Include(x => x.ProductCategory)
                                .ToListAsync();

            var productsByCategory = productsPerCategory.GroupBy(x => x.ProductCategory)
                                .Select(x => new { Category = x.Key.CategoryName, ProductCount = x.Count() }).ToList();
            var adsCount = await _databaseContext.Set<Ads>().CountAsync();
            var status = new
            {
                shopsCount,
                usersCount,
                productsByCategory,
                adsCount
            };

            return status;
        }
    }
}