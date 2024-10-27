using MediatR;
using SportsCenter.Application.Exceptions;
using SportsCenter.Application.Security;
using SportsCenter.Core.Repositories;

namespace SportsCenter.Application.Users.Commands.Login
{
    internal sealed class LoginHandler : IRequestHandler<Login, LoginResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordManager _passwordManager;
        private readonly IJWTTokenGenerator _jwtTokenGenerator;

        public LoginHandler(IUserRepository userRepository, IPasswordManager passwordManager, IJWTTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _passwordManager = passwordManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<LoginResponse> Handle(Login request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.Email, cancellationToken);
            if (user == null || !_passwordManager.Validate(request.Password, user.Haslo))
            {
                throw new InvalidLoginException();
            }

            var token = _jwtTokenGenerator.GenerateToken(user);
            // var refresToken = _jwtTokenGenerator.GenerateRefreshToken();

            return new LoginResponse
            {
                Token = token
            };
        }
    }
}
