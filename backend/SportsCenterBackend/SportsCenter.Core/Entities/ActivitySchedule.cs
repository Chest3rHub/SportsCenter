namespace SportsCenter.Core.Entities;

public partial class SportActivitySchedule
{
    public int GrafikZajecId { get; set; }

    public int CzasTrwania { get; set; }

    public int ZajeciaId { get; set; }

    public int PracownikId { get; set; }

    public int LimitOsob { get; set; }

    public int KortId { get; set; }

    public decimal KoszBezSprzetu { get; set; }

    public decimal KoszZeSprzetem { get; set; }

    public virtual ICollection<DataZajec> DataZajecs { get; set; } = new List<DataZajec>();

    public virtual ICollection<GrafikZajecKlient> GrafikZajecKlients { get; set; } = new List<GrafikZajecKlient>();

    public virtual Kort Kort { get; set; } = null!;

    public virtual Pracownik Pracownik { get; set; } = null!;

    public virtual SportActivity Zajecia { get; set; } = null!;
}
