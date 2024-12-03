using MediatR;
using Microsoft.AspNetCore.Http;
using SportsCenter.Application.Exceptions;
using SportsCenter.Application.Exceptions.ClientsExceptions;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Users.Commands.AccountDeposit
{
    internal sealed class AddDepositHandler : IRequestHandler<AddDeposit, Unit>
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AddDepositHandler(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Unit> Handle(AddDeposit request, CancellationToken cancellationToken)
        {

            var client = await _userRepository.GetClientByEmailAsync(request.Email, cancellationToken);
            if (client == null)
                throw new ClientNotFoundException(request.Email);

            client.Saldo += request.Deposit;
            await _userRepository.UpdateClientAsync(client, cancellationToken);

            return Unit.Value;
        }
    }
}
