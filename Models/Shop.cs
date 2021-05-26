using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Zembil.Models
{
    [Table("shops")]
    public class Shop
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ShopId { get; set; }

        [MaxLength(150)]
        public string BuildingName { get; set; }

        [Required]
        [MaxLength(50)]
        public string PhoneNumber1 { get; set; }

        [MaxLength(50)]
        public string PhoneNumber2 { get; set; }

        [Required]
        [ForeignKey("UserId")]
        public int OwnerId { get; set; }

        [ForeignKey("CategoryId")]
        public int CategoryId { get; set; }

        [ForeignKey("LocationId")]
        public int LocationId { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

    }
}
