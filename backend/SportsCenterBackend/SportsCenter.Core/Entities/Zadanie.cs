using System;
using System.Collections.Generic;

namespace SportsCenter.Core.Entities;

public partial class Zadanie
{
    public int ZadanieId { get; set; }

    public string Opis { get; set; } = null!;

    public DateOnly? DataDo { get; set; }

    public int PracownikId { get; set; }

    public int PracownikZlecajacyId { get; set; }

    public virtual Pracownik Pracownik { get; set; } = null!;

    public virtual Pracownik PracownikZlecajacy { get; set; } = null!;
}
