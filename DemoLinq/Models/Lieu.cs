namespace DemoLinq.Models
{
    public class Lieu
    {
        public int Id { get; set; }
        public string? Nom { get; set; }
        public string? Description { get; set; }
        public string? Type {  get; set; }
        public long? Latitude { get; set; }
        public long? Longitude { get; set; }
        public long? Superficie { get; set; }
    }
}
