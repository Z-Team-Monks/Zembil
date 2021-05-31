﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zembil.ValidationAttributes;

namespace Zembil.Views
{
    [ValidReview]
    public class ReviewDto
    {
        public int Rating { get; set; }
        public string Comment { get; set; }

    }
}
