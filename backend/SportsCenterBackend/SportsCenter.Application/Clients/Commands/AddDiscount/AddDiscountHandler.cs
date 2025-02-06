using MediatR;
using Microsoft.AspNetCore.Http;
using SportsCenter.Application.Exceptions.ClientsExceptions;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SportsCenter.Application.Clients.Commands.AddDiscount
{
    internal sealed class AddDiscountHandler : IRequestHandler<AddDiscount, Unit>
    {
        private readonly IClientRepository _clientRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AddDiscountHandler(IClientRepository clientRepository, IHttpContextAccessor httpContextAccessor)
        {
            _clientRepository = clientRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Unit> Handle(AddDiscount request, CancellationToken cancellationToken)
        {
            var client = await _clientRepository.GetClientByIdAsync(request.ClientId, cancellationToken);
            if (client == null)
            {
                throw new ClientWithGivenIdNotFoundException(request.ClientId);
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
