using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zembil.ErrorHandler;
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
        private readonly HelperMethods _helperMethods;

        public CategoryController(IRepositoryWrapper repoWrapper, IAccountService accountServices, IMapper mapper)
        {
            _repoCategory = repoWrapper;
            _accountServices = accountServices;
            _mapper = mapper;
            _helperMethods = HelperMethods.getInstance(repoWrapper, accountServices);
        }


        [AllowAnonymous]
        [HttpGet]
        public async Task<IEnumerable<Category>> GetAllCategory()
        {
            List<Category> Categories = await _repoCategory.CategoryRepo.GetAll();
            return Categories;
        }

        [HttpPost]
        public async Task<ActionResult<Category>> AddCategory(CategoryDto categoryDto)
        {

            //TODO: add role auth here     only admins are allowed
            var user = await _helperMethods.getUserFromHeader(Request.Headers["Authorization"]);
            if (user.Role.ToLower() != "admin")
            {
                throw new CustomAppException(new ErrorDetail() { StatusCode = 401, Message = "Admin access required!", Status = "fail" });
            }
            var category = _mapper.Map<Category>(categoryDto);
            await _repoCategory.CategoryRepo.Add(category);
            return category;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Category>> DeleeteCategory(int id)
        {

            //only admins are allowed
            var user = await _helperMethods.getUserFromHeader(Request.Headers["Authorization"]);
            if (user.Role.ToLower() != "admin")
            {
                throw new CustomAppException(new ErrorDetail() { StatusCode = 401, Message = "Admin access required!", Status = "fail" });
            }
            var category = await _repoCategory.CategoryRepo.Delete(id);
            return category;
        }
    }
}