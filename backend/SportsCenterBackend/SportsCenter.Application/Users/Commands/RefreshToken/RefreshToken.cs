using SportsCenter.Application.Abstractions;

namespace SportsCenter.Application.Users.Commands.RefreshToken;

public sealed record RefreshToken : ICommand<RefreshTokenResponse>
{
    public string Token { get; set; }

    public RefreshToken(string token)
    {
        Token = token;
    }
}