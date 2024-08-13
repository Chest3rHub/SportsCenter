namespace SportsCenter.Core.Entities;

public partial class Zapis
{
    public int ZapisId { get; set; }

    public int ZajeciaWGrafikuId { get; set; }

    public int KlientId { get; set; }

    public virtual Klient Klient { get; set; } = null!;

    public virtual ZajeciaWGrafiku ZajeciaWGrafiku { get; set; } = null!;
}