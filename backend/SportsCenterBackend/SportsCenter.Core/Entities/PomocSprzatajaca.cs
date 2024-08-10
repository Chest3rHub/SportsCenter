namespace SportsCenter.Core.Entities;

public partial class PomocSprzatajaca
{
    public int PomocSprzatajacaId { get; set; }

    public virtual Osoba PomocSprzatajacaNavigation { get; set; } = null!;

    public virtual ICollection<Todo> Todos { get; set; } = new List<Todo>();
}