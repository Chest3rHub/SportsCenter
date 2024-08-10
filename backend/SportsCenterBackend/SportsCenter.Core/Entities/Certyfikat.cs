using System.Collections.Generic;

namespace SportsCenter.Core.Entities;

public partial class Certyfikat
{
    public int CertyfikatId { get; set; }

    public string Nazwa { get; set; } = null!;

    public virtual ICollection<Posiadanie> Posiadanies { get; set; } = new List<Posiadanie>();
}