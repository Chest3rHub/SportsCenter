namespace SportsCenter.Core.Entities;

public partial class Kort
{
    public int KortId { get; set; }

    public string Nazwa { get; set; } = null!;

    public virtual ICollection<SportActivitySchedule> GrafikZajecs { get; set; } = new List<SportActivitySchedule>();

    public virtual ICollection<Rezerwacja> Rezerwacjas { get; set; } = new List<Rezerwacja>();
}
