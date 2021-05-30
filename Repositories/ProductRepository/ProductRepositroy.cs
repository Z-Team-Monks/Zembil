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

        public async Task<Product> GetProductWithReviewes(int productId)
        {
            var fullProductList = await _databaseContext.Set<Product>().Include(x => x.ProductReviews).ToListAsync();
            var products = fullProductList.FirstOrDefault(r => r.ProductId == productId);
            Console.WriteLine($"Products: {fullProductList[0].ProductReviews}");
            return products;
        }

        public async Task<List<Product>> FilterProducts(QueryFilterParams queryParams)
        {
            List<Product> products = await FilterModels(queryParams);

            string Sort = queryParams.Sort;

            Console.WriteLine($"sort: {Sort}");

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

        public async Task<List<Product>> SearchProducts(ProductSearchQuery queryParams)
        {
            List<Product> products = await _databaseContext.Set<Product>().ToListAsync();
            int TotalProductsCount = products.Count();
            Console.WriteLine($"total: {TotalProductsCount}");
            string Name = queryParams.Name;
            int Category = queryParams.Category;
            string Brand = queryParams.Brand;


            if (!string.IsNullOrEmpty(Name))
            {
                products = products.Where(p => p.ProductName.ToLower().StartsWith(Name.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(Brand))
            {
                products = products.Where(p => p.Brand.ToLower().Contains(Brand.ToLower())).ToList();
            }

            if (Category != 0)
            {
                products = products.Where(p => p.CategoryId == Category).ToList();
            }

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
    }
}