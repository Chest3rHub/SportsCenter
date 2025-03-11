using System;
using System.Collections.Generic;

namespace SportsCenter.Core.Entities;

public partial class Klient
{
    public int KlientId { get; set; }

    public decimal Saldo { get; set; }

    public int? ZnizkaNaZajecia { get; set; }

    public int? ZnizkaNaProdukty { get; set; }

    public virtual ICollection<InstancjaZajecKlient> InstancjaZajecKlients { get; set; } = new List<InstancjaZajecKlient>();

    public virtual Osoba KlientNavigation { get; set; } = null!;

    public virtual ICollection<Rezerwacja> Rezerwacjas { get; set; } = new List<Rezerwacja>();

    public virtual ICollection<Zamowienie> Zamowienies { get; set; } = new List<Zamowienie>();

    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
