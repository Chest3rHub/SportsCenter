using Microsoft.EntityFrameworkCore;
using SportsCenter.Core.Entities;
using SportsCenter.Core.Repositories;

namespace SportsCenter.Infrastructure.DAL.Repositories;

public class UsersRepository : IUserRepository
{
    private ApplicationDbContext _dbContext;

    public UsersRepository(ApplicationDbContext dbContext)
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
    }
}