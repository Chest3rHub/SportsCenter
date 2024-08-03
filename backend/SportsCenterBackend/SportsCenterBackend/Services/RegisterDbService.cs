using SportsCenterBackend.Context;
using SportsCenterBackend.DTOs;
using SportsCenterBackend.Entities;

namespace SportsCenterBackend.Services;

public class RegisterDbService : IRegisterDbService
{
    
    private readonly SportsCenterDbContext _context;

    public RegisterDbService(SportsCenterDbContext context)
    {
        _context = context;
    }
    public async Task RegisterClientAsync(RegisterClientDTO clientDto)
    {
        _context.Osobas.Add(new Osoba()
        {
            Imie = clientDto.Imie,
            Nazwisko = clientDto.Nazwisko,
            Adres = clientDto.Adres,
            DataUr = clientDto.DataUr,
            Email = clientDto.Email,
            Haslo = clientDto.Haslo,
            NrTel = clientDto.NrTel
        });
        // dodac jednoczesne dodawanie osoby do tabeli Klient i walidacje czy email nie jest juz uzyty
        await _context.SaveChangesAsync();
    }
    
}