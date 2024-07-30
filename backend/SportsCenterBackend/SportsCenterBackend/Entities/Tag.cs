using System;
using System.Collections.Generic;

namespace SportsCenterBackend.Entities;

public partial class Tag
{
    public int TagId { get; set; }

    public string Nazwa { get; set; } = null!;

    public virtual ICollection<KlientTag> KlientTags { get; set; } = new List<KlientTag>();
}
