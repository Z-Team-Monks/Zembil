using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Zembil.DbContexts;
using Zembil.Models;
using Zembil.Utils;

namespace Zembil.Repositories
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(ZembilContext context) : base(context)
        {

        }

        //public async Task<Review> AddReview(Review review)
        //{
        //    _databaseContext.Set<Review>().Add(review);
        //    await _databaseContext.SaveChangesAsync();
        //    return review;
        //}

        //public async Task<Review> DeleteReview(Review review)
        //{
        //    _databaseContext.Set<Review>().Remove
        //}

        public IEnumerable<Review> GetReviewes(int productId)
        {
            var reviewes = _databaseContext.Reviewes.Where(r => r.ProductId == productId);
            return reviewes;
        }

        public async Task<List<Product>> FilterProducts(QueryParams queryParams)
        {
            List<Product> products = await _databaseContext.Set<Product>().ToListAsync();
            int TotalProductsCount = products.Count();
            Console.WriteLine($"total: {TotalProductsCount}");
            string Name = queryParams.Name;
            int Limit = queryParams.Limit;
            string Sort = queryParams.Sort;
            int Pagination = queryParams.Pagination;
            Console.WriteLine($"name: {Name} Limit: {Limit} sort: {Sort} pagination: {Pagination}");
            if (Limit <= 0)
            {
                Limit = 10;
            }
            if (Pagination <= 0)
            {
                Pagination = 1;
            }

            if (!string.IsNullOrEmpty(Name))
            {
                products = products.Where(x => x.Name.Equals(Name)).ToList();
            }
            var startIndex = (Pagination - 1) * Limit;
            var count = TotalProductsCount - startIndex;
            var endIndex = Limit < count ? Limit : count;

            if (Limit < TotalProductsCount && startIndex < TotalProductsCount)
            {
                products = products.GetRange(startIndex, endIndex);

            }
            else if (Limit < TotalProductsCount)
            {
                products = products.GetRange(0, Limit);
            }
            if (!string.IsNullOrEmpty(Sort))
            {
                if (Sort.ToLower().Equals("price"))
                {
                    products.Sort(delegate (Product p1, Product p2) { return p1.Price.CompareTo(p2.Price); });
                }
                else if (Sort.ToLower().Equals("name"))
                {
                    products.Sort(delegate (Product p1, Product p2) { return p1.Name.CompareTo(p2.Name); });
                }
            }

            return products;
        }

        public async Task<List<Product>> SearchProducts(QueryParams queryParams)
        {
            List<Product> products = await _databaseContext.Set<Product>().ToListAsync();
            int TotalProductsCount = products.Count();
            Console.WriteLine($"total: {TotalProductsCount}");
            string Name = queryParams.Name;
            string Category = queryParams.Category;

            if (!string.IsNullOrEmpty(Name))
            {
                products = products.Where(p => p.Name.ToLower().Contains(Name.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(Category))
            {
                products = products.Where(p => p.Category.ToLower().Equals(Category.ToLower())).ToList();
            }

            return products;
        }
    }
}