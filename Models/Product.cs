using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zembil.Models
{
    [Table("products")]
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        [ForeignKey("ShopId")]
        public int ShopId { get; set; }

        [Required]
        public string ProductName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "0:yyy-MM-dd", ApplyFormatInEditMode = true)]
        public DateTime DateInserted { get; set; }

        public string Description { get; set; }

        [ForeignKey("CategoryId")]
        public int CategoryId { get; set; }

        [Required]
        public int Price { get; set; }

        public string Condition { get; set; }

        public string ImageUrl { get; set; }

        [Display(Name = "Delivery Available")]
        public bool DeliveryAvailable { get; set; }

        public int Discount { get; set; }

        [Display(Name = "Product Count")]
        public int ProductCount { get; set; }

        // add rating id related to the product
        [ForeignKey("ReviewId")]
        public int? ProductReviewId { get; set; }
        public List<Review> ProductReviews { get; set; }

    }
}