using MediatR;
using Microsoft.AspNetCore.Http;
using SportsCenter.Application.Clients.Commands.AddDeposit;
using SportsCenter.Application.Exceptions.ClientsExceptions;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Clients.Commands.AddDepositYourself
{
    internal sealed class AddDepositYourselfHandler : IRequestHandler<AddDepositYourself, Unit>
    {
        private readonly IClientRepository _clientRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AddDepositYourselfHandler(IClientRepository clientRepository, IHttpContextAccessor httpContextAccessor)
        {
            _clientRepository = clientRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Unit> Handle(AddDepositYourself request, CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new UnauthorizedAccessException("You are not authorized to perform this action.");
            }

            var client = await _clientRepository.GetClientByIdAsync(userId, cancellationToken);
            if (client == null)
            {
                throw new ClientWithGivenIdNotFoundException(userId);
            }

            client.Saldo += request.Deposit;
            await _clientRepository.UpdateClientAsync(client, cancellationToken);

            return Unit.Value;

        }
    }
}
