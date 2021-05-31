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
    [Route("api/v1/users/notification")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repoNotification;
        private readonly IAccountService _accountService;
        public NotificationController(IRepositoryWrapper repoWrapper, IAccountService accountService, IMapper mapper)
        {
            _mapper = mapper;
            _repoNotification = repoWrapper;
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotificationDto>>> GetNotifications()
        {
            var userExists = await getUserFromHeader(Request.Headers["Authorization"]);

            if (userExists == null)
            {
                return Unauthorized();
            }
            var notifications = await _repoNotification.NotificationRepo.GetUserNotifications(userExists.UserId);            
            var notificationDtos = new List<NotificationDto>();

            foreach (Notification notification in notifications)
            {
                notificationDtos.Add(_mapper.Map<NotificationDto>(notification));
            }

            foreach(Notification notification in notifications)
            {
                notification.Seen = true;
                await _repoNotification.NotificationRepo.Update(notification);
            }
            return Ok(notificationDtos);
        }


        private async Task<User> getUserFromHeader(string authHeader)
        {
            int tokenid = _accountService.Decrypt(authHeader);
            var userExists = await _repoNotification.UserRepo.Get(tokenid);
            return userExists;
        }
    }
}
