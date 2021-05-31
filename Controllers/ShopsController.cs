using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using Zembil.ErrorHandler;
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
        private readonly HelperMethods _helperMethods;

        public ShopsController(IRepositoryWrapper repository, IAccountService accountService, IMapper mapper)
        {
            _repository = repository;
            _accountService = accountService;
            _mapper = mapper;
            _helperMethods = HelperMethods.getInstance(repository, accountService);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IEnumerable<Shop>> GetShops([FromQuery] QueryFilterParams queryParams)
        {
            var results = await _repository.ShopRepo.FilterProducts(queryParams);
            var shops = _mapper.Map<List<Shop>>(results);
            return shops;
        }

        [AllowAnonymous]
        [Route("{id:int}/products")]
        [HttpGet]
        public async Task<IEnumerable<ProductGetBatchDto>> GetProductsOfShop(int id)
        {
            var results = await _repository.ShopRepo.GetAllProductsOfShop(id);
            var products = _mapper.Map<List<Product>>(results);
            return _mapper.Map<IEnumerable<ProductGetBatchDto>>(products);
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Shop>> GetShop(int id)
        {
            try
            {
                var result = await _repository.ShopRepo.GetShopWithLocation(id);
                if (result == null)
                {
                    throw new CustomAppException(new ErrorDetail() {StatusCode = 404,Message = "Shop Doesn't Exist", Status = "Fail" });
                }
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
            await _repository.LocationRepo.Delete(result.ShopLocationDto.LocationId);
            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<Shop>> UpdateShop(int id, [FromBody] JsonPatchDocument<ShopChangeDto> shopChangeDto)
        {
            var userExists = await _helperMethods.getUserFromHeader(Request.Headers["Authorization"]);

            //use this dto inorder not to accept status updates in this route
            ShopChangeDto updatedDto;
            var ShopExist = await _repository.ShopRepo.Get(id);
            if(ShopExist == null)
            {
                throw new CustomAppException(new ErrorDetail() { StatusCode = 404, Message = "Shop Doesn't Exist", Status = "Fail" });
            }

            if ( ShopExist.ShopId != userExists.UserId)
            {
                throw new CustomAppException(new ErrorDetail() { StatusCode = 403, Message = "Current user can't modify this shop", Status = "Fail" });  //Not your shop
            }
            else
            {
                updatedDto = _mapper.Map<ShopChangeDto>(ShopExist);
                shopChangeDto.ApplyTo(updatedDto);                
            }

            ShopExist = _mapper.Map<Shop>(updatedDto);
            await _repository.ShopRepo.Update(ShopExist);
            return Ok(ShopExist);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Shop>> UpdateFullShop(int id, [FromBody] Shop shop)
        {
            var userExists = await _helperMethods.getUserFromHeader(Request.Headers["Authorization"]);

            var ShopExist = await _repository.ShopRepo.Get(id);

            if (ShopExist == null)
            {
                throw new CustomAppException(new ErrorDetail() { StatusCode = 404, Message = "Shop Doesn't Exist", Status = "Fail" });  
            }
            if(ShopExist.OwnerId != userExists.UserId)
            {
                throw new CustomAppException(new ErrorDetail() { StatusCode = 403, Message = "Current user can't modify this shop", Status = "Fail" });  //Not your shop
            }
            if (shop.ShopLocationDto == null)
            {
                throw new CustomAppException(new ErrorDetail() { StatusCode = 400, Message = "Associated location is also required!", Status = "Fail" });
            }
            shop.ShopId = id;
            await _repository.ShopRepo.Update(shop);
            return Ok(shop);
        }

        [HttpPost]
        public async Task<ActionResult<Shop>> CreateShop(ShopCreateDto shopCreateDto, [FromForm] IFormFile file = null)
        {
            var userExists = await _helperMethods.getUserFromHeader(Request.Headers["Authorization"]);

            if (shopCreateDto == null)
            {
                throw new CustomAppException(new ErrorDetail() { StatusCode = 400, Message = "Shop field is required!", Status = "Fail" });
            }
            try
            {

                if (file != null)
                {
                    var size = file.Length;
                    Console.WriteLine($"File upload size: {size}");
                    var filePath = Path.Combine(@Directory.GetCurrentDirectory() + "/Uploads/", file.FileName);
                    if (file.Length > 0)
                    {
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                            shopCreateDto.CoverImage = filePath;
                        }
                    }
                }

                var shop = _mapper.Map<Shop>(shopCreateDto);
                shop.OwnerId = userExists.UserId;
                shop.IsActive = null;

                var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
                var loc = geometryFactory.CreatePoint(new Coordinate(shop.ShopLocationDto.Latitude, shop.ShopLocationDto.Longitude));
                var newLoc = new ShopLocation() { GeoLoacation = loc, LocationName = shopCreateDto.ShopLocationDto.LocationName };

                var newLocation = await _repository.LocationRepo.Add(newLoc);
                shop.ShopLocationId = newLocation.LocationId;
                var newShop = await _repository.ShopRepo.Add(shop);

                return CreatedAtAction(nameof(GetShop), new { Id = newShop.ShopId }, newShop);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }


        }

        [HttpGet("{shopId:int}/follow")]
        public async Task<ActionResult> Getfollow(int shopId)
        {
            var count = await _repository.ShopRepo.GetFollow(shopId);
            return Ok($"shop follow: {count}");
        }

        [HttpPost("{shopId:int}/follow")]
        public async Task<ActionResult> FollowShop(int shopId)
        {
            var userExists = await _helperMethods.getUserFromHeader(Request.Headers["Authorization"]);
            var followExists = _repository.ShopRepo.FollowExists(userExists.UserId, shopId);

            if (userExists == null) return NotFound("User doesn't Exist");
            if (followExists)
            {
                throw new CustomAppException(new ErrorDetail() { StatusCode = 400, Message = "Shop is already liked by the user!", Status = "Fail" });
            }

            ShopFollow shopFollow = new ShopFollow
            {
                UserId = userExists.UserId,
                ShopId = shopId
            };

            await _repository.ShopRepo.FollowShop(shopFollow);
            return Ok();
        }

        [HttpDelete("{shopId:int}/follow")]
        public async Task<ActionResult> RetractFollow(int shopId)
        {
            var userExists = await _helperMethods.getUserFromHeader(Request.Headers["Authorization"]);

            var followExists = _repository.ShopRepo.FollowExists(userExists.UserId, shopId);

            if (!followExists)
            {
                throw new CustomAppException(new ErrorDetail() { StatusCode = 400, Message = "Current User doesn't follow this shop!", Status = "Fail" });
            }            

            await _repository.ShopRepo.RetractFollow(userExists.UserId, shopId);
            return Ok();
        }

        [HttpPut("{shopId:int}/status")]
        public async Task<ActionResult> ChangeStatus(int shopId, [FromBody] ShopStatusDto shopStatus)
        {
            var userExists = await _helperMethods.getUserFromHeader(Request.Headers["Authorization"]);
            const string admin = "admin";

            if (userExists.Role.ToLower() != admin)
            {
                throw new CustomAppException(new ErrorDetail() { StatusCode = 403, Message = "Current user can't modify status", Status = "Fail" });
            }

            var shopRepo = await _repository.ShopRepo.Get(shopId);
            if (shopRepo == null)
            {
                throw new CustomAppException(new ErrorDetail() { StatusCode = 404, Message = "Shop Doesn't Exist", Status = "Fail" });
            }

            shopRepo.IsActive = shopStatus.IsActive;
            await _repository.ShopRepo.Update(shopRepo);
            return Ok(shopRepo);
        }       
    }
}