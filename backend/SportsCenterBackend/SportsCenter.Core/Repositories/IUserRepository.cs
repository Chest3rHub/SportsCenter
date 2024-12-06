using SportsCenter.Core.Entities;

namespace SportsCenter.Core.Repositories;

public interface IUserRepository
{
    Task<Osoba?> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
    Task<Osoba?> GetUserByEmailWithRoleAsync(string email, CancellationToken cancellationToken);
    Task<Osoba?> GetUserByIdAsync(int id, CancellationToken cancellationToken);
    Task UpdateUserAsync(Osoba user, CancellationToken cancellationToken);
}