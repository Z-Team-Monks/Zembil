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

        [Required(ErrorMessage = "Shop needs to have a name")]
        public string ShopName { get; set; }

        [MaxLength(150)]
        public string BuildingName { get; set; }

        [Required]
        [MaxLength(50)]
        public string PhoneNumber1 { get; set; }

        public string CoverImage { get; set; }

        [Required]
        [ForeignKey("UserId")]
        public int OwnerId { get; set; }

        [ForeignKey("CategoryId")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "The associated location id is required")]
        [ForeignKey("LocationId")]
        public int ShopLocationId { get; set; }
        public Location ShopLocation { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }
        public bool? IsActive { get; set; }

    }
}
