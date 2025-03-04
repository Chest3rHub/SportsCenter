using System;
using System.Collections.Generic;

namespace SportsCenter.Core.Entities;

public partial class TrenerCertyfikat
{
    public int PracownikId { get; set; }

    public int CertyfikatId { get; set; }

    public DateOnly DataOtrzymania { get; set; }

    public virtual Certyfikat Certyfikat { get; set; } = null!;

    public virtual Pracownik Pracownik { get; set; } = null!;
}
