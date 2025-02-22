using System;
using System.Collections.Generic;

namespace SportsCenter.Core.Entities;

public partial class GodzinyPracyKlubu
{
    public int GodzinyPracyKlubuId { get; set; }

    public TimeSpan GodzinaOtwarcia { get; set; }

    public TimeSpan GodzinaZamkniecia { get; set; }
    public int DzienTygodniaId { get; set; }
    public virtual DzienTygodnium DzienTygodnia { get; set; }
}
