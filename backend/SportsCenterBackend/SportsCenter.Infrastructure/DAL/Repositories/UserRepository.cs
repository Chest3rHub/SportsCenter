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

    public Task<Osoba?> GetUserByIdAsync(int id, CancellationToken cancellationToken)
    {
        return _dbContext.Osobas.Where(o => o.OsobaId == id).FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }

    public async Task UpdateUserAsync(Osoba user, CancellationToken cancellationToken)
    {
        _dbContext.Osobas.Update(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}