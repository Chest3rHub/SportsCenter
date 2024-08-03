namespace SportsCenterBackend.DTOs;

public class RegisterClientDTO
{
    
    public string Imie { get; set; } = null!;

    public string Nazwisko { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Haslo { get; set; } = null!;

    public DateTime? DataUr { get; set; }

    public string NrTel { get; set; } = null!;
    
    public string Adres { get; set; } = null!;
    
}