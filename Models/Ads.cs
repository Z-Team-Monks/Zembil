using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zembil.Models
{
    [Table("ads")]
    public class Ads
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AdsId { get; set; }

        [Required]
        public string Description { get; set; }

        [ForeignKey("ShopId")]
        public int ShopId { get; set; }
        public Shop AdsShop { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "0:yyy-MM-dd", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "0:yyy-MM-dd", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; }
    }
}
