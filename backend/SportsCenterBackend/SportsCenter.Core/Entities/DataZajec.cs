namespace SportsCenter.Core.Entities;

public partial class DataZajec
{
    public int DataZajecId { get; set; }

    public DateTime Date { get; set; }

    public int GrafikZajecId { get; set; }

    public virtual SportActivitySchedule SportActivitySchedule { get; set; } = null!;
}
