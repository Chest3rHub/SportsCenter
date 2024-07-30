using System;
using System.Collections.Generic;

namespace SportsCenterBackend.Entities;

public partial class Zamowienie
{
    public int ZamowienieId { get; set; }

    public int KlientId { get; set; }

    public virtual Klient Klient { get; set; } = null!;

    public virtual ICollection<ZamowienieProdukt> ZamowienieProdukts { get; set; } = new List<ZamowienieProdukt>();
}
