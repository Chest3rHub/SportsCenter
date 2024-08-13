namespace SportsCenter.Core.Entities;

public partial class Todo
{
    public int TodoId { get; set; }

    public int? PracownikAdmId { get; set; }

    public int? WlascicielKlubuId { get; set; }

    public int? PomocSprzatajacaId { get; set; }

    public int? TrenerId { get; set; }

    public int Opis { get; set; }

    public virtual PomocSprzatajaca? PomocSprzatajaca { get; set; }

    public virtual PracownikAdministracyjny? PracownikAdm { get; set; }

    public virtual Trener? Trener { get; set; }

    public virtual WlascicielKlubu? WlascicielKlubu { get; set; }
}