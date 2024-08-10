namespace SportsCenter.Core.Entities;

public partial class Produkt
{
    public int ProduktId { get; set; }

    public string Nazwa { get; set; } = null!;

    //public string Producent { get; set; } = null!;

    public int Ilosc { get; set; }

    //public int Koszt { get; set; }

    //public byte[] Zdjecie { get; set; } = null!;

    public virtual ICollection<ZamowienieProdukt> ZamowienieProdukts { get; set; } = new List<ZamowienieProdukt>();
}