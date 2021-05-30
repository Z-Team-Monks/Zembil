using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Zembil.Models;
using Zembil.Repositories;
using Zembil.Services;
using Zembil.Utils;
using Zembil.Views;

namespace Zembil.Controllers
{
    [Authorize]
    [Route("api/v1/shops")]
    [ApiController]
    public class ShopsController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public ShopsController(IRepositoryWrapper repository, IAccountService accountService, IMapper mapper)
        {
            _repository = repository;
            _accountService = accountService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IEnumerable<ShopBatchGetDto>> GetShops([FromQuery] QueryFilterParams queryParams)
        {
            var results = await _repository.ShopRepo.FilterProducts(queryParams);
            var shops = _mapper.Map<List<ShopBatchGetDto>>(results);
            return shops;
        }

        [AllowAnonymous]
        [Route("{id:int}/products")]
        [HttpGet]
        public async Task<IEnumerable<Product>> GetProductsOfShop(int id)
        {
            var results = await _repository.ShopRepo.GetAllProductsOfShop(id);
            var products = _mapper.Map<List<Product>>(results);
            return products;
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ShopDto>> GetShop(int id)
        {
            try
            {
                var result = await _repository.ShopRepo.GetShopWithLocation(id);
                if (result == null) return NotFound();
                var shopDto = _mapper.Map<ShopDto>(result);
                return shopDto;
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }
        
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteShop(int id)
        {
            var userExists = getUserFromHeader(Request.Headers["Authorization"]);

            if (userExists == null)
            {
                return Unauthorized();
            }

            var ShopExist = await _repository.ShopRepo.Get(id);
            if (ShopExist == null)
            {
                return NotFound("No Shop found with that id for current user!");
            }

            var result = await _repository.ShopRepo.Delete(id);
            await _repository.LocationRepo.Delete(result.ShopLocationId);
            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<Shop>> UpdateShop(int id, [FromBody] JsonPatchDocument<ShopChangeDto> shopChangeDto)
        {
            var userExists = getUserFromHeader(Request.Headers["Authorization"]);

            if (userExists == null)
            {
                return Unauthorized();
            }

            //use this dto inorder not to accept status updates in this route
            ShopChangeDto updatedDto;
            var ShopExist = await _repository.ShopRepo.Get(id);

            if (ShopExist == null || ShopExist.ShopId != userExists.Id)
            {
                return NotFound("No Shop found with that id for current user!");
            }
            else
            {
                updatedDto = _mapper.Map<ShopChangeDto>(ShopExist);
                shopChangeDto.ApplyTo(updatedDto);
                //ShopChangeDto.ApplyTo(shopChangeDto, ModelState);
            }

            ShopExist = _mapper.Map<Shop>(updatedDto);
            await _repository.ShopRepo.Update(ShopExist);
            return Ok(ShopExist);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ShopDto>> UpdateFullShop(int id, [FromBody] ShopWithLocation shopWithLocation)
        {
            var location = _mapper.Map<Location>(shopWithLocation.Location);

            var ShopExist = await _repository.ShopRepo.Get(id);

            if (ShopExist == null)
            {
                return NotFound("No Shop found with that id!");
            }
            if (location == null)
            {
                return BadRequest("Associated location is also required!");
            }

            _mapper.Map(shopWithLocation.Shop, ShopExist);            
            location.LocationId = ShopExist.ShopLocationId;
            //Console.WriteLine($"{ShopExist.ShopLocationId}");
            await _repository.LocationRepo.Update(location);
            await _repository.ShopRepo.Update(ShopExist);
            return _mapper.Map<ShopDto>(ShopExist);
        }

        [HttpPost]
        public async Task<ActionResult<Shop>> CreateShop(ShopCreateDto shopDto)
        {
            string authHeader = Request.Headers["Authorization"];
            int tokenid = _accountService.Decrypt(authHeader);

            var userExists = await _repository.UserRepo.Get(tokenid);

            if (userExists == null) return NotFound("User doesn't Exist");

            if (shopDto == null)
            {
                return BadRequest("shop can't be empty");
            }            
            var shopRepo = _mapper.Map<Shop>(shopDto);
            shopRepo.OwnerId = tokenid;
            shopRepo.IsActive = null;
            shopRepo.ShopLocation = _mapper.Map<Location>(shopDto.ShopLocation);
            try
            {
                var newShop = await _repository.ShopRepo.Add(shopRepo);
                return CreatedAtAction(nameof(GetShop), new { Id = newShop.ShopId }, newShop);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [AllowAnonymous]
        [HttpGet("{shopId:int}/follow")]
        public async Task<ActionResult> Getfollow(int shopId)
        {            
            var count = await _repository.ShopRepo.GetFollow(shopId);
            return Ok($"shop follow: {count}");
        }

        [HttpPost("{shopId:int}/follow")]
        public async Task<ActionResult> FollowShop(int shopId)
        {
            string authHeader = Request.Headers["Authorization"];
            int tokenid = _accountService.Decrypt(authHeader);

            var userExists = await _repository.UserRepo.Get(tokenid);
            var followExists = _repository.ShopRepo.FollowExists(userExists.UserId, shopId);

            if (userExists == null) return NotFound("User doesn't Exist");
            if (followExists) return BadRequest("Shop already liked by this user");

            ShopFollow shopFollow = new ShopFollow
            {
                UserId = tokenid,
                ShopId = shopId
            };

            await _repository.ShopRepo.FollowShop(shopFollow);
            return Ok();
        }

        [HttpDelete("{shopId:int}/follow")]
        public async Task<ActionResult> RetractFollow(int shopId)
        {
            string authHeader = Request.Headers["Authorization"];
            int tokenid = _accountService.Decrypt(authHeader);

            var userExists = await _repository.UserRepo.Get(tokenid);
            var followExists = _repository.ShopRepo.FollowExists(userExists.UserId, shopId);

            if (!followExists) return BadRequest("Can't retract shop not liked before");
            //if (userExists == null) return NotFound("User doesn't Exist");            

            await _repository.ShopRepo.RetractFollow(userExists.UserId, shopId);
            return Ok();
        }

        [HttpPut("{shopId:int}/status")]
        public async Task<ActionResult> ChangeStatus(int shopId, [FromBody] ShopStatusDto shopStatus)
        {
            var user = await getUserFromHeader(Request.Headers["Authorization"]);
            const string admin = "admin";
            if (user == null || user.Role.ToLower() != admin)
            {
                return Unauthorized();
            }

            var shopRepo = await _repository.ShopRepo.Get(shopId);
            if (shopRepo == null) return NotFound("shop doesn't exist");

            if (shopStatus == null)
            {
                return BadRequest("isActive should be included");
            }

            shopRepo.IsActive = shopStatus.IsActive;
            await _repository.ShopRepo.Update(shopRepo);
            return Ok(shopRepo);
        }

        private async Task<User> getUserFromHeader(string authHeader)
        {
            int tokenid = _accountService.Decrypt(authHeader);
            var userExists = await _repository.UserRepo.Get(tokenid);
            return userExists;
        }
    }
}