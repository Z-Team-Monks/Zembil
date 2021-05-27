using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
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
            var product = await _repoProduct.ProductRepo.Get(id);
            if (product == null)
            {
                return NotFound();
            }
            return product;
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
            if(shop.OwnerId != user.Id) return Unauthorized();  //Not your shop 

            var productrepo = _mapper.Map<Product>(product);
            var NewProduct = await _repoProduct.ProductRepo.Add(productrepo);
            return CreatedAtAction(nameof(GetProduct), new { Id = NewProduct.Id }, NewProduct);
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
            if (shop.OwnerId != user.Id) return Unauthorized();  //Not your shop

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
            product.Id = id;
            await _repoProduct.ProductRepo.Update(product);
            return Ok(product);
        }

        [HttpPost("{id}/reviewes")]
        public async Task<ActionResult<Review>> AddReview(int id, ReviewDto review)
        {
            var productExist = await _repoProduct.ProductRepo.Get(id);
            var userExists = await getUserFromHeader(Request.Headers["Authorization"]);

            if (productExist == null) return NotFound("No product found with that id!");
            if (userExists == null) return NotFound("User doesn't Exist");

            review.UserId = userExists.Id;
            review.ProductId = id;

            var reviewForRepo = _mapper.Map<Review>(review);
            await _repoProduct.ReviewRepo.Add(reviewForRepo);
            return Ok(review);
        }        

        private async Task<User> getUserFromHeader(string authHeader)
        {
            int tokenid = _accountServices.Decrypt(authHeader);
            var userExists = await _repoProduct.UserRepo.Get(tokenid);
            return userExists;
        }

    }
}