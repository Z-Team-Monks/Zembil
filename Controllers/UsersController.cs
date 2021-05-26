using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Zembil.Models;
using Zembil.Repositories;
using Zembil.Services;
using Zembil.Views;

namespace Zembil.Controllers
{
    [Authorize]
    [Route("api/v1/users")]
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

        [HttpGet]
        public async Task<IEnumerable<User>> GetUsers()
        {
            //string authHeader = Request.Headers["Authorization"];
            //string username = _accountService.Decrypt(authHeader);
            //Console.WriteLine(username);

            return await _repoUser.UserRepo.GetAll();

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserGetDto>> GetUser(int id)
        {
            string authHeader = Request.Headers["Authorization"];
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
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser([FromBody] User user)
        {
            user.Password = _accountService.HashPassword(user.Password);
            var NewUser = await _repoUser.UserRepo.Add(user);
            return CreatedAtAction(nameof(GetUser), new { Id = NewUser.Id }, NewUser);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<User>> UpdateUser(int id, [FromBody] User user)
        {
            var userExist = await _repoUser.UserRepo.Get(user.Id);
            if (id != user.Id)
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

        [HttpGet("likes/{shopId}")]
        [HttpPost("likes/{shopId}")]
        [HttpDelete("likes/{shopId}")]
        public async Task<IActionResult> LikeAShop(int shopId)
        {
            string authHeader = Request.Headers["Authorization"];
            int tokenid = _accountService.Decrypt(authHeader);

            var userExists = await _repoUser.UserRepo.Get(tokenid);

            if (userExists == null) return NotFound("User doesn't Exist");

            var shopExists = await _repoUser.ShopRepo.Get(shopId);

            if (userExists == null) return NotFound("Shop doesn't Exist");

            //TODO: like implementation in the repo
            //await _repoUser.ShopRepo.Like();

            return Ok();
        }

    }
}