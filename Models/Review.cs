using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zembil.Models
{
    [Table("review")]
    public class Review
    {
        [Required]
        public int ReviewId { get; set; }

        [ForeignKey("UserId")]
        public int UserId { get; set; }

        [ForeignKey("ProductId")]
        public int ProductId { get; set; }

        [Required]
        public int Rating { get; set; }

        [Required]
        [MaxLength(500)]
        public string ReviewString { get; set; }

        [Required]
        public DateTime ReviewDate { get; set; }
    }
}
