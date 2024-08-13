using System.Collections.Generic;

namespace SportsCenter.Core.Entities;

public partial class PracownikAdministracyjny
{
    public int PracownikAdmId { get; set; }

    public virtual Osoba PracownikAdm { get; set; } = null!;

    public virtual ICollection<Todo> Todos { get; set; } = new List<Todo>();
}