using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zembil.Views
{
    public class ShopChangeDto
    {
        public string ShopName { get; set; }
        public string BuildingName { get; set; }
        public string PhoneNumber { get; set; }
        public string CoverImage { get; set; } //reconsider this
        public int CategoryId { get; set; }
        public NewLocationDto ShopLocation { get; set; }
        public string Description { get; set; }
    }
}
