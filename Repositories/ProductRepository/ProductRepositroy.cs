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

        public async Task<Product> GetProductWithReviewes(int productId)
        {
            var fullProductList = await _databaseContext.Set<Product>().Include(x => x.ProductReviews).ToListAsync();
            var products = fullProductList.FirstOrDefault(r => r.ProductId == productId);
            Console.WriteLine($"Products: {fullProductList[0].ProductReviews}");
            return products;
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
                products = products.Where(x => x.ProductName.Equals(Name)).ToList();
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
                    products.Sort(delegate (Product p1, Product p2) { return p1.ProductName.CompareTo(p2.ProductName); });
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
                products = products.Where(p => p.ProductName.ToLower().Contains(Name.ToLower())).ToList();
            }

            // if (!string.IsNullOrEmpty(Category))
            // {
            //     products = products.Where(p => p.Category.ToLower().Equals(Category.ToLower())).ToList();
            // }

            return products;
        }

        public async Task<List<Product>> GetTrendingProducts(TrendingQuery queryParams)
        {

            int Latest = queryParams.Latest;
            int Popular = queryParams.Popular;
            int MaxAmount = 20;
            List<Product> products = await _databaseContext.Set<Product>().Include(x => x.ProductReviews).ToListAsync();


            if (Latest != 0)
            {
                products = products.OrderByDescending(p => p.DateInserted).ToList();
            }
            else if (Popular != 0)
            {
                var TopProducts = await _databaseContext.Set<Review>().GroupBy(x => x.ProductId)
                            .Select(x => new { ProductId = x.Key, AvgRatingSum = x.Average(a => a.Rating) })
                            .OrderByDescending(x => x.AvgRatingSum)
                            .Select(x => x.ProductId)
                            .Take(MaxAmount)
                            .ToListAsync();
                foreach (var item in TopProducts)
                {
                    Console.WriteLine(item);
                }
                products = TopProducts.Select(x => products.Find(y => y.ProductId == x)).ToList();
            }

            int itemCount = products.Count();
            if (itemCount <= MaxAmount)
            {
                return products;
            }
            products = products.GetRange(0, MaxAmount);
            return products;
        }

        public async Task<List<Product>> GetCategorizedProducts(TrendingQuery queryParams)
        {

            int Latest = queryParams.Latest;
            int Popular = queryParams.Popular;
            int MaxAmount = 20;
            List<Product> products = await _databaseContext.Set<Product>().Include(x => x.ProductReviews).ToListAsync();


            if (Latest != 0)
            {
                products = products.OrderByDescending(p => p.DateInserted).ToList();
            }
            else if (Popular != 0)
            {
                var TopProducts = await _databaseContext.Set<Review>().GroupBy(x => x.ProductId)
                            .Select(x => new { ProductId = x.Key, AvgRatingSum = x.Average(a => a.Rating) })
                            .OrderByDescending(x => x.AvgRatingSum)
                            .Select(x => x.ProductId)
                            .Take(MaxAmount)
                            .ToListAsync();
                products = TopProducts.Select(x => products.Find(y => y.ProductId == x)).ToList();
            }

            int itemCount = products.Count();
            if (itemCount <= MaxAmount)
            {
                return products;
            }
            products = products.GetRange(0, MaxAmount);
            return products;
        }
    }
}