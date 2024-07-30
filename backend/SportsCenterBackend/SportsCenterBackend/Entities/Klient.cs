using System;
using System.Collections.Generic;

namespace SportsCenterBackend.Entities;

public partial class Klient
{
    public int KlientId { get; set; }

    public int Saldo { get; set; }

    public int? ZnizkaZajecia { get; set; }

    public int? ZnizkaProdukty { get; set; }

    public virtual Osoba KlientNavigation { get; set; } = null!;

    public virtual ICollection<KlientTag> KlientTags { get; set; } = new List<KlientTag>();

    public virtual ICollection<Ocena> Ocenas { get; set; } = new List<Ocena>();

    public virtual ICollection<Rezerwacja> Rezerwacjas { get; set; } = new List<Rezerwacja>();

    public virtual ICollection<Zamowienie> Zamowienies { get; set; } = new List<Zamowienie>();

    public virtual ICollection<Zapis> Zapis { get; set; } = new List<Zapis>();
}
