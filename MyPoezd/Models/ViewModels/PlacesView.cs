
namespace MyPoezd.Models.ViewModels
{
    public class PlacesView
    {
        public Route? Route { get; set; }
        public Wagon? Wagon { get; set; }
        public Type? Type { get; set; }
        public Train? Train { get; set; }
        public Place? Place { get; set; }
        public string CountOfPlaces { get; set; }
        public string? WagonTypeName { get; set; }
        public string? Price { get; set; }
        public int TrainId { get; set; }
        public int RouteId { get; set; }
        public int WagonId { get; set; }
        public string TrainName { get; set; }
        public string WagonName { get; set; }
        public string PlaceName { get; set; }
        public string Status { get; set; }
        public IEnumerable<Wagon>? Wagons { get; set; }
        public IEnumerable<Train>? Trains { get; set; }
        public IEnumerable<Route>? Routes { get; set; }

    }
}
