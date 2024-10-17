namespace SportsCenter.Core.Entities;

public partial class PoziomZajec
{
    public int IdPoziomZajec { get; set; }

    public string Nazwa { get; set; } = null!;

    public virtual ICollection<Zajecium> Zajecia { get; set; } = new List<Zajecium>();
}
