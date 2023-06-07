using System;
using System.Collections.Generic;

namespace MyPoezd.Models;

public partial class Wagon
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Count { get; set; } = null!;

    public int TrainsId { get; set; }

    public int TypeId { get; set; }

    public virtual ICollection<Place> Places { get; set; } = new List<Place>();

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual Train Trains { get; set; } = null!;

    public virtual Type Type { get; set; } = null!;
}
