namespace SportsCenter.Core.Entities;

public partial class TypPracownika
{
    public int IdTypPracownika { get; set; }

    public string Nazwa { get; set; } = null!;

    public virtual ICollection<Pracownik> Pracowniks { get; set; } = new List<Pracownik>();
}
