using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Zembil.Models;
using Zembil.Repositories;
using Zembil.Services;
using Zembil.Utils;
using Zembil.Views;

namespace Zembil.Controllers
{
    [Authorize]
    [Route("api/v1/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private IRepositoryWrapper _repoProduct;
        private readonly IAccountService _accountServices;
        private readonly IMapper _mapper;

        public ProductsController(IRepositoryWrapper repoWrapper, IAccountService accountServices, IMapper mapper)
        {
            _repoProduct = repoWrapper;
            _accountServices = accountServices;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _repoProduct.ProductRepo.GetProductWithReviewes(id);
            // List<Review> reviews = await _repoProduct.ReviewRepo.GetReviewesOfProduct(id);
            if (product == null)
            {
                return NotFound();
            }
            // List<Review> productReview = await _repoProduct.ReviewRepo.GetReviewesOfProduct(product.ProductId);
            // int totalRating = productReview.Count();
            // // int c = productReview.Select(x => x.Rating).Sum();
            // int ratingCount = productReview.Sum(item => item.Rating);
            // product.ProductReviews = reviews;
            return product;
            // return Ok(new { Rating = reviews, Product = product });
        }

        [AllowAnonymous]
        [HttpGet]
        [HttpHead]
        public async Task<IEnumerable<Product>> GetProducts([FromQuery] QueryParams queryParams)
        {
            List<Product> products;
            Console.WriteLine($"run");
            if (queryParams == null)
            {
                products = await _repoProduct.ProductRepo.GetAll();
                return products;
            }
            products = await _repoProduct.ProductRepo.FilterProducts(queryParams);
            return products;
        }


        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] ProductCreateDto product)
        {

            var shopExists = await _repoProduct.ShopRepo.Get(product.ShopId);
            if (shopExists == null) return NotFound("Shop doesn't exist");

            var user = await getUserFromHeader(Request.Headers["Authorization"]);
            if (user == null) return Unauthorized();

            var shop = await _repoProduct.ShopRepo.Get(product.ShopId);
            if (shop.OwnerId != user.UserId) return Unauthorized();  //Not your shop 

            var productrepo = _mapper.Map<Product>(product);
            var NewProduct = await _repoProduct.ProductRepo.Add(productrepo);
            return CreatedAtAction(nameof(GetProduct), new { Id = NewProduct.ProductId }, NewProduct);
        }


        [HttpPatch("{id:int}")]
        public async Task<ActionResult<Product>> UpdateProduct(int id, [FromBody] JsonPatchDocument<Product> product)
        {
            var user = await getUserFromHeader(Request.Headers["Authorization"]);
            if (user == null) return Unauthorized();

            var productExist = await _repoProduct.ProductRepo.Get(id);

            if (productExist == null)
            {
                return NotFound("No product found with that id!");
            }

            var shopExists = await _repoProduct.ShopRepo.Get(productExist.ShopId);
            if (shopExists == null) return NotFound("Shop doesn't exist");

            var shop = await _repoProduct.ShopRepo.Get(productExist.ShopId);
            if (shop.OwnerId != user.UserId) return Unauthorized();  //Not your shop

            product.ApplyTo(productExist, ModelState);
            await _repoProduct.ProductRepo.Update(productExist);
            return Ok(productExist);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Product>> UpdateFullProduct(int id, [FromBody] Product product)
        {

            var productExist = await _repoProduct.ProductRepo.Get(id);

            if (productExist == null)
            {
                return NotFound("No product found with that id!");
            }
            product.ProductId = id;
            await _repoProduct.ProductRepo.Update(product);
            return Ok(product);
        }

        [HttpPost("{id}/reviewes")]
        public async Task<ActionResult<Review>> AddReview(int id, Review review)
        {

            //prevent same user from adding multiple review to one product

            var productExist = await _repoProduct.ProductRepo.Get(id);
            var userExists = await getUserFromHeader(Request.Headers["Authorization"]);

            if (productExist == null) return NotFound("No product found with that id!");
            if (userExists == null) return NotFound("User doesn't Exist");

            review.UserId = userExists.UserId;
            review.ProductId = id;


            await _repoProduct.ReviewRepo.Add(review);
            var reviewForRepo = _mapper.Map<Review>(review);
            return Ok(reviewForRepo);
        }

        [HttpGet("trending")]
        public async Task<IEnumerable<Product>> TrendingProducts([FromQuery] TrendingQuery queryParams)
        {
            if (queryParams == null)
            {
                queryParams = new TrendingQuery();
                queryParams.Latest = 1;
            }
            var trendingProducts = await _repoProduct.ProductRepo.GetTrendingProducts(queryParams);
            return trendingProducts;
        }

        private async Task<User> getUserFromHeader(string authHeader)
        {
            int tokenid = _accountServices.Decrypt(authHeader);
            var userExists = await _repoProduct.UserRepo.Get(tokenid);
            return userExists;
        }

    }
}