using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zembil.Models;
using Zembil.Repositories;
using Zembil.Services;

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
        public AdsController(IRepositoryWrapper repoWrapper, IAccountService accountServices, IMapper mapper)
        {
            _repoAds = repoWrapper;
            _accountServices = accountServices;
            _mapper = mapper;
        }


        [Route("user")]
        [HttpPost]
        public async Task<ActionResult<Ads>> CreateAds([FromBody] Ads newAds)
        {
            var user = await getUserFromHeader(Request.Headers["Authorization"]);
            if (user == null) return Unauthorized();

            var ownerShops = await _repoAds.ShopRepo.GetShopsByOwner(user.UserId);
            if (ownerShops == null) return BadRequest("User doesn't own any shops");

            List<Shop> myShops = ownerShops.Where(s => s.ShopId == newAds.ShopId).ToList();

            if (myShops.Count() == 0) return BadRequest("Can't run adds on unavailable shops");

            var ads = await _repoAds.AdsRepo.Add(newAds);

            return Ok(ads);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IEnumerable<Ads>> GetAllAds()
        {
            var allAds = await _repoAds.AdsRepo.GetAll();
            return allAds;
        }


        [Route("admin")]
        [HttpPut]
        public async Task<ActionResult<Ads>> ApproveAds([FromBody] Ads ads)
        {
            User user = await getUserFromHeader(Request.Headers["Authorization"]);
            if (user == null) return Unauthorized();

            if (user.Role != "admin") return Unauthorized("Admin only");

            ads.IsActive = true;
            var newAds = await _repoAds.AdsRepo.Update(ads);

            return Ok(newAds);
        }

        [Route("admin")]
        [HttpDelete]
        public async Task<ActionResult<Ads>> DisapproveAds([FromBody] Ads ads)
        {
            User user = await getUserFromHeader(Request.Headers["Authorization"]);
            if (user == null) return Unauthorized();

            if (user.Role != "admin") return Unauthorized("Admin only");

            ads.IsActive = false;
            var newAds = await _repoAds.AdsRepo.Update(ads);

            return Ok(newAds);
        }


        private async Task<User> getUserFromHeader(string authHeader)
        {
            int tokenid = _accountServices.Decrypt(authHeader);
            var userExists = await _repoAds.UserRepo.Get(tokenid);
            return userExists;
        }

    }
}