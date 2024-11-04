using Microsoft.EntityFrameworkCore;
using SportsCenter.Core.Entities;
using SportsCenter.Core.Repositories;

namespace SportsCenter.Infrastructure.DAL.Repositories;

public class UserRepository : IUserRepository
{
    private SportsCenterDbContext _dbContext;

    public UserRepository(SportsCenterDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public Task<Osoba?> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return _dbContext.Osobas.Where(o => o.Email == email).FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }

    public async Task AddClientAsync(Klient client, CancellationToken cancellationToken)
    {
        await _dbContext.Klients.AddAsync(client, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Klient?> GetClientByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await _dbContext.Klients
            .Include(k => k.KlientNavigation)
            .FirstOrDefaultAsync(k => k.KlientNavigation.Email == email, cancellationToken);
    }
    
    public async Task<Klient?> GetClientByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.Klients
            .Include(k => k.KlientNavigation)
            .FirstOrDefaultAsync(k => k.KlientNavigation.OsobaId == id, cancellationToken);
    }

    public async Task UpdateClientAsync(Klient client, CancellationToken cancellationToken)
    {
        _dbContext.Klients.Update(client);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}