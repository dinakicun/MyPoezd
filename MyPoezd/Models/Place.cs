using System;
using System.Collections.Generic;

namespace MyPoezd.Models;

public partial class Place
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? UserId { get; set; }

    public int WagonId { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual User? User { get; set; }

    public virtual Wagon Wagon { get; set; } = null!;
}
