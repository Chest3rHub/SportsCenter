using SportsCenter.Application.Abstractions;
using MediatR;

namespace SportsCenter.Application.Clients.Commands.RemoveClientTags;

public sealed record RemoveClientTags : ICommand<Unit>
{
    public int ClientId { get; set; }
    public List<int> TagIds { get; set; }

    public RemoveClientTags(int clientId, List<int> tagIds)
    {
        ClientId = clientId;
        TagIds = tagIds;
    }
}
