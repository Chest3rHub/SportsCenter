using SportsCenter.Application.Abstractions;

namespace SportsCenter.Application.Users.Queries.GetClientsByAge;

public class GetClientsByAge : IQuery<IEnumerable<ClientByAgeDto>>
{
    public int MinAge { get; set; }
    public int MaxAge { get; set; }
}
