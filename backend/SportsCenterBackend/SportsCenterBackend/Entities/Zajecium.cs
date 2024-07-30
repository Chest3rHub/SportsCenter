using System;
using System.Collections.Generic;

namespace SportsCenterBackend.Entities;

public partial class Zajecium
{
    public int ZajeciaId { get; set; }

    public string Nazwa { get; set; } = null!;

    public string Poziom { get; set; } = null!;

    public bool CzyRezerwacjaPrywatna { get; set; }

    public virtual ICollection<Rezerwacja> Rezerwacjas { get; set; } = new List<Rezerwacja>();

    public virtual ICollection<ZajeciaWGrafiku> ZajeciaWGrafikus { get; set; } = new List<ZajeciaWGrafiku>();
}
