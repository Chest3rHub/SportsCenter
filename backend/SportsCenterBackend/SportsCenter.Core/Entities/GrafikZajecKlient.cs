namespace SportsCenter.Core.Entities;

public partial class GrafikZajecKlient
{
    public int GrafikZajecKlientId { get; set; }

    public int GrafikZajecId { get; set; }

    public int KlientId { get; set; }

    public DateOnly DataZapisu { get; set; }

    public DateOnly? DataWypisu { get; set; }

    public bool CzyUwzglednicSprzet { get; set; }

    public virtual GrafikZajec GrafikZajec { get; set; } = null!;

    public virtual Klient Klient { get; set; } = null!;

    public virtual ICollection<Ocena> Ocenas { get; set; } = new List<Ocena>();
}
