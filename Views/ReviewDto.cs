using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zembil.Views
{
    public class ReviewDto
   {                
        public int UserId { get; set; }
        
        public int ProductId { get; set; }
        
        public int Rating { get; set; }

        public string ReviewString { get; set; }                
    }
}
