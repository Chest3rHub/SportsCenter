using SportsCenter.Core.Entities;

namespace SportsCenter.Core.Repositories;

public interface IUserRepository
{
    Task<Osoba?> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
    Task AddClientAsync(Klient client, CancellationToken cancellationToken);
    Task<Klient?> GetClientByEmailAsync(string email, CancellationToken cancellationToken);
    Task UpdateClientAsync(Klient client, CancellationToken cancellationToken);
}