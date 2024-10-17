namespace SportsCenter.Core.Entities;

public partial class Zajecium
{
    public int ZajeciaId { get; set; }

    public string Nazwa { get; set; } = null!;

    public int IdPoziomZajec { get; set; }

    public virtual ICollection<GrafikZajec> GrafikZajecs { get; set; } = new List<GrafikZajec>();

    public virtual PoziomZajec IdPoziomZajecNavigation { get; set; } = null!;
}
