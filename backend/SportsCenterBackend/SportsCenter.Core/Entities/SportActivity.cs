namespace SportsCenter.Core.Entities;

public partial class SportActivity
{
    public int ZajeciaId { get; set; }

    public string Nazwa { get; set; } = null!;

    public int IdPoziomZajec { get; set; }

    public virtual ICollection<SportActivitySchedule> GrafikZajecs { get; set; } = new List<SportActivitySchedule>();

    public virtual PoziomZajec IdPoziomZajecNavigation { get; set; } = null!;
}
