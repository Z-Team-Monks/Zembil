using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zembil.ValidationAttributes;

namespace Zembil.Views
{
    [ValidReview]
    public class ReviewDto
    {
        public int UserId { get; set; }

        public int ProductId { get; set; }

        public int Rating { get; set; }

        public string ReviewString { get; set; }

        public DateTime ReviewDate { get; set; }
    }
}
