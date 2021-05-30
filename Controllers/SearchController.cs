using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zembil.Models;
using Zembil.Repositories;
using Zembil.Services;
using Zembil.Utils;
using Zembil.Views;

namespace Zembil.Controllers
{
    [Authorize]
    [Route("api/v1/search")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private IRepositoryWrapper _repoProduct { get; set; }
        private readonly IAccountService _accountServices;
        private readonly IMapper _mapper;

        public SearchController(IRepositoryWrapper repoWrapper, IAccountService accountServices, IMapper mapper)
        {
            _repoProduct = repoWrapper;
            _accountServices = accountServices;
            _mapper = mapper;
        }


        [AllowAnonymous]
        [Route("products")]
        [HttpGet]
        public async Task<IEnumerable<Product>> GetProducts([FromQuery] ProductSearchQuery queryParams)
        {
            List<Product> products;
            Console.WriteLine($"run");
            if (queryParams == null)
            {
                products = await _repoProduct.ProductRepo.GetAll();
                return products;
            }
            products = await _repoProduct.ProductRepo.SearchProducts(queryParams);
            return products;
        }

        [AllowAnonymous]
        [Route("shops")]
        [HttpGet]
        public async Task<IEnumerable<Shop>> GetShops([FromQuery] ShopSearchQuery queryParams)
        {
            List<Shop> shops;
            Console.WriteLine($"run");
            if (queryParams == null)
            {
                shops = await _repoProduct.ShopRepo.GetAll();
                return shops;
            }
            shops = await _repoProduct.ShopRepo.SearchShops(queryParams);
            return shops;
        }
    }
}