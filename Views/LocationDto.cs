using System.ComponentModel.DataAnnotations.Schema;
using Zembil.Models;

namespace Zembil.Views
{
    public class LocationDto
    {
        [NotMapped]
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}