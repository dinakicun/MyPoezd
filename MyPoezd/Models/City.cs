using System;
using System.Collections.Generic;

namespace MyPoezd.Models;

public partial class City
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Route> RouteArrivalCities { get; set; } = new List<Route>();

    public virtual ICollection<Route> RouteDepartureCities { get; set; } = new List<Route>();
}
