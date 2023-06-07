using System;
using System.Collections.Generic;

namespace MyPoezd.Models;

public partial class User
{
    public int Id { get; set; }

    public string? Surname { get; set; }

    public string Name { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string? NumberPhone { get; set; }

    public string? PassportData { get; set; }

    public string Password { get; set; } = null!;

    public int RoleId { get; set; }

    public virtual ICollection<Place> Places { get; set; } = new List<Place>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
