using MediatR;
using Microsoft.AspNetCore.Http;
using SportsCenter.Application.Exceptions.UsersException;
using SportsCenter.Application.Security;
using SportsCenter.Application.Users.Commands.ChangeUserPassword;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//klasa sluzaca do tego by uprawniony pracownik mogl zmienic haslo uzytkownikowi

namespace SportsCenter.Application.Users.Commands.ChangePassowrd
{
    internal sealed class ChangeOtherUserPasswordHandler : IRequestHandler<ChangeOtherUserPassword, Unit>
    {

        private readonly IUserRepository _userRepository;
        private readonly IPasswordManager _passwordManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ChangeOtherUserPasswordHandler(IUserRepository userRepository, IPasswordManager passwordManager, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _passwordManager = passwordManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Unit> Handle(ChangeOtherUserPassword request, CancellationToken cancellationToken)
        {

            var user = await _userRepository.GetUserByIdAsync(request.UserId, cancellationToken);
            if (user == null)
                throw new UserNotFoundException(request.UserId);

            user.Haslo = _passwordManager.Secure(request.Value);
            await _userRepository.UpdateUserAsync(user, cancellationToken);

            return Unit.Value;
        }
    }
}