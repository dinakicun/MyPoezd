namespace MyPoezd.Models.ViewModels
{
    public class OrderView
    {
        public City? City { get; set; }
        public Route? Route { get; set; }
        public string DepartureCity { get; set; }
        public string ArrivalCity { get; set; }
        public int RouteId { get; set; }
        public Train? Train { get; set; }
        public int TrainId { get; set; }
        public Wagon? Wagon { get; set; }
        public int WagonId { get; set; }
        public string? WagonTypeName { get; set; }
        public string? TrainName { get; set; }
        public string? WagonName { get; set; }
        public int PlaceId { get; set; }
        public string? Price { get; set; }
        public string? PlaceName { get; set; }
        public Type? Type { get; set; }
        public int TicketId { get; set; }
    }
}
