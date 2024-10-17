﻿namespace SportsCenter.Core.Entities;

public partial class Certyfikat
{
    public int CertyfikatId { get; set; }

    public string Nazwa { get; set; } = null!;

    public virtual ICollection<TrenerCertifikat> TrenerCertifikats { get; set; } = new List<TrenerCertifikat>();
}
