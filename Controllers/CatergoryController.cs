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
    [Route("api/v1/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private IRepositoryWrapper _repoCategory { get; set; }
        private readonly IAccountService _accountServices;
        private readonly IMapper _mapper;

        public CategoryController(IRepositoryWrapper repoWrapper, IAccountService accountServices, IMapper mapper)
        {
            _repoCategory = repoWrapper;
            _accountServices = accountServices;
            _mapper = mapper;
        }


        [AllowAnonymous]
        [HttpGet]
        public async Task<IEnumerable<Category>> GetAllCategory()
        {
            List<Category> Categories = await _repoCategory.CategoryRepo.GetAll();
            return Categories;
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<Category> AddCategory(Category Category)
        {

            //TODO: add role auth here     only admins are allowed

            var category = await _repoCategory.CategoryRepo.Add(Category);
            return category;
        }
    }
}