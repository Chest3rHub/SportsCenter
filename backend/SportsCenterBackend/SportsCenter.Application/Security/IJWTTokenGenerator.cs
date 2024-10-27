using SportsCenter.Core.Entities;

namespace SportsCenter.Application.Security
{
    public interface IJWTTokenGenerator
    {
        string GenerateToken(Osoba osoba);
        string GenerateRefreshToken();
    }
}
