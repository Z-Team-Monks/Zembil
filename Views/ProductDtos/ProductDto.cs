using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zembil.Models;

namespace Zembil.Views
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public int ShopId { get; set; }
        public string ProductName { get; set; }
        public DateTime DateInserted { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }
        //database categories validator
        public int CategoryId { get; set; }
        public int Price { get; set; }
        //used or new validator
        public string Condition { get; set; }
        public string ImageUrl { get; set; }
        public bool DeliveryAvailable { get; set; }
        public int ProductCount { get; set; }
        public IEnumerable<ReviewToReturnDto> ProductReviews { get; set; }

    }
}
