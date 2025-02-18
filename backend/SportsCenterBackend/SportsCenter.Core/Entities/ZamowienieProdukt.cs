using System;
using System.Collections.Generic;

namespace SportsCenter.Core.Entities;

public partial class ZamowienieProdukt
{
    public int ZamowienieId { get; set; }

    public int ProduktId { get; set; }

    public int Liczba { get; set; }

    public decimal Koszt { get; set; }

    public virtual Produkt Produkt { get; set; } = null!;

    public virtual Zamowienie Zamowienie { get; set; } = null!;
}
