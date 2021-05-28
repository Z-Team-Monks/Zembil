﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zembil.Models
{
    [Table("category")]
    public class Category
    {
        [Required]
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Category_name { get; set; }
    }
}
