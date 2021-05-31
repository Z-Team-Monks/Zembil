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

        [HttpGet("users/{id}")]
        public async Task<ActionResult<UserGetDto>> GetUser(int id)
        {
            var userExists = await _helperMethods.getUserFromHeader(Request.Headers["Authorization"]);
            return _mapper.Map<UserGetDto>(userExists);
        }

        [AllowAnonymous]
        [HttpPost("users")]
        public async Task<ActionResult<User>> CreateUser([FromBody] UserCreateDto userCreteDto)
        {
            var user = _mapper.Map<User>(userCreteDto);
            user.Password = _accountService.HashPassword(user.Password);
            user.Role = "user";
            user.DateAccountCreated = DateTime.Now;
            await _repoUser.UserRepo.Add(user);
            var userDTO = _mapper.Map<UserGetDto>(user);
            return CreatedAtAction(nameof(GetUser), new { Id = userDTO.UserId }, userDTO);
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
                    return CreatedAtAction(nameof(GetUser), new { Id = NewUser.UserId }, NewUser);
                }
            }

            throw new CustomAppException(new ErrorDetail() { StatusCode = 404, Message = "Not authorized for this user", Status = "Fail" });
        }

        [HttpPut("users/{id}")]
        public async Task<ActionResult<User>> UpdateUser(int id, [FromBody] User user)
        {
            var userExists = await _helperMethods.getUserFromHeader(Request.Headers["Authorization"]);
            if(userExists.UserId != id)
            {
                throw new CustomAppException(new ErrorDetail() { StatusCode = 404, Message = "Not authorized for this user", Status = "Fail" });
            }
            else
            {
                user.UserId = id;
                await _repoUser.UserRepo.Update(user);
            }
            return NoContent();
        }
    }
}