using MediatR;
using SportsCenter.Application.Abstractions;

namespace SportsCenter.Application.Users.Commands.Login
{
    public sealed record Login : ICommand<LoginResponse>
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;

        public Login(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
