using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Zembil.Views
{
    public class ShopStatusDto
    {
        [Required]
        public bool IsActive { get; set; }
    }
}
