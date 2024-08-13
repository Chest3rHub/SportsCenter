namespace SportsCenter.Core.Entities;

public partial class ZamowienieProdukt
{
    public int ZamowienieProduktId { get; set; }

    public int ZamowienieId { get; set; }

    public int ProduktId { get; set; }

    public int Koszt { get; set; }

    public DateTime DataZamowienia { get; set; }

    public virtual Produkt Produkt { get; set; } = null!;

    public virtual Zamowienie Zamowienie { get; set; } = null!;
}