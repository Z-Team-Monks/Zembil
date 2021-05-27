using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
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

        [AllowAnonymous]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Shop>> DeleteShop(int id)
        {
            var result = await _repository.ShopRepo.Delete(id);
            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<Shop>> UpdateShop(int id, [FromBody] JsonPatchDocument<Shop> Shop)
        {

            var ShopExist = await _repository.ShopRepo.Get(id);

            if (ShopExist == null)
            {
                return NotFound("No Shop found with that id!");
            }
            else
            {
                Shop.ApplyTo(ShopExist, ModelState);
            }
            await _repository.ShopRepo.Update(ShopExist);
            return Ok(ShopExist);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Shop>> UpdateFullShop(int id, [FromBody] Shop Shop)
        {

            var ShopExist = await _repository.ShopRepo.Get(id);

            if (ShopExist == null)
            {
                return NotFound("No Shop found with that id!");
            }
            Shop.ShopId = id;
            await _repository.ShopRepo.Update(Shop);
            return Ok(Shop);
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

        [HttpGet("{shopId:int}/likes")]
        public async Task<ActionResult> GetLikes(int shopId)
        {
            string authHeader = Request.Headers["Authorization"];
            int tokenid = _accountService.Decrypt(authHeader);

            var userExists = await _repository.UserRepo.Get(tokenid);

            var count = await _repository.ShopRepo.GetLikes(shopId);
            return Ok($"shop likes: {count}");
        }
        [HttpPost("{shopId:int}/likes")]
        public async Task<ActionResult> LikeShop(int shopId)
        {
            string authHeader = Request.Headers["Authorization"];
            int tokenid = _accountService.Decrypt(authHeader);

            var userExists = await _repository.UserRepo.Get(tokenid);
            var likeExists = _repository.ShopRepo.LikeExists(userExists.Id, shopId);

            if (userExists == null) return NotFound("User doesn't Exist");
            if (likeExists) return BadRequest("Shop already liked by this user");

            ShopLike shoplike = new ShopLike
            {
                UserId = tokenid,
                ShopId = shopId
            };

            await _repository.ShopRepo.LikeShop(shoplike);
            return Ok();
        }

        [HttpDelete("{shopId:int}/likes")]
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