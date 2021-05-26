using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Zembil.Models
{
    public class Category
    {
        [Required]
        public int id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Category_name { get; set; }
    }
}
