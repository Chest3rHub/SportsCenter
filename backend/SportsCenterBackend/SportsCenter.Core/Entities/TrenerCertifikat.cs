namespace SportsCenter.Core.Entities;

public partial class TrenerCertifikat
{
    public int PracownikId { get; set; }

    public int CertyfikatId { get; set; }

    public DateOnly DataOtrzymania { get; set; }

    public virtual Certyfikat Certyfikat { get; set; } = null!;

    public virtual Pracownik Pracownik { get; set; } = null!;
}
