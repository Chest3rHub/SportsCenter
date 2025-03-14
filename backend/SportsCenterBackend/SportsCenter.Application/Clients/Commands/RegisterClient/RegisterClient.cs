using System;
using MediatR;
using SportsCenter.Application.Abstractions;

namespace SportsCenter.Application.Clients.Commands.RegisterClient;

public sealed record RegisterClient : ICommand<Unit>
{
    public string Name { get; } = null!;
    public string Surname { get; } = null!;
    public string Email { get; } = null!;
    public string Password { get; } = null!;
    public DateTime BirthDate { get; }
    public string PhoneNumber { get; } = null!;
    public string Address { get; } = null!;

    public RegisterClient(string name, string surname, string email, string password, DateTime birthDate, string phoneNumber,
        string address)
    {
        Name = name;
        Surname = surname;
        Email = email;
        Password = password;
        BirthDate = birthDate;
        PhoneNumber = phoneNumber;
        Address = address;
    }
}