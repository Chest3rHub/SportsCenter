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
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(clientDto.Haslo);

                var newOsoba = new Osoba()
                {
                    Imie = clientDto.Imie,
                    Nazwisko = clientDto.Nazwisko,
                    Adres = clientDto.Adres,
                    DataUr = clientDto.DataUr,
                    Email = clientDto.Email,
                    Haslo = hashedPassword,
                    NrTel = clientDto.NrTel
                };
                _context.Osobas.Add(newOsoba);
                await _context.SaveChangesAsync();

                var client = new Klient()
                {
                    KlientId = newOsoba.OsobaId,
                    Saldo = 0,
                };
                _context.Klients.Add(client);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}