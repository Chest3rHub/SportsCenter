using System;

namespace SportsCenter.Core.Entities;

public partial class Grafik
{
    public int GrafikId { get; set; }

    public int ZajeciaWGrafikuId { get; set; }

    public int RezerwacjaId { get; set; }

    public DateTime Data { get; set; }

    public TimeSpan GodzinaOd { get; set; }

    public int CzasTrwania { get; set; }

    public virtual Rezerwacja Rezerwacja { get; set; } = null!;

    public virtual ZajeciaWGrafiku ZajeciaWGrafiku { get; set; } = null!;
}