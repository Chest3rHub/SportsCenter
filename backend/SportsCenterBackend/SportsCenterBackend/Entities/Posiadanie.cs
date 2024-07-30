using System;
using System.Collections.Generic;

namespace SportsCenterBackend.Entities;

public partial class Posiadanie
{
    public int PosiadanieId { get; set; }

    public int TrenerId { get; set; }

    public int CertyfikatId { get; set; }

    public DateTime DataOtrzymania { get; set; }

    public virtual Certyfikat Certyfikat { get; set; } = null!;

    public virtual Trener Trener { get; set; } = null!;
}
