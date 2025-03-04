using System;
using System.Collections.Generic;

namespace SportsCenter.Core.Entities;

public partial class Osoba
{
    public int OsobaId { get; set; }

    public string Imie { get; set; } = null!;

    public string Nazwisko { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Haslo { get; set; } = null!;

    public DateOnly? DataUr { get; set; }

    public string NrTel { get; set; } = null!;

    public string? Pesel { get; set; }

    public string Adres { get; set; } = null!;

    public virtual Klient? Klient { get; set; }

    public virtual Pracownik? Pracownik { get; set; }
}
