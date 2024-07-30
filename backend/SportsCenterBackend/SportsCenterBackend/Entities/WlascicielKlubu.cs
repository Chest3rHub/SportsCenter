using System;
using System.Collections.Generic;

namespace SportsCenterBackend.Entities;

public partial class WlascicielKlubu
{
    public int WlascicielKlubuId { get; set; }

    public virtual ICollection<Todo> Todos { get; set; } = new List<Todo>();

    public virtual Osoba WlascicielKlubuNavigation { get; set; } = null!;
}
