using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Zembil.Models
{
    [Table("shopFollow")]
    public class ShopFollow
    {
        [Key]
        public int ShopLikeId { get; set; }

        [ForeignKey("UserId")]
        public int UserId { get; set; }

        [ForeignKey("ShopId")]
        public int ShopId { get; set; }
    }
}
