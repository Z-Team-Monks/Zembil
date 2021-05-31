using System;
using System.Collections.Generic;
using System.Linq;
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
    [Route("api/v1/ads")]
    [ApiController]
    public class AdsController : ControllerBase
    {
        private IRepositoryWrapper _repoAds;
        private readonly IAccountService _accountServices;
        private readonly IMapper _mapper;
        private readonly HelperMethods _helperMethods;

        public AdsController(IRepositoryWrapper repoWrapper, IAccountService accountServices, IMapper mapper)
        {
            _repoAds = repoWrapper;
            _accountServices = accountServices;
            _mapper = mapper;
            _helperMethods = HelperMethods.getInstance(repoWrapper, accountServices);
        }


        [Route("user")]
        [HttpPost]
        public async Task<ActionResult<Ads>> CreateAds([FromBody] AdsCreateDto newAds)
        {
            var user = await _helperMethods.getUserFromHeader(Request.Headers["Authorization"]);            
            var ownerShops = await _repoAds.ShopRepo.GetShopsByOwner(user.UserId);
            if (ownerShops == null) throw new CustomAppException(new ErrorDetail() { StatusCode = 403, Message = "Wrong shop! Cannot advertize shop that doen't belong to this user.", Status = "fail" });

            List<Shop> myShops = ownerShops.Where(s => s.ShopId == newAds.ShopId).ToList();

            if (myShops.Count() == 0) throw new CustomAppException(new ErrorDetail() { StatusCode = 404, Message = "Current your does not own any shops!", Status = "fail" });

            newAds.IsActive = false;
            var ads = _mapper.Map<Ads>(newAds);
            await _repoAds.AdsRepo.Add(ads);
            return Ok(ads);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IEnumerable<Ads>> GetAllAds()
        {
            var allAds = await _repoAds.AdsRepo.GetAdsWithShops();
            return allAds;
        }

        [AllowAnonymous]
        [HttpGet("{shopId}")]
        public async Task<IEnumerable<Ads>> GetAllAdsForShop(int shopId)
        {
            var allAds = await _repoAds.AdsRepo.GetAdsForShop(shopId);
            return allAds;
        }

        [AllowAnonymous]
        [Route("active")]
        [HttpGet]
        public async Task<IEnumerable<Ads>> GetAllActiveAds()
        {
            var allAds = await _repoAds.AdsRepo.GetActiveAds();
            return allAds;
        }
        [AllowAnonymous]
        [HttpGet("inactive")]
        public async Task<IEnumerable<Ads>> GetAllInActiveAds()
        {
            var allAds = await _repoAds.AdsRepo.GetInActiveAds();
            return allAds;
        }


        [Route("admin")]
        [HttpPut]
        public async Task<ActionResult<Ads>> ApproveAds([FromBody] Ads ads)
        {
            var user = await _helperMethods.getUserFromHeader(Request.Headers["Authorization"]);
            // if (user == null) return Unauthorized();

            if (user.Role != "admin") throw new CustomAppException(new ErrorDetail() { StatusCode = 401, Message = "Admin access required!", Status = "fail" });
            ads.IsActive = true;
            var newAds = await _repoAds.AdsRepo.Update(ads);

            return Ok(newAds);
        }

        [Route("admin")]
        [HttpDelete]
        public async Task<ActionResult<Ads>> DisapproveAds([FromBody] Ads ads)
        {
            var user = await _helperMethods.getUserFromHeader(Request.Headers["Authorization"]);
            // if (user == null) return Unauthorized();

            if (user.Role != "admin") throw new CustomAppException(new ErrorDetail() { StatusCode = 401, Message = "Admin access required!", Status = "fail" });

            ads.IsActive = false;
            var newAds = await _repoAds.AdsRepo.Update(ads);

            return Ok(newAds);
        }        

    }
}