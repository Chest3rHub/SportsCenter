using System.Collections.Generic;

namespace SportsCenter.Core.Entities;

public partial class Trener
{
    public int TrenerId { get; set; }

    public virtual ICollection<Ocena> Ocenas { get; set; } = new List<Ocena>();

    public virtual ICollection<Posiadanie> Posiadanies { get; set; } = new List<Posiadanie>();

    public virtual ICollection<Rezerwacja> Rezerwacjas { get; set; } = new List<Rezerwacja>();

    public virtual ICollection<Todo> Todos { get; set; } = new List<Todo>();

    public virtual Osoba TrenerNavigation { get; set; } = null!;

    public virtual ICollection<ZajeciaWGrafiku> ZajeciaWGrafikus { get; set; } = new List<ZajeciaWGrafiku>();
}