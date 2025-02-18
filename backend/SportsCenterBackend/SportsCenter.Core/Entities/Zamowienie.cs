using System;
using System.Collections.Generic;

namespace SportsCenter.Core.Entities;

public partial class Zamowienie
{
    public int ZamowienieId { get; set; }

    public int KlientId { get; set; }

    public DateOnly Data { get; set; }

    public DateOnly? DataOdbioru { get; set; }

    public DateOnly? DataRealizacji { get; set; }

    public int PracownikId { get; set; }

    public string Status { get; set; } = null!;

    public virtual Klient Klient { get; set; } = null!;

    public virtual Pracownik Pracownik { get; set; } = null!;

    public virtual ICollection<ZamowienieProdukt> ZamowienieProdukts { get; set; } = new List<ZamowienieProdukt>();
}
