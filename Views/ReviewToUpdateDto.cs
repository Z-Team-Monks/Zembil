using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zembil.Views
{
    public class ReviewToUpdateDto
    {        
        public int Rating { get; set; }
        public string ReviewString { get; set; }
        public DateTime ReviewDate { get; set; }

    }
}
