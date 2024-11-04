using SportsCenter.Application.Abstractions;
using MediatR;

namespace SportsCenter.Application.Users.Commands.AddClientTags;

public sealed record AddClientTags : ICommand<Unit>
{
    public int ClientId { get; set; }
    public List<int> TagIds { get; set; }
    
    public AddClientTags(int clientId, List<int> tagIds) { 
        ClientId = clientId;
        TagIds = tagIds;
    }
    
    
}