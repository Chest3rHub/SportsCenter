using MediatR;
using SportsCenter.Application.Exceptions;
using SportsCenter.Application.Exceptions.UsersException;
using SportsCenter.Application.Exceptions.UsersExceptions;
using SportsCenter.Application.Security;
using SportsCenter.Application.Users.Commands.RefreshToken;
using SportsCenter.Core.Repositories;

namespace SportsCenter.Application.Users.Commands.RefreshToken
{
    internal sealed class RefreshTokenHandler : IRequestHandler<RefreshToken, RefreshTokenResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordManager _passwordManager;
        private readonly IJWTTokenGenerator _jwtTokenGenerator;

        public RefreshTokenHandler(IUserRepository userRepository, IPasswordManager passwordManager, IJWTTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _passwordManager = passwordManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<RefreshTokenResponse> Handle(RefreshToken request, CancellationToken cancellationToken)
        {
            
            var refreshToken = _jwtTokenGenerator.GenerateRefreshToken(request.Token);

            return new RefreshTokenResponse
            {
                Token = refreshToken
            };
        }
    }
}