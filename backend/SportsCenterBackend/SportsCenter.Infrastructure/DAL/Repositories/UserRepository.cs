using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SportsCenter.Core.Entities;
using SportsCenter.Core.Repositories;
using SportsCenterBackend.Context;

namespace SportsCenter.Infrastructure.DAL.Repositories;

internal class UserRepository : IUserRepository
{
    private readonly SportsCenterDbContext _dbContext;

    public UserRepository(SportsCenterDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Osoba?> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return _dbContext.Osobas.Include(o => o.Klient)
            .SingleOrDefaultAsync(e => e.Email == email, cancellationToken);
    }

    public async Task AddClientAsync(Klient client, CancellationToken cancellationToken)
    {
        await _dbContext.Klients.AddAsync(client, cancellationToken);
    }
}