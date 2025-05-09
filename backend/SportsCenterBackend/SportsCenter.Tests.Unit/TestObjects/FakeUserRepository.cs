using SportsCenter.Core.Entities;
using SportsCenter.Core.Repositories;

namespace SportsCenter.Tests.Unit.TestObjects;

public class FakeUserRepository : IUserRepository
{
    private List<Osoba> _osoby = new();
    private List<Klient> _klienci = new();

    public FakeUserRepository()
    {
    }

    public FakeUserRepository(List<Osoba> osoby)
    {
        _osoby = osoby;
    }

    public FakeUserRepository(List<Klient> klienci)
    {
        _klienci = klienci;
    }

    public Task<Osoba?> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return Task.FromResult(_osoby.SingleOrDefault(e => e.Email == email));
    }

    public Task<Osoba?> GetUserByEmailWithRoleAsync(string email, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Osoba?> GetUserByIdAsync(int id, CancellationToken cancellationToken)
    {
        return Task.FromResult(_osoby.SingleOrDefault(e => e.OsobaId ==  id));
    }

    public Task UpdateUserAsync(Osoba user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
  