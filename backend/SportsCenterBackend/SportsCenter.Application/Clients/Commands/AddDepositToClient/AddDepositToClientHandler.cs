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

namespace SportsCenter.Application.Clients.Commands.AddDeposit
{
    internal sealed class AddDepositToClientHandler : IRequestHandler<AddDepositToClient, Unit>
    {
        private readonly IClientRepository _clientRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AddDepositToClientHandler(IClientRepository clientRepository, IHttpContextAccessor httpContextAccessor)
        {
            _clientRepository = clientRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Unit> Handle(AddDepositToClient request, CancellationToken cancellationToken)
        {

            var client = await _clientRepository.GetClientByEmailAsync(request.Email, cancellationToken);
            if (client == null)
                throw new ClientNotFoundException(request.Email);

            client.Saldo += request.Deposit;
            await _clientRepository.UpdateClientAsync(client, cancellationToken);

            return Unit.Value;
        }
    }
}
