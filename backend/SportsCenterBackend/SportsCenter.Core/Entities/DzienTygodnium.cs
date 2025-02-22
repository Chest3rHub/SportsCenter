using System;
using System.Collections.Generic;

namespace SportsCenter.Core.Entities;

public partial class DzienTygodnium
{
    public int DzienTygodniaId { get; set; }

    public string Nazwa { get; set; } = null!;

    public virtual GodzinyPracyKlubu GodzinyPracyKlubu { get; set; }
}
