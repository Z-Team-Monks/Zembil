using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zembil.Models
{
    public class Location
    {
        [Key]
        public int LocationId { get; set; }

        [Required]
        public int Longitude { get; set; }

        [Required]
        public int Latitude { get; set; }

        [MaxLength(500)]
        public string LocationDescription { get; set; }
    }
}

