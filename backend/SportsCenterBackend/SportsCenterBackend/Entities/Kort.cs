using System;
using System.Collections.Generic;

namespace SportsCenterBackend.Entities;

public partial class Kort
{
    public int KortId { get; set; }

    public string Nazwa { get; set; } = null!;

    public virtual ICollection<Rezerwacja> Rezerwacjas { get; set; } = new List<Rezerwacja>();

    public virtual ICollection<ZajeciaWGrafiku> ZajeciaWGrafikus { get; set; } = new List<ZajeciaWGrafiku>();
}
