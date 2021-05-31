using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zembil.DbContexts;
using Zembil.Models;

namespace Zembil.Repositories
{
    public class NotificationRepository : RepositoryBase<Notification>, INotificationRepository
    {
        public NotificationRepository(ZembilContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Notification>> GetUserNotifications(int userId)
        {
            var following = await _databaseContext.Notification.ToListAsync();
            following = following.Where(n => n.UserId == userId).ToList();
            return following;
            //var notifications = await _databaseContext.Set<Notification>().ToListAsync();
            //var userNotifications = notifications.Where(n => n.UserId == userId);
            //return userNotifications;
        }

    }
}
