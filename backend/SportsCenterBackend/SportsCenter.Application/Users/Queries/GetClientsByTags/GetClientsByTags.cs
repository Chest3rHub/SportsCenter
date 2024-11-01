using SportsCenter.Application.Abstractions;
using SportsCenter.Core.Entities;

namespace SportsCenter.Application.Users.Queries.GetClientsByTags;

public class GetClientsByTags : IQuery<IEnumerable<ClientByTagsDto>>
{
    public List<int> TagIds { get; set; }
}
