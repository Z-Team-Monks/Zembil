using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zembil.Views
{
    public class NotificationDto
    {
        public int NotificationId { get; set; }
        public string NotificationMessage { get; set; }
        public bool Seen { get; set; }
    }
}
