using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Zembil.ErrorHandler;
using Zembil.Models;
using Zembil.Repositories;
using Zembil.Services;
using Zembil.Utils;
using Zembil.Views;

namespace Zembil.Controllers
{

    [Route("api/v1")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repoUser;
        private readonly IAccountService _accountService;
        private readonly HelperMethods _helperMethods;

        public UsersController(IRepositoryWrapper repoWrapper, IAccountService accountService, IMapper mapper)
        {
            _mapper = mapper;
            _repoUser = repoWrapper;
            _accountService = accountService;
            _helperMethods = HelperMethods.getInstance(repoWrapper, accountService);
        }

        [Route("users")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserGetDto>>> GetUsers()
        {
            var currentUser = await _helperMethods.getUserFromHeader(Request.Headers["Authorization"]);
            if (currentUser.Role.ToLower().Equals("admin"))
            {
                var users = await _repoUser.UserRepo.GetAll();
                var usersDto = _mapper.Map<IEnumerable<UserGetDto>>(users);
                return Ok(usersDto);
            }

            throw new CustomAppException(new ErrorDetail() { StatusCode = 403, Message = "Not authorized for this user", Status = "Fail" });

        }

        [HttpGet("me")]
        public async Task<ActionResult<UserGetDto>> GetMe()
        {
            var userExists = await _helperMethods.getUserFromHeader(Request.Headers["Authorization"]);
            Console.WriteLine("before");
            var user = await _repoUser.UserRepo.Get(userExists.UserId);
            Console.WriteLine("after");
            return _mapper.Map<UserGetDto>(user);
        }

        [HttpGet("user/shops")]
        public async Task<ActionResult<ShopReturnDto>> GetUserShops(int id)
        {
            var userExists = await _helperMethods.getUserFromHeader(Request.Headers["Authorization"]);
            var shops = await _repoUser.ShopRepo.GetShopsByOwner(userExists.UserId);
            return _mapper.Map<ShopReturnDto>(shops);
        }

        [AllowAnonymous]
        [HttpPost("user")]
        public async Task<ActionResult<User>> CreateUser([FromBody] UserCreateDto userCreteDto)
        {
            var user = _mapper.Map<User>(userCreteDto);
            user.Password = _accountService.HashPassword(user.Password);
            user.Role = "user";
            user.DateAccountCreated = DateTime.Now;
            //user.Username = user.Username.ToLower();
            await _repoUser.UserRepo.Add(user);
            var userDTO = _mapper.Map<UserGetDto>(user);
            return CreatedAtAction(nameof(CreateUser), new { Id = userDTO.UserId }, userDTO);
        }

        [HttpPost("admin")]
        public async Task<ActionResult<User>> CreateAdmin([FromBody] User user)
        {
            string authHeader = Request.Headers["Authorization"];
            if (authHeader != null)
            {
                int tokenid = _accountService.Decrypt(authHeader);
                User currentUser = await _repoUser.UserRepo.Get(tokenid);

                if (currentUser.Role.ToLower().Equals("admin"))
                {
                    user.Password = _accountService.HashPassword(user.Password);
                    user.Role = "admin";
                    var NewUser = await _repoUser.UserRepo.Add(user);
                    NewUser.DateAccountCreated = DateTime.Now;
                    return CreatedAtAction(nameof(CreateAdmin), new { Id = NewUser.UserId }, NewUser);
                }
            }

            throw new CustomAppException(new ErrorDetail() { StatusCode = 403, Message = "Not authorized for this user", Status = "Fail" });
        }
        [HttpGet("admin/status")]
        public async Task<ActionResult<Object>> GetStatus()
        {
            string authHeader = Request.Headers["Authorization"];
            if (authHeader != null)
            {
                int tokenid = _accountService.Decrypt(authHeader);
                User currentUser = await _repoUser.UserRepo.Get(tokenid);

                if (currentUser.Role.ToLower().Equals("admin"))
                {
                    return await _repoUser.UserRepo.GetZembilStatus();
                }
            }

            throw new CustomAppException(new ErrorDetail() { StatusCode = 403, Message = "Not authorized for this user", Status = "Fail" });
        }

        [HttpPut("user")]
        public async Task<ActionResult<User>> UpdateUser([FromBody] User user)
        {
            var userExists = await _helperMethods.getUserFromHeader(Request.Headers["Authorization"]);
            user.UserId = userExists.UserId;
            try
            {
                await _repoUser.UserRepo.Update(user);
            }
            catch (System.Exception)
            {
                throw new CustomAppException(new ErrorDetail() { StatusCode = 500, Message = "Error, cannot update user! please make sure you are logged in", Status = "Fail" });
            }
            return NoContent();
        }
    }
}