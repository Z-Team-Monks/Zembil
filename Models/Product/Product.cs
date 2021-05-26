using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zembil.Models
{
    [Table("products")]
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey(nameof(Shop))]
        public int ShopId { get; set; }

        [Required]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "0:yyy-MM-dd", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        //Why is this here?
        [Display(Name = "Builing Name")]
        public string BuilingName { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        [Required]
        public int Price { get; set; }

        public string Condition { get; set; }

        public string ImageUrl { get; set; }

        [Display(Name = "Delivery Available")]
        public bool DeliveryAvailable { get; set; }

        public int Discount { get; set; }

        [Display(Name = "Product Count")]
        public int ProductCount { get; set; }

    }
}