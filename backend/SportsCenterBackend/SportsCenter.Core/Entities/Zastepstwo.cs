using System;
using System.Collections.Generic;

namespace SportsCenter.Core.Entities;

public partial class Zastepstwo
{
    public int ZastepstwoId { get; set; }

    public DateOnly Data { get; set; }

    public TimeOnly GodzinaOd { get; set; }

    public TimeOnly GodzinaDo { get; set; }
    public int? ZajeciaId { get; set; }
    public int? RezerwacjaId { get; set; }

    public int PracownikNieobecnyId { get; set; }

    public int? PracownikZastepujacyId { get; set; }

    public int? PracownikZatwierdzajacyId { get; set; }

    public virtual Pracownik PracownikNieobecny { get; set; } = null!;

    public virtual Pracownik PracownikZastepujacy { get; set; } = null!;

    public virtual Pracownik PracownikZatwierdzajacy { get; set; } = null!;
}
