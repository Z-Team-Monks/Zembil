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

        [HttpPost]
        public async Task<ActionResult<Category>> AddCategory(Category Category)
        {

            //TODO: add role auth here     only admins are allowed
            var user = await getUserFromHeader(Request.Headers["Authorization"]);
            const string admin = "admin";
            if (user == null || user.Role.ToLower() != admin)
            {
                return Unauthorized();
            }
            var category = await _repoCategory.CategoryRepo.Add(Category);
            return category;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Category>> DeleeteCategory(int id)
        {

            //only admins are allowed
            var user = await getUserFromHeader(Request.Headers["Authorization"]);
            const string admin = "admin";
            if (user == null || user.Role.ToLower() != admin)
            {
                return Unauthorized();
            }
            var category = await _repoCategory.CategoryRepo.Delete(id);
            return category;
        }

        private async Task<User> getUserFromHeader(string authHeader)
        {
            int tokenid = _accountServices.Decrypt(authHeader);
            var userExists = await _repoCategory.UserRepo.Get(tokenid);
            return userExists;
        }
    }
}