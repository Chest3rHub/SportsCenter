using System;
using System.Collections.Generic;

namespace SportsCenter.Core.Entities;

public partial class Pracownik
{
    public int PracownikId { get; set; }

    public int IdTypPracownika { get; set; }

    public DateOnly DataZatrudnienia { get; set; }

    public DateTime? DataZwolnienia { get; set; }

    public virtual ICollection<GrafikZajec> GrafikZajecs { get; set; } = new List<GrafikZajec>();

    public virtual TypPracownika IdTypPracownikaNavigation { get; set; } = null!;

    public virtual Osoba PracownikNavigation { get; set; } = null!;

    public virtual ICollection<Rezerwacja> Rezerwacjas { get; set; } = new List<Rezerwacja>();

    public virtual ICollection<TrenerCertifikat> TrenerCertifikats { get; set; } = new List<TrenerCertifikat>();

    public virtual ICollection<Zadanie> ZadaniePracownikZlecajacies { get; set; } = new List<Zadanie>();

    public virtual ICollection<Zadanie> ZadaniePracowniks { get; set; } = new List<Zadanie>();

    public virtual ICollection<Zamowienie> Zamowienies { get; set; } = new List<Zamowienie>();
}
