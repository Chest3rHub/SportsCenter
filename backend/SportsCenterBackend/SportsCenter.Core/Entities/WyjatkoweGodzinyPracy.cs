using System;
using System.Collections.Generic;

namespace SportsCenter.Core.Entities;

public partial class WyjatkoweGodzinyPracy
{
    public int WyjatkoweGodzinyPracyId { get; set; }

    public DateOnly Data { get; set; }

    public TimeOnly GodzinaOtwarcia { get; set; }

    public TimeOnly GodzinaZamkniecia { get; set; }
}
