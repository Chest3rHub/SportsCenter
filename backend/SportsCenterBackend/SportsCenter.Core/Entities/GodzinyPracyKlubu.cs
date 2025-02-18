using System;
using System.Collections.Generic;

namespace SportsCenter.Core.Entities;

public partial class GodzinyPracyKlubu
{
    public int GodzinyPracyKlubuId { get; set; }

    public TimeOnly GodzinaOtwarcia { get; set; }

    public TimeOnly GodzinaZamkniecia { get; set; }

    public int DzienTygodniaId { get; set; }
}
