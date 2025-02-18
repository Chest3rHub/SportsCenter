using System;
using System.Collections.Generic;

namespace SportsCenter.Core.Entities;

public partial class Aktualnosci
{
    public int AktualnosciId { get; set; }

    public string Nazwa { get; set; } = null!;

    public string Opis { get; set; } = null!;

    public DateTime WazneOd { get; set; }

    public DateTime? WazneDo { get; set; }
}
