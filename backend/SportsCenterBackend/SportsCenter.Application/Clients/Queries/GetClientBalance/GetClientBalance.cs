using SportsCenter.Application.Abstractions;

namespace SportsCenter.Application.Clients.Queries.GetClientBalance
{
    public class GetClientBalance : IQuery<ClientBalanceDto>
    {
        public int ClientId { get; set; }
        public GetClientBalance(int clientId)
        {
            ClientId = clientId;
        }
    }
}