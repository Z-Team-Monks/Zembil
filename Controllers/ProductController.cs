using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ProductsController(IRepositoryWrapper repoWrapper, IAccountService accountServices, IMapper mapper, IWebHostEnvironment hostingEnvironment)
        {
            _repoProduct = repoWrapper;
            _accountServices = accountServices;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var product = await _repoProduct.ProductRepo.GetProductWithReviewes(id);
            if (product == null)
            {
                return NotFound();
            }
            var reviewesFromRepo = await _repoProduct.ReviewRepo.GetReviewesOfProduct(id);
            var reviewesToReturn = _mapper.Map<IEnumerable<ReviewToReturnDto>>(reviewesFromRepo);

            foreach (var review in reviewesToReturn)
            {
                var userFromRepo = await _repoProduct.UserRepo.Get(review.UserId);
                review.UserName = userFromRepo.Username;
            }

            var productDto = _mapper.Map<ProductDto>(product);
            productDto.ProductReviews = reviewesToReturn;
            return productDto;            
        }

        [AllowAnonymous]
        [HttpGet]
        [HttpHead]
        public async Task<IEnumerable<ProductGetBatchDto>> GetProducts([FromQuery] QueryFilterParams queryParams)
        {
            List<Product> products;
            if (queryParams == null)
            {
                products = await _repoProduct.ProductRepo.GetAll();
            }
            else
            {
                products = await _repoProduct.ProductRepo.FilterProducts(queryParams);
            }
            return _mapper.Map<IEnumerable<ProductGetBatchDto>>(products);
        }


        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] ProductCreateDto product, [FromForm] IFormFile file = null)
        {

            var shopExists = await _repoProduct.ShopRepo.Get(product.ShopId);
            if (shopExists == null) return NotFound("Shop doesn't exist");

            var user = await getUserFromHeader(Request.Headers["Authorization"]);
            if (user == null) return Unauthorized();

            var shop = await _repoProduct.ShopRepo.Get(product.ShopId);
            if (shop.OwnerId != user.UserId) return Unauthorized();  //Not your shop 

            var isProductValid = await ValidateProduct(product);
            if (!isProductValid)
            {
                return BadRequest("Invalid Category");
            }

            // notification for followers
            var following = await _repoProduct.ShopRepo.GetUsersFollowing(product.ShopId);
            foreach (var follower in following)
            {
                var newNotification = new Notification
                {
                    UserId = user.UserId,
                    NotificationMessage = $"{shopExists.ShopName} added new product {product.ProductName}",
                    Seen = false,
                };
                await _repoProduct.NotificationRepo.Add(newNotification);
            }


            // upload image if exists
            if (file != null)
            {
                var size = file.Length;
                Console.WriteLine($"File upload size: {size}");
                var filePath = Path.Combine(@Directory.GetCurrentDirectory() + "/Uploads/", file.FileName);
                if (file.Length > 0)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                        shop.CoverImage = filePath;
                    }
                }
            }

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
            if (!TryValidateModel(productExist))
            {
                return ValidationProblem(ModelState);
            }
            await _repoProduct.ProductRepo.Update(productExist);
            return Ok(productExist);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ProductDto>> UpdateFullProduct(int id, [FromBody] ProductUpdateDto productUpdateDto)
        {

            var productExist = await _repoProduct.ProductRepo.Get(id);

            if (productExist == null)
            {
                return NotFound("No product found with that id!");
            }

            _mapper.Map(productUpdateDto, productExist);
            await _repoProduct.ProductRepo.Update(productExist);
            return _mapper.Map<ProductDto>(productExist);
        }

        [AllowAnonymous]
        [HttpGet("trending")]
        public async Task<IEnumerable<ProductGetBatchDto>> TrendingProducts([FromQuery] TrendingQuery queryParams)
        {
            if (queryParams == null)
            {
                queryParams = new TrendingQuery();
                queryParams.Latest = 1;
            }
            var trendingProducts = await _repoProduct.ProductRepo.GetTrendingProducts(queryParams);
            return _mapper.Map<IEnumerable<ProductGetBatchDto>>(trendingProducts);
        }

        //will be moved later
        private async Task<bool> ValidateProduct(ProductCreateDto newProduct)
        {
            var categories = await _repoProduct.CategoryRepo.GetAll();
            if (!categories.Any(c => c.CategoryId == newProduct.CategoryId))
            {
                return false;
            }
            return true;
        }

        private async Task<User> getUserFromHeader(string authHeader)
        {
            int tokenid = _accountServices.Decrypt(authHeader);
            var userExists = await _repoProduct.UserRepo.Get(tokenid);
            return userExists;
        }

    }
}