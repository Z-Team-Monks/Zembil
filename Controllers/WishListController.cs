using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zembil.Models;
using Zembil.Repositories;
using Zembil.Services;

namespace Zembil.Controllers
{
    [Authorize]
    [Route("api/v1/cart")]
    [ApiController]
    public class WishListController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repoCart;
        private readonly IAccountService _accountService;
        public WishListController(IRepositoryWrapper repoWrapper, IAccountService accountService, IMapper mapper)
        {
            _mapper = mapper;
            _repoCart = repoWrapper;
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WishListItem>>> GetCart()
        {
            var user = await getUserFromHeader(Request.Headers["Authorization"]);
            if (user == null) return Unauthorized();

            var cart = await _repoCart.WishListRepo.GetCart(user.UserId);
            return Ok(cart);
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<WishListItem>> GetCartItem(int Id)
        {
            var user = await getUserFromHeader(Request.Headers["Authorization"]);
            if (user == null) return Unauthorized();

            var cartItem = await _repoCart.WishListRepo.Get(Id);
            if (cartItem == null) return NotFound("Cart Item doesn't exist");
            return Ok(cartItem);
        }


        [HttpPost]
        public async Task<ActionResult<WishListItem>> AddToCart(WishListItem wishListItem)
        {
            var user = await getUserFromHeader(Request.Headers["Authorization"]);
            if (user == null) return Unauthorized();

            wishListItem.UserId = user.UserId;
            wishListItem.DateAdded = DateTime.Now;
            await _repoCart.WishListRepo.Add(wishListItem);
            return CreatedAtAction(nameof(GetCartItem), new { Id = wishListItem.WishListItemId }, wishListItem);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItem(int id)
        {
            var user = await getUserFromHeader(Request.Headers["Authorization"]);

            var wishListFromRepo = await _repoCart.WishListRepo.Get(id);
            if (wishListFromRepo == null) return NotFound();
            if (user == null || wishListFromRepo.UserId != user.UserId) return Unauthorized();

            await _repoCart.WishListRepo.Delete(wishListFromRepo.WishListItemId);
            return Ok();
        }

        private async Task<User> getUserFromHeader(string authHeader)
        {
            int tokenid = _accountService.Decrypt(authHeader);
            var userExists = await _repoCart.UserRepo.Get(tokenid);
            return userExists;
        }
    }
}
