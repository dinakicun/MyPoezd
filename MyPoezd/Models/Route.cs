using System;
using System.Collections.Generic;

namespace MyPoezd.Models;

public partial class Route
{
    public int Id { get; set; }

    public DateTime DepartureDate { get; set; }

    public int DepartureCityId { get; set; }

    public int ArrivalCityId { get; set; }

    public DateTime ArrivalDate { get; set; }

    public int TrainsId { get; set; }

    public decimal PriceCoupe { get; set; }

    public decimal PriceEconom { get; set; }

    public virtual City ArrivalCity { get; set; } = null!;

    public virtual City DepartureCity { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual Train Trains { get; set; } = null!;
}
