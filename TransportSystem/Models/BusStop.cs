using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransportSystem.Models
{
    public class BusStop
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public TimeSpan ArrivalTime { get; set; }

        [Required]
        public TimeSpan DepartureTime { get; set; }

        [Required]
        public int Order { get; set; }  // Stop order in the route

        [ForeignKey("BusRoute")]
        public int BusRouteId { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public BusRoute? BusRoute { get; set; }
    }
}
