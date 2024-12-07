using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SportsCenter.Application.Security;
using SportsCenter.Core.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using SportsCenter.Application.Exceptions.UsersException;

namespace SportsCenter.Infrastructure.Security
{
    internal class JWTTokenGenerator : IJWTTokenGenerator
    {
        private readonly IConfiguration _configuration;

        public JWTTokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(Osoba osoba)
        {
            var jwtSecret = _configuration["Auth:SigningKey"];
            var jwtIssuer = _configuration["Auth:Issuer"];
            var jwtAudience = _configuration["Auth:Audience"];
            var expiryTimeSpan = TimeSpan.Parse(_configuration["Auth:Expiry"]);

            if (jwtSecret is null) throw new InvalidOperationException("JWT secret is missing in configuration");

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var role = DetermineUserRole(osoba);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("userId", osoba.OsobaId.ToString()),
                new Claim("role", role)
            };

            var tokenDescriptor = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.UtcNow.Add(expiryTimeSpan),
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        public string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        private string DetermineUserRole(Osoba osoba)
        {
            if (osoba.Klient != null)
                return "Klient";

            if (osoba.Pracownik != null && osoba.Pracownik.IdTypPracownikaNavigation?.Nazwa != null)
                return osoba.Pracownik.IdTypPracownikaNavigation.Nazwa;

            throw new InvalidUserRoleException();
        }
    }
}
