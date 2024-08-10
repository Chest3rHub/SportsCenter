using System.Collections.Generic;

namespace SportsCenter.Core.Entities;

public partial class Rezerwacja
{
    public int RezerwacjaId { get; set; }

    public int KlientId { get; set; }

    public int? KortId { get; set; }

    public int? TrenerId { get; set; }

    public int? ZajeciaZajeciaId { get; set; }

    public virtual ICollection<Grafik> Grafiks { get; set; } = new List<Grafik>();

    public virtual Klient Klient { get; set; } = null!;

    public virtual Kort? Kort { get; set; }

    public virtual Trener? Trener { get; set; }

    public virtual Zajecium? ZajeciaZajecia { get; set; }
}