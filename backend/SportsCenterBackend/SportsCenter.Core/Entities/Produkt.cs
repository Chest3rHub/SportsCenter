using System;
using System.Collections.Generic;

namespace SportsCenter.Core.Entities;

public partial class Produkt
{
    public int ProduktId { get; set; }

    public string Nazwa { get; set; } = null!;

    public string Producent { get; set; } = null!;

    public int LiczbaNaStanie { get; set; }

    public decimal Koszt { get; set; }

    public string ZdjecieUrl { get; set; } = null!;

    public virtual ICollection<ZamowienieProdukt> ZamowienieProdukts { get; set; } = new List<ZamowienieProdukt>();
}
