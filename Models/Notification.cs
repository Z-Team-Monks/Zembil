using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Zembil.Models
{
    public class Notification
    {
        [Key]
        public int NotificatoinId { get; set; }

        [Required]
        public int UserId { get; set; }

        [MaxLength(500)]
        public string NotificationMessage { get; set; }

        //public string NotificationType { get; set; }

        [Required]
        public bool Seen { get; set; }
    }
}
