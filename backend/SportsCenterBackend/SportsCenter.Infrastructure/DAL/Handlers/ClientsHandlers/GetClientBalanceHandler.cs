using MediatR;
using SportsCenter.Application.Clients.Queries.GetClientBalance;
using SportsCenter.Core.Repositories;


namespace SportsCenter.Infrastructure.DAL.Handlers.ClientsHandlers
{
    internal class GetClientBalanceHandler : IRequestHandler<GetClientBalance, ClientBalanceDto>
    {
        private readonly IClientRepository _clientRepository;

        public GetClientBalanceHandler(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<ClientBalanceDto> Handle(GetClientBalance request, CancellationToken cancellationToken)
        {
            var balance = await _clientRepository.GetClientBalanceAsync(request.ClientId, cancellationToken);
            return new ClientBalanceDto { Balance = balance };
        }
    }
}