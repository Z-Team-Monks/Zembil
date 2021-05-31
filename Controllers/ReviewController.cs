using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Zembil.ErrorHandler;
using Zembil.Models;
using Zembil.Repositories;
using Zembil.Services;
using Zembil.Utils;
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
        private readonly HelperMethods _helperMethods;

        public ReviewController(IRepositoryWrapper repoWrapper, IAccountService accountServices, IMapper mapper)
        {
            _repoReview = repoWrapper;
            _accountServices = accountServices;
            _mapper = mapper;
            _helperMethods = HelperMethods.getInstance(repoWrapper, accountServices);
        }

        [HttpPost]
        public async Task<ActionResult<Review>> AddReview(int id, ReviewDto reviewDto)
        {
            // can't give multiple review for same product
            var productExist = await _repoReview.ProductRepo.Get(id);
            var userExists = await _helperMethods.getUserFromHeader(Request.Headers["Authorization"]);
            var shopExists = await _repoReview.ShopRepo.Get(productExist.ShopId);

            if (productExist == null)
            {
                throw new CustomAppException(new ErrorDetail() { StatusCode = 404, Message = "Product Doesn't Exist", Status = "Fail" });
            }

            if(shopExists == null)
            {
                throw new CustomAppException(new ErrorDetail() { StatusCode = 404, Message = "Shop Doesn't Exist", Status = "Fail" });
            }            

            if (await _repoReview.ReviewRepo.GetRevieweByUserAndProduct(id, userExists.UserId) != null)
            {
                throw new CustomAppException(new ErrorDetail() { Status = "fail", StatusCode = (int)HttpStatusCode.BadRequest, Message = "User Already gave review for the product!" });
            }

            var reviewForRepo = _mapper.Map<Review>(reviewDto);
            reviewForRepo.UserId = userExists.UserId;
            reviewForRepo.ProductId = id;
            reviewForRepo.ReviewDate = DateTime.Now;

            // notification for shop owner
            var newNotification = new Notification
            {
                UserId = shopExists.OwnerId,
                NotificationMessage = $"{userExists.Username} reviewed {productExist.ProductName} from your shop {shopExists.ShopName}.",
                Seen = false,
            };

            await _repoReview.NotificationRepo.Add(newNotification);            
            await _repoReview.ReviewRepo.Add(reviewForRepo);
            return Ok(reviewForRepo); // created at here
        }


        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviewesOfAProduct(int id)
        {
            var productExist = await _repoReview.ProductRepo.Get(id);
            if (productExist == null)
            {
                throw new CustomAppException(new ErrorDetail() { StatusCode = 404, Message = "Product Doesn't Exist", Status = "Fail" });
            }

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
        public async Task<ActionResult<Review>> GetReview(int reviewId)
        {
            var reviewExists = await _repoReview.ReviewRepo.GetRevieweById(reviewId);
            if (reviewExists == null)
            {
                throw new CustomAppException(new ErrorDetail() { StatusCode = 404, Message = "No review found with that id", Status = "Fail" });
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
                throw new CustomAppException(new ErrorDetail() { StatusCode = 403, Message = "No review found with that id", Status = "Fail" });
            }

            if (reviewExists.UserId != userExists.UserId)
            {
                throw new CustomAppException(new ErrorDetail() { StatusCode = 403, Message = "Current user can't modify this review", Status = "Fail" });
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
            var userExists = await _helperMethods.getUserFromHeader(Request.Headers["Authorization"]);

            var reviewExists = await _repoReview.ReviewRepo.Get(reviewId);
            if (reviewExists == null)
            {
                throw new CustomAppException(new ErrorDetail() { StatusCode = 404, Message = "No review found with that id", Status = "Fail" });
            }

            if (reviewExists.UserId != userExists.UserId)
            {
                throw new CustomAppException(new ErrorDetail() { StatusCode = 403, Message = "Current user can't delete this review", Status = "Fail" });
            }
            await _repoReview.ReviewRepo.Delete(reviewId);
            return Ok();
        }
    }
}
