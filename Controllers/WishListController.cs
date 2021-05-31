using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zembil.ErrorHandler;
using Zembil.Models;
using Zembil.Repositories;
using Zembil.Services;
using Zembil.Views;

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
        public async Task<ActionResult<IEnumerable<WishListDto>>> GetCart()
        {
            var user = await getUserFromHeader(Request.Headers["Authorization"]);
            if (user == null) return Unauthorized();

            var cart = await _repoCart.WishListRepo.GetCart(user.UserId);
            var cartDto = _mapper.Map<IEnumerable<WishListDto>>(cart);
            foreach (var wishItem in cartDto)
            {
                var product = await _repoCart.ProductRepo.Get(wishItem.ProductId);
                wishItem.Product = _mapper.Map<ProductGetBatchDto>(product);
            }

            return Ok(cartDto);
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<WishListItem>> GetCartItem(int Id)
        {
            var user = await getUserFromHeader(Request.Headers["Authorization"]);
            if (user == null) return Unauthorized();

            var cartItem = await _repoCart.WishListRepo.Get(Id);
            if (cartItem == null)
            {
                throw new CustomAppException(new ErrorDetail() { StatusCode = 404, Message = "Cart Item doesn't exist", Status = "Fail" });
            }
            return Ok(cartItem);
        }


        [HttpPost]
        public async Task<ActionResult<WishListItem>> AddToCart(WishListAddDto wishListItemDto)
        {
            var user = await getUserFromHeader(Request.Headers["Authorization"]);
            if (user == null) return Unauthorized();

            var wishListRepo = _mapper.Map<WishListItem>(wishListItemDto);
            wishListRepo.UserId = user.UserId;
            wishListRepo.DateAdded = DateTime.Now;
            await _repoCart.WishListRepo.Add(wishListRepo);
            return CreatedAtAction(nameof(GetCartItem), new { Id = wishListRepo.WishListItemId }, wishListRepo);
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
