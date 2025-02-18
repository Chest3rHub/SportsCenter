using System;
using System.Collections.Generic;

namespace SportsCenter.Core.Entities;

public partial class Rezerwacja
{
    public int RezerwacjaId { get; set; }

    public int KlientId { get; set; }

    public int KortId { get; set; }

    public DateTime DataOd { get; set; }

    public DateTime DataDo { get; set; }

    public DateOnly DataStworzenia { get; set; }

    public int? TrenerId { get; set; }

    public bool CzyUwzglednicSprzet { get; set; }

    public decimal Koszt { get; set; }

    public virtual Klient Klient { get; set; } = null!;

    public virtual Kort Kort { get; set; } = null!;

    public virtual Pracownik? Trener { get; set; }
}
