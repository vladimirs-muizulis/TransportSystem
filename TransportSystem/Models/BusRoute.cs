using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransportSystem.Models
{
    public class BusRoute
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public List<BusStop> Stops { get; set; } = new List<BusStop>();

        [ForeignKey("AssignedBus")]
        public int? AssignedBusId { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public Transport? AssignedBus { get; set; }
    }
}