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
using Zembil.Views;

namespace Zembil.Controllers
{
    [Authorize]
    [Route("api/v1/products/{id}/reviews")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private IRepositoryWrapper _repoReview { get; set; }
        private readonly IAccountService _accountServices;
        private readonly IMapper _mapper;

        public ReviewController(IRepositoryWrapper repoWrapper, IAccountService accountServices, IMapper mapper)
        {
            _repoReview = repoWrapper;
            _accountServices = accountServices;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<Review>> AddReview(int id, ReviewDto review)
        {
            var productExist = await _repoReview.ProductRepo.Get(id);
            var userExists = await getUserFromHeader(Request.Headers["Authorization"]);

            if (productExist == null) return NotFound("No product found with that id!");
            if (userExists == null) return NotFound("User doesn't Exist");

            review.UserId = userExists.Id;
            review.ProductId = id;

            var reviewForRepo = _mapper.Map<Review>(review);
            await _repoReview.ReviewRepo.Add(reviewForRepo);
            return Ok(review);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviewesOfAProduct(int id)
        {
            var productExist = await _repoReview.ProductRepo.Get(id);
            if (productExist == null) return NotFound("No product found with that id!");

            var reviewesFromRepo = await _repoReview.ReviewRepo.GetReviewesOfProduct(id);
            var reviewesToReturn = _mapper.Map<IEnumerable<ReviewToReturnDto>>(reviewesFromRepo);

            foreach (var review in reviewesToReturn)
            {
                var userFromRepo = await _repoReview.UserRepo.Get(review.UserId);
                review.UserName = userFromRepo.Username;
            }
            return Ok(reviewesToReturn);
        }

        [AllowAnonymous]
        [HttpGet("{reviewId}")]
        public async Task<ActionResult> GetReview(int reviewId)
        {
            var reviewExists = await _repoReview.ReviewRepo.Get(reviewId);
            if (reviewExists == null)
            {
                return NotFound("No Review found with that id!");
            }
            var reviewToReturn = _mapper.Map<ReviewToReturnDto>(reviewExists);
            var userFromRepo = await _repoReview.UserRepo.Get(reviewToReturn.UserId);
            reviewToReturn.UserName = userFromRepo.Username;
            return Ok(reviewToReturn);
        }



        [HttpPut("{reviewId}")]
        public async Task<ActionResult<ReviewToReturnDto>> UpdateReview(int reviewId, [FromBody] ReviewToUpdateDto reviewDto)
        {
            var userExists = await getUserFromHeader(Request.Headers["Authorization"]);
            if (userExists == null) return NotFound("User doesn't Exist");

            var reviewExists = await _repoReview.ReviewRepo.Get(reviewId);
            if (reviewExists == null)
            {
                return NotFound("No Review found with that id!");
            }

            if (reviewExists.UserId != userExists.Id)
            {
                return Unauthorized("Not your review");
            }


            _mapper.Map(reviewDto, reviewExists);
            await _repoReview.ReviewRepo.Update(reviewExists);

            var reviewToReturn = _mapper.Map<ReviewToReturnDto>(reviewExists);
            reviewToReturn.UserName = userExists.Username;
            return Ok(reviewToReturn);


        }

        [HttpDelete("{reviewId}")]
        public async Task<ActionResult> DeleteAReview(int reviewId)//reviewid
        {
            var userExists = await getUserFromHeader(Request.Headers["Authorization"]);
            if (userExists == null) return NotFound("User doesn't Exist");

            var reviewExists = await _repoReview.ReviewRepo.Get(reviewId);
            if (reviewExists == null)
            {
                return NotFound("No Review found with that id!");
            }

            if (reviewExists.UserId != userExists.Id)
            {
                return Unauthorized("Not your review");
            }
            await _repoReview.ReviewRepo.Delete(reviewId);
            return Ok();
        }

        private async Task<User> getUserFromHeader(string authHeader)
        {
            int tokenid = _accountServices.Decrypt(authHeader);
            var userExists = await _repoReview.UserRepo.Get(tokenid);
            return userExists;
        }
    }
}
