using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Zembil.Models
{
    [Table("reviews")]
    public class Review
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReviewId { get; set; }

        [Key, Column(Order = 1)]
        [ForeignKey("UserId")]
        public int UserId { get; set; }

        [Key, Column(Order = 2)]
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
