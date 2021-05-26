using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Zembil.Models
{
    public class Brand
    {
        [Key]
        public int BrandId { get; set; }

        [Required]
        [MaxLength(100)]
        public string BrandName { get; set; }
    }
}
