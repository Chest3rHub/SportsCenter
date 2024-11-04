using MediatR;

namespace SportsCenter.Application.Users.Commands.AddClientTags
{
    public sealed record GetClientTagCount : IRequest<int>
    {
        public int ClientId { get; init; }

        public GetClientTagCount(int clientId)
        {
            ClientId = clientId;
        }
    }
}