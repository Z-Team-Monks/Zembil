using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zembil.Views
{
    public class WishListDto
    {
        public int WishListItemId { get; set; }
        public int ProductId { get; set; }
        public DateTime DateAdded { get; set; }
        public ProductGetBatchDto Product { get; set; }                
    }
}
