using System;
using System.Collections.Generic;

namespace MyPoezd.Models;

public partial class Type
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Wagon> Wagons { get; set; } = new List<Wagon>();
}
