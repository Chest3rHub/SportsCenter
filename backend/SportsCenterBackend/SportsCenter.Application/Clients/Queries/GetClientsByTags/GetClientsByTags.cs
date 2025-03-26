using SportsCenter.Application.Abstractions;
using SportsCenter.Core.Entities;
using static System.Net.Mime.MediaTypeNames;

namespace SportsCenter.Application.Clients.Queries.GetClientsByTags;

public class GetClientsByTags : IQuery<IEnumerable<ClientByTagsDto>>
{
    public List<int> TagIds { get; set; }
    public int Offset { get; set; }
    public GetClientsByTags(int offSet, List<int> tagIds)
    {
        Offset = offSet;
        TagIds = tagIds;
    }
}
