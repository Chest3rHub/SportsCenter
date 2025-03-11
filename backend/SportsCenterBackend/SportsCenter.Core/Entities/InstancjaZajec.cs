using System;
using System.Collections.Generic;

namespace SportsCenter.Core.Entities
{
    public partial class InstancjaZajec
    {
        public int InstancjaZajecId { get; set; }

        public int GrafikZajecId { get; set; }

        public DateOnly Data{ get; set; }

        public bool? CzyOdwolane { get; set; }

        public virtual GrafikZajec GrafikZajec { get; set; } = null!;

        public virtual ICollection<InstancjaZajecKlient> InstancjaZajecKlients { get; set; } = new List<InstancjaZajecKlient>();
    }
}
