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

    public Task AddClientAsync(Klient client, CancellationToken cancellationToken)
    {
        _klienci.Add(client);
        return Task.CompletedTask;
    }

    public Task<Klient?> GetClientByIdAsync(int id, CancellationToken cancellationToken)
    {
        // nie testowalem, wrzucam zeby usunac bledy z projektu
        return Task.FromResult(_klienci.FirstOrDefault(e => e.KlientId == id));
    }

    public Task<Klient?> GetClientByEmailAsync(string email, CancellationToken cancellationToken)
    {
        // nie testowalem, wrzucam zeby usunac bledy z projektu
        return Task.FromResult(
            _klienci
                .Join(
                    _osoby,
                    klient => klient.KlientId,
                    osoba => osoba.OsobaId,
                    (klient, osoba) => new { Klient = klient, Osoba = osoba }
                )
                .Where(joined => joined.Osoba.Email == email)
                .Select(joined => joined.Klient)
                .SingleOrDefault()
        );

    }
    public Task UpdateClientAsync(Klient client, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}