using MediatR;
using Microsoft.AspNetCore.Http;
using SportsCenter.Application.Exceptions.ClientsExceptions;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Clients.Commands.UpdateClientDeposit
{
    internal sealed class UpdateClientDepositHandler : IRequestHandler<UpdateClientDeposit, Unit>
    {
        private readonly IClientRepository _clientRepository;

        public UpdateClientDepositHandler(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }
        public async Task<Unit> Handle(UpdateClientDeposit request, CancellationToken cancellationToken)
        {
            var client = await _clientRepository.GetClientByEmailAsync(request.Email, cancellationToken);
            if (client == null)
                throw new ClientNotFoundException(request.Email);

            client.Saldo = request.Deposit;
            await _clientRepository.UpdateClientAsync(client, cancellationToken);
            return Unit.Value;
        }
    }
}
