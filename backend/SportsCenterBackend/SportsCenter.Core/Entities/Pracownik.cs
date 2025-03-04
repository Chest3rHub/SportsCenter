using System;
using System.Collections.Generic;

namespace SportsCenter.Core.Entities;

public partial class Pracownik
{
    public int PracownikId { get; set; }

    public int IdTypPracownika { get; set; }

    public DateOnly DataZatrudnienia { get; set; }

    public DateOnly? DataZwolnienia { get; set; }

    public virtual ICollection<BrakDostepnosci> BrakDostepnoscis { get; set; } = new List<BrakDostepnosci>();

    public virtual ICollection<GrafikZajec> GrafikZajecs { get; set; } = new List<GrafikZajec>();

    public virtual TypPracownika IdTypPracownikaNavigation { get; set; } = null!;

    public virtual Osoba PracownikNavigation { get; set; } = null!;

    public virtual ICollection<Rezerwacja> Rezerwacjas { get; set; } = new List<Rezerwacja>();

    public virtual ICollection<TrenerCertyfikat> TrenerCertyfikats { get; set; } = new List<TrenerCertyfikat>();

    public virtual ICollection<Zadanie> ZadaniePracownikZlecajacies { get; set; } = new List<Zadanie>();

    public virtual ICollection<Zadanie> ZadaniePracowniks { get; set; } = new List<Zadanie>();

    public virtual ICollection<Zamowienie> Zamowienies { get; set; } = new List<Zamowienie>();

    public virtual ICollection<Zastepstwo> ZastepstwoPracownikNieobecnies { get; set; } = new List<Zastepstwo>();

    public virtual ICollection<Zastepstwo> ZastepstwoPracownikZastepujacies { get; set; } = new List<Zastepstwo>();

    public virtual ICollection<Zastepstwo> ZastepstwoPracownikZatwierdzajacies { get; set; } = new List<Zastepstwo>();
}
