using System;
using System.Collections.Generic;

namespace SportsCenter.Core.Entities;

public partial class BrakDostepnosci
{
    public int BrakDostepnosciId { get; set; }

    public DateOnly Data { get; set; }

    public TimeOnly GodzinaOd { get; set; }

    public TimeOnly GodzinaDo { get; set; }

    public int PracownikId { get; set; }
    public bool CzyZatwierdzone { get; set; }
    public virtual Pracownik Pracownik { get; set; } = null!;
}
