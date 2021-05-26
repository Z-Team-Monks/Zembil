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
        private IRepositoryWrapper _repoProduct { get; set; }
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
            if (shopExists == null)
            {
                return NotFound("Shop doesn't exist");
            }

            var productrepo = _mapper.Map<Product>(product);
            var NewProduct = await _repoProduct.ProductRepo.Add(productrepo);
            return CreatedAtAction(nameof(GetProduct), new { Id = NewProduct.Id }, NewProduct);
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<Product>> UpdateProduct(int id, [FromBody] JsonPatchDocument<Product> product)
        {

            var productExist = await _repoProduct.ProductRepo.Get(id);

            if (productExist == null)
            {
                return NotFound("No product found with that id!");
            }
            else
            {
                product.ApplyTo(productExist, ModelState);
            }
            Console.WriteLine("Price:" + productExist.Price);
            await _repoProduct.ProductRepo.Update(productExist);
            return Ok(productExist);
        }

        [HttpPost("{id}/Reviews")]
        public async Task<ActionResult<Review>> AddReview(int id, ReviewDto review)
        {
            var productExist = await _repoProduct.ProductRepo.Get(id);
            string authHeader = Request.Headers["Authorization"];
            int tokenid = _accountServices.Decrypt(authHeader);
            var userExists = await _repoProduct.UserRepo.Get(tokenid);

            if (productExist == null) return NotFound("No product found with that id!");
            if (userExists == null) return NotFound("User doesn't Exist");

            review.UserId = tokenid;
            review.ProductId = id;

            var reviewForRepo = _mapper.Map<Review>(review);
            await _repoProduct.ProductRepo.AddReview(reviewForRepo);
            return Ok(review);
        }

        [AllowAnonymous]
        [HttpGet("{id}/Reviews")]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviewesOfAProduct(int id)
        {
            var productExist = await _repoProduct.ProductRepo.Get(id);
            //string authHeader = Request.Headers["Authorization"];
            //int tokenid = _accountServices.Decrypt(authHeader);
            //var userExists = await _repoProduct.UserRepo.Get(tokenid);

            if (productExist == null) return NotFound("No product found with that id!");
            //if (userExists == null) return NotFound("User doesn't Exist");

            var reviewesFromRepo = _repoProduct.ProductRepo.GetReviewes(id);

            var reviewesToReturn = _mapper.Map<IEnumerable<ReviewToReturnDto>>(reviewesFromRepo);

            foreach (var review in reviewesToReturn)
            {
                var userFromRepo = await _repoProduct.UserRepo.Get(review.UserId);
                review.UserName = userFromRepo.Username;
            }

            return Ok(reviewesToReturn);
        }
    }
}