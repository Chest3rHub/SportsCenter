using Microsoft.EntityFrameworkCore;
using SportsCenterBackend.Context;
using SportsCenterBackend.DTOs;
using SportsCenterBackend.Entities;

namespace SportsCenterBackend.Services;

public class LoginDbService : ILoginDbService
{
    
    private readonly SportsCenterDbContext _context;

    public LoginDbService(SportsCenterDbContext context)
    {
        _context = context;
    }
    public async Task<Osoba> LoginAsync(LoginDTO login)
    {
        // zakomentowane to oraz konfiguracja w programcs ktora usuwa blad ale dodaje nowe pole w jsonach
        //var result = await _context.Osobas
        //    .Include(o => o.Klient) 
        //    .SingleOrDefaultAsync(x => x.Email == login.Email);
        var result = await _context.Osobas
            .Include(o => o.Klient) 
            .SingleOrDefaultAsync(x => x.Email == login.Email);


        if (result == null || !BCrypt.Net.BCrypt.Verify(login.Password, result.Haslo))
        {
            throw new Exception("Błędny login lub hasło : LoginDbService");
        }

        return result;
    }
}