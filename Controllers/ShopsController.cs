using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Zembil.Models;
using Zembil.Repositories;
using Zembil.Services;

namespace Zembil.Controllers
{
    [Authorize]
    [Route("api/v1/shops")]
    [ApiController]
    public class ShopsController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IAccountService _accountService;

        public ShopsController(IRepositoryWrapper repository, IAccountService accountService)
        {
            _repository = repository;
            _accountService = accountService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IEnumerable<Shop>> GetShops()
        {
            var results = await _repository.ShopRepo.GetAll();
            return results;
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Shop>> GetShop(int id)
        {
            try
            {
                var result = await _repository.ShopRepo.Get(id);
                if (result == null) return NotFound();
                return result;
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Shop>> CreateShop(Shop shop)
        {
            string authHeader = Request.Headers["Authorization"];
            int tokenid = _accountService.Decrypt(authHeader);

            var userExists = await _repository.UserRepo.Get(tokenid);

            if (userExists == null) return NotFound("User doesn't Exist");

            try
            {
                shop.OwnerId = tokenid;
                var newShop = await _repository.ShopRepo.Add(shop);

                return CreatedAtAction(nameof(GetShop), new { Id = newShop.ShopId }, newShop);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }


        [HttpPost("{shopId:int}/Likes")]
        public async Task<ActionResult> LikeShop(int shopId)
        {
            string authHeader = Request.Headers["Authorization"];
            int tokenid = _accountService.Decrypt(authHeader);

            var userExists = await _repository.UserRepo.Get(tokenid);
            var likeExists = _repository.ShopRepo.LikeExists(userExists.Id, shopId);
            if (!likeExists) return BadRequest("Can't retract shop not liked before");



            if (userExists == null) return NotFound("User doesn't Exist");

            ShopLike shoplike = new ShopLike
            {
                UserId = tokenid,
                ShopId = shopId
            };

            await _repository.ShopRepo.LikeShop(shoplike);
            return Ok();
        }

        [HttpDelete("{shopId:int}/Likes")]
        public async Task<ActionResult> RetractLike(int shopId)
        {
            string authHeader = Request.Headers["Authorization"];
            int tokenid = _accountService.Decrypt(authHeader);

            var userExists = await _repository.UserRepo.Get(tokenid);
            var likeExists = _repository.ShopRepo.LikeExists(userExists.Id, shopId);

            if (!likeExists) return BadRequest("Can't retract shop not liked before");
            //if (userExists == null) return NotFound("User doesn't Exist");            

            await _repository.ShopRepo.RetractLike(userExists.Id, shopId);
            return Ok();
        }


        //Get number of likes using search query
    }
}