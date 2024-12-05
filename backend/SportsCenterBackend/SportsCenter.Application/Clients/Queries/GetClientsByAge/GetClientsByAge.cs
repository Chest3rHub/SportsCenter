using SportsCenter.Application.Abstractions;

namespace SportsCenter.Application.Clients.Queries.GetClientsByAge;

public class GetClientsByAge : IQuery<IEnumerable<ClientByAgeDto>>
{
    public int MinAge { get; set; }
    public int MaxAge { get; set; }
}
