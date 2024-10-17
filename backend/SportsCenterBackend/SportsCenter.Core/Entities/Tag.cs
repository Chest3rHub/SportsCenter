namespace SportsCenter.Core.Entities;

public partial class Tag
{
    public int TagId { get; set; }

    public string Nazwa { get; set; } = null!;

    public virtual ICollection<Klient> Klients { get; set; } = new List<Klient>();
}
