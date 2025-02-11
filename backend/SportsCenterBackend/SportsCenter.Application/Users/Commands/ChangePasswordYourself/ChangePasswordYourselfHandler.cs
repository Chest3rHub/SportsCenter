using MediatR;
using Microsoft.AspNetCore.Http;
using SportsCenter.Application.Exceptions.UsersException;
using SportsCenter.Application.Security;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//klasa sluzaca do tego by uzytkownik mogl sam sobie zmienic haslo

namespace SportsCenter.Application.Users.Commands.ChangePassowrd
{
    internal sealed class ChangePasswordYourselfHandler : IRequestHandler<ChangePasswordYourself, Unit>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordManager _passwordManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ChangePasswordYourselfHandler(IUserRepository userRepository, IPasswordManager passwordManager, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _passwordManager = passwordManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Unit> Handle(ChangePasswordYourself request, CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new UnauthorizedAccessException("You are not authorized to change your password.");
            }

            var user = await _userRepository.GetUserByIdAsync(userId, cancellationToken);
            if (user == null)
            {
                throw new UserNotFoundException(userId);
            }

            user.Haslo = _passwordManager.Secure(request.Value);
            await _userRepository.UpdateUserAsync(user, cancellationToken);

            return Unit.Value;
        }
    }
}
