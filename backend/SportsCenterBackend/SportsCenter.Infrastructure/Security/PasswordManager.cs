using Microsoft.AspNetCore.Identity;
using SportsCenter.Application.Security;
using SportsCenter.Core.Entities;

namespace SportsCenter.Infrastructure.Security;

internal class PasswordManager : IPasswordManager
{
    private readonly IPasswordHasher<Osoba> _passwordHasher;

    public PasswordManager(IPasswordHasher<Osoba> passwordHasher)
    {
        _passwordHasher = passwordHasher;
    }

    public string Secure(string password)
    {
        return _passwordHasher.HashPassword(default, password);
    }

    public bool Validate(string password, string securedPassword)
    {
        return _passwordHasher.VerifyHashedPassword(default, securedPassword, password) ==
               PasswordVerificationResult.Success;
    }
}