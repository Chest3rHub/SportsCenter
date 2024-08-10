namespace SportsCenter.Core.Entities;

public partial class Ocena
{
    public int OcenaId { get; set; }

    public int KlientKlientId { get; set; }

    public int? TrenerTrenerId { get; set; }

    public int? ZajeciaWGrafikuZajeciaWGrafikuId { get; set; }

    public string Opis { get; set; } = null!;

    public int Gwiazdki { get; set; }

    public virtual Klient KlientKlient { get; set; } = null!;

    public virtual Trener? TrenerTrener { get; set; }

    public virtual ZajeciaWGrafiku? ZajeciaWGrafikuZajeciaWGrafiku { get; set; }
}