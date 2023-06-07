using MyPoezd.Models;

namespace MyPoezd.Models.ViewModels
{
    public class RoutesView
    {
        public string? DepartureCity { get; set; }
        public string? ArrivalCity { get; set; }
        public string? DepartureTime { get; set; }
        public string? ArrivalDate { get; set; }
        public string? ArrivalTime { get; set; }
        public Route? Route { get; set; }
        public Train? Train { get; set; }
        public City? City { get; set; }
        public string TrainName { get; set; }
        public IEnumerable<Train>? Trains { get; set; }
        public IEnumerable<Route>? Routes { get; set; }
        public IEnumerable<City>? Cities { get; set; }
        public IEnumerable<Type>? TrainTypes { get; set; }
        public string? StringDeparutureDate { get; set; }
        public string? StringArrivalDate { get; set; }
        public string? TravelTime { get; set; }
        public User? User { get; set; }
    }
}
