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
        public UsersController(IRepositoryWrapper repoWrapper, IAccountService accountService, IMapper mapper)
        {
            _mapper = mapper;
            _repoUser = repoWrapper;
            _accountService = accountService;
        }

        [Route("users")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserGetDto>>> GetUsers()
        {
            string authHeader = Request.Headers["Authorization"];
            if (authHeader != null)
            {
                int tokenid = _accountService.Decrypt(authHeader);
                User currentUser = await _repoUser.UserRepo.Get(tokenid);
                if (currentUser.Role.ToLower().Equals("admin"))
                {
                    var users = await _repoUser.UserRepo.GetAll();
                    var usersDto = _mapper.Map<IEnumerable<UserGetDto>>(users);
                    return Ok(usersDto);
                }
            }

            throw new CustomAppException(new ErrorDetail() { StatusCode = 403, Message = "Not authorized for this user", Status = "Fail" });

        }

        [HttpGet("users/{id}")]
        public async Task<ActionResult<UserGetDto>> GetUser(int id)
        {
            string authHeader = Request.Headers["Authorization"];
            if (authHeader == null) return Unauthorized();
            int tokenid = _accountService.Decrypt(authHeader);

            if (id != tokenid)
            {
                return NotFound();
            }

            var user = await _repoUser.UserRepo.Get(id);
            if (user == null)
            {
                return NotFound();
            }
            return _mapper.Map<UserGetDto>(user);
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
            var userExist = await _repoUser.UserRepo.Get(user.UserId);
            if (id != user.UserId)
            {
                return BadRequest();
            }
            if (userExist == null)
            {
                return NotFound("No user found with that id!");
            }
            else
            {
                await _repoUser.UserRepo.Update(user);
            }
            return NoContent();
        }

    }
}