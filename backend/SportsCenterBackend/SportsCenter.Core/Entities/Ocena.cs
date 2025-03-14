using System;
using System.Collections.Generic;

namespace SportsCenter.Core.Entities;

public partial class Ocena
{
    public int OcenaId { get; set; }

    public string Opis { get; set; } = null!;

    public int Gwiazdki { get; set; }

    public int? GrafikZajecKlientId { get; set; }

    public DateOnly DataWystawienia { get; set; }

    public virtual InstancjaZajecKlient InstancjaZajecKlient { get; set; } = null!;
}
