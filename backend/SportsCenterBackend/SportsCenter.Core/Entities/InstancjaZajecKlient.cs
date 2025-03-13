using System;
using System.Collections.Generic;

namespace SportsCenter.Core.Entities;

public partial class InstancjaZajecKlient
{
    public int InstancjaZajecKlientId { get; set; }

    public int InstancjaZajecId { get; set; }

    public int KlientId { get; set; }

    public DateOnly DataZapisu { get; set; }

    public DateOnly? DataWypisu { get; set; }

    public bool CzyUwzglednicSprzet { get; set; }
    public bool? CzyOplacone {  get; set; }

    public virtual InstancjaZajec InstancjaZajec { get; set; } = null!;

    public virtual Klient Klient { get; set; } = null!;

    public virtual ICollection<Ocena> Ocenas { get; set; } = new List<Ocena>();
}
