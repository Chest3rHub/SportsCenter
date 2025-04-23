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
using SportsCenter.Application.Exceptions.UsersExceptions;

namespace SportsCenter.Infrastructure.Security
{
    internal class JWTTokenGenerator : IJWTTokenGenerator
    {
        private readonly IConfiguration _configuration;

        public JWTTokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(Osoba osoba, string role)
        {
            var jwtSecret = _configuration["Auth:SigningKey"];
            var jwtIssuer = _configuration["Auth:Issuer"];
            var jwtAudience = _configuration["Auth:Audience"];
            var expiryTimeSpan = TimeSpan.Parse(_configuration["Auth:Expiry"]);
            var maxRefreshTimeSpan = TimeSpan.Parse(_configuration["Auth:MaxRefreshTime"]);

            if (jwtSecret is null) throw new InvalidOperationException("JWT secret is missing in configuration");

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("userId", osoba.OsobaId.ToString()),
                // nie usuwam tego clam Types.role dlatego dwie role zwraca
                new Claim(ClaimTypes.Role, role),
                new Claim("role", role),
                new Claim("loginDate", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")),
                new Claim("maxRefreshTime", DateTime.UtcNow.Add(maxRefreshTimeSpan).ToString("yyyy-MM-ddTHH:mm:ssZ"))
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

        public string GenerateRefreshToken(string currentToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(currentToken);
            
            var loginDateClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "loginDate")?.Value;
            var maxRefreshTimeClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "maxRefreshTime")?.Value;

            if (loginDateClaim == null || maxRefreshTimeClaim == null)
            {
                throw new InvalidTokenException("Token does not contain required claims.");
            }

            DateTime loginDate = DateTime.Parse(loginDateClaim);
            DateTime maxRefreshTime = DateTime.Parse(maxRefreshTimeClaim);

            if (DateTime.UtcNow > maxRefreshTime)
            {
                throw new InvalidTokenException("Token is no longer valid as 24 hours have passed since login.");
            }

            var jwtSecret = _configuration["Auth:SigningKey"];
            var jwtIssuer = _configuration["Auth:Issuer"];
            var jwtAudience = _configuration["Auth:Audience"];
            var expiryTimeSpan =TimeSpan.Parse(_configuration["Auth:Expiry"]); 

            if (jwtSecret is null) throw new InvalidOperationException("JWT secret is missing in configuration");

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var role = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("userId", jwtToken.Claims.FirstOrDefault(c => c.Type == "userId")?.Value),
                // nie usuwam tego claimTypes.Role dlatego sa dwie role
                new Claim(ClaimTypes.Role, role),
                new Claim("role", role),
                new Claim("loginDate", loginDateClaim),
                new Claim("maxRefreshTime", maxRefreshTimeClaim)
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

        public string DetermineUserRole(Osoba osoba)
        {
            if (osoba.Klient != null)
                return "Klient";

            if (osoba.Pracownik != null && osoba.Pracownik.IdTypPracownikaNavigation?.Nazwa != null)
                return osoba.Pracownik.IdTypPracownikaNavigation.Nazwa;

            throw new InvalidUserRoleException();
        }
    }
}
