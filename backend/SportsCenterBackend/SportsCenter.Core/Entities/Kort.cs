using System;
using System.Collections.Generic;

namespace SportsCenter.Core.Entities;

public partial class Kort
{
    public int KortId { get; set; }

    public string Nazwa { get; set; } = null!;

    public virtual ICollection<GrafikZajec> GrafikZajecs { get; set; } = new List<GrafikZajec>();

    public virtual ICollection<Rezerwacja> Rezerwacjas { get; set; } = new List<Rezerwacja>();
}
