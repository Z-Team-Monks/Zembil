using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zembil.Views
{
    public class ShopChangeDto
    {        
        public string BuildingName { get; set; }
        public string PhoneNumber1 { get; set; }
        public string PhoneNumber2 { get; set; }        
        public int CategoryId { get; set; }
        public int LocationId { get; set; }
        public string Description { get; set; }        
    }
}
