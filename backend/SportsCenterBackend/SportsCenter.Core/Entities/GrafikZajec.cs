using System;
using System.Collections.Generic;

namespace SportsCenter.Core.Entities;

public partial class GrafikZajec
{
    public int GrafikZajecId { get; set; }

    public string DzienTygodnia { get; set; }

    public TimeSpan GodzinaOd {  get; set; }

    public int CzasTrwania { get; set; }

    public int ZajeciaId { get; set; }

    public int PracownikId { get; set; }

    public int LimitOsob { get; set; }

    public int KortId { get; set; }

    public decimal KosztBezSprzetu { get; set; }

    public decimal KosztZeSprzetem { get; set; }

    public virtual ICollection<GrafikZajecKlient> GrafikZajecKlients { get; set; } = new List<GrafikZajecKlient>();

    public virtual Kort Kort { get; set; } = null!;

    public virtual Pracownik Pracownik { get; set; } = null!;

    public virtual Zajecium Zajecia { get; set; } = null!;
}
