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
    internal sealed class ChangePasswordHandler : IRequestHandler<ChangePassword, Unit>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordManager _passwordManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ChangePasswordHandler(IUserRepository userRepository, IPasswordManager passwordManager, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _passwordManager = passwordManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Unit> Handle(ChangePassword request, CancellationToken cancellationToken)
        {
            //w przyszlosci pobranie id uzytkownika z sesji (aby sam sobie mogl zmienic haslo)

            var user = await _userRepository.GetUserByIdAsync(request.UserId, cancellationToken);
            if (user == null)
                throw new UserNotFoundException(request.UserId);
           
            user.Haslo = _passwordManager.Secure(request.Value);
            await _userRepository.UpdateUserAsync(user, cancellationToken);

            return Unit.Value;
        }
    }
}
