using System;
using System.Collections.Generic;

namespace MyPoezd.Models;

public partial class Ticket
{
    public int Id { get; set; }

    public int RouteId { get; set; }

    public int WagonId { get; set; }

    public int PlaceId { get; set; }

    public int UserId { get; set; }

    public virtual Place Place { get; set; } = null!;

    public virtual Route Route { get; set; } = null!;

    public virtual User User { get; set; } = null!;

    public virtual Wagon Wagon { get; set; } = null!;
}
