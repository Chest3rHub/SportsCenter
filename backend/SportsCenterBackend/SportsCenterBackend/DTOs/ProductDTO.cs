namespace SportsCenterBackend.DTOs;

public class ProductDTO
{
    public int ProduktId { get; set; }

    public string Nazwa { get; set; } = null!;

    public string Producent { get; set; } = null!;

    public int Ilosc { get; set; }

    public int Koszt { get; set; }

    public byte[] Zdjecie { get; set; } = null!;
}