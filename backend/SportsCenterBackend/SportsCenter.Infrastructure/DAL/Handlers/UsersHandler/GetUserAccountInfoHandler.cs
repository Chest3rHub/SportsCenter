using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsCenter.Application.Users.Queries.GetUserAccountInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using SportsCenter.Application.Exceptions.UsersException;
using SportsCenter.Core.Repositories;

namespace SportsCenter.Infrastructure.DAL.Handlers.UsersHandler;

internal class GetUserAccountInfoHandler : IRequestHandler<GetUserAccountInfo, UserAccountInfoDto>
{
    private readonly SportsCenterDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserRepository _userRepository;

    public GetUserAccountInfoHandler(IHttpContextAccessor httpContextAccessor, SportsCenterDbContext dbContext, IUserRepository userRepository)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
        _userRepository = userRepository;
    }

    public async Task<UserAccountInfoDto> Handle(GetUserAccountInfo request, CancellationToken cancellationToken)
    {
        var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst("userId")?.Value;
        var roleClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
        {
            throw new UnauthorizedAccessException("You are not authorized to access account informations.");
        }

        var user = await _dbContext.Osobas
            .Include(o => o.Klient)
            .FirstOrDefaultAsync(o => o.OsobaId == userId, cancellationToken);

        if (user == null)
        {
            throw new UserNotFoundException(userId);
        }

        string role = roleClaim ?? "Unknown";
        decimal? balance = null;

        if (user.Klient != null)
        {
            balance = user.Klient.Saldo;
        }

        return new UserAccountInfoDto
        {
            Name = user.Imie,
            LastName = user.Nazwisko,
            Address = user.Adres,
            PhoneNumber = user.NrTel,
            DateOfBirth = user.DataUr,
            Email = user.Email,
            Role = role,
            Balance = balance
        };
    }
}
