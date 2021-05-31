using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zembil.Views
{
    public class ShopBatchGetDto
    {
        public int ShopId { get; set; }        
        public string ShopName { get; set; }        
        public string BuildingName { get; set; }
        public string PhoneNumber1 { get; set; }
        public string CoverImage { get; set; }        
        public int OwnerId { get; set; }        
        public int CategoryId { get; set; }        
        public int ShopLocationId { get; set; }        
        public string Description { get; set; }
        public string Status { get; set; }
    }
}
