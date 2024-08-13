namespace SportsCenter.Core.Entities;

public partial class ZajeciaWGrafiku
{
    public int ZajeciaWGrafikuId { get; set; }

    public int ZajeciaId { get; set; }

    public int TrenerId { get; set; }

    public int KortId { get; set; }

    public string NazwaGrupy { get; set; } = null!;

    public virtual ICollection<Grafik> Grafiks { get; set; } = new List<Grafik>();

    public virtual Kort Kort { get; set; } = null!;

    public virtual ICollection<Ocena> Ocenas { get; set; } = new List<Ocena>();

    public virtual Trener Trener { get; set; } = null!;

    public virtual Zajecium Zajecia { get; set; } = null!;

    public virtual ICollection<Zapis> Zapis { get; set; } = new List<Zapis>();
}