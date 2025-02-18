using System;
using System.Collections.Generic;

namespace SportsCenter.Core.Entities;

public partial class DataZajec
{
    public int DataZajecId { get; set; }

    public DateTime Date { get; set; }

    public int GrafikZajecId { get; set; }

    public virtual GrafikZajec GrafikZajec { get; set; } = null!;
}
