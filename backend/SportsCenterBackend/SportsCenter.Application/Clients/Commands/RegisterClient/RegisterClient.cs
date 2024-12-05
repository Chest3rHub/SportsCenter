using System;
using MediatR;
using SportsCenter.Application.Abstractions;

namespace SportsCenter.Application.Clients.Commands.RegisterClient;

public sealed record RegisterClient : ICommand<Unit>
{
    public string Imie { get; } = null!;
    public string Nazwisko { get; } = null!;
    public string Email { get; } = null!;
    public string Haslo { get; } = null!;
    public DateTime DataUr { get; }
    public string NrTel { get; } = null!;
    public string Adres { get; } = null!;

    public RegisterClient(string imie, string nazwisko, string email, string haslo, DateTime dataUr, string nrTel,
        string adres)
    {
        Imie = imie;
        Nazwisko = nazwisko;
        Email = email;
        Haslo = haslo;
        DataUr = dataUr;
        NrTel = nrTel;
        Adres = adres;
    }
}