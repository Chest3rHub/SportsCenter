using MediatR;
using Microsoft.AspNetCore.Http;
using SportsCenter.Application.Exceptions.ClientsExceptions;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Clients.Commands.UpdateDiscount
{
    internal sealed class UpdateDiscountHandler : IRequestHandler<UpdateDiscount, Unit>
    {
        private readonly IClientRepository _clientRepository;

        public UpdateDiscountHandler(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }
        public async Task<Unit> Handle(UpdateDiscount request, CancellationToken cancellationToken)
        {
            var client = await _clientRepository.GetClientByEmailAsync(request.ClientEmail, cancellationToken);
            if (client == null)
            {
                throw new ClientNotFoundException(request.ClientEmail);
            }

            if (request.ActivityDiscount.HasValue)
            {
                client.ZnizkaNaZajecia = request.ActivityDiscount.Value;
            }
            if (request.ProductDiscount.HasValue)
            {
                client.ZnizkaNaProdukty = request.ProductDiscount.Value;
            }

            await _clientRepository.UpdateClientAsync(client, cancellationToken);
            return Unit.Value;
        }
    }
}
