namespace SportsCenter.Core.Entities;

public partial class KlientTag
{
    public int KlientTagId { get; set; }

    public int KlientKlientId { get; set; }

    public int TagTagId { get; set; }

    public virtual Klient KlientKlient { get; set; } = null!;

    public virtual Tag TagTag { get; set; } = null!;
}