using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Geometries;

namespace Zembil.Models
{
    [Table("location")]
    public class ShopLocation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LocationId { get; set; }

        [MaxLength(500)]
        public string LocationName { get; set; }

        [Required]
        [Column(TypeName = "geometry")]
        public Point GeoLoacation { get; set; }
    }
}
