using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zembil.Models
{
    [Table("WishList")]
    public class WishListItem
    {
        [Key]
        public int WishListItemId { get; set; }

        [ForeignKey("ProductId")]
        public int ProductId { get; set; }

        [ForeignKey("UserId")]
        public int UserId { get; set; }

        [Required]
        public DateTime DateAdded { get; set; }
    }
}
