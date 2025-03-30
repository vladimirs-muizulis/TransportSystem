namespace TransportSystem.Models
{
    public class Route
    {
        public int Id { get; set; }
        public string StartLocation { get; set; }
        public string EndLocation { get; set; }
        public Transport Transport { get; set; }
    }
}
