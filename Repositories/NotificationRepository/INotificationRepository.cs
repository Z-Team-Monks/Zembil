using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zembil.Models;

namespace Zembil.Repositories
{
    public interface INotificationRepository : IRepositoryBase<Notification>
    {
        Task<IEnumerable<Notification>> GetUserNotifications(int userid);
    }
}
