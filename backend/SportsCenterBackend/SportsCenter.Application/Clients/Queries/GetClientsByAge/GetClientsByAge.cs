using SportsCenter.Application.Abstractions;

namespace SportsCenter.Application.Clients.Queries.GetClientsByAge;

public class GetClientsByAge : IQuery<IEnumerable<ClientByAgeDto>>
{
    public int MinAge { get; set; }
    public int MaxAge { get; set; }
    public int Offset { get; set; }

    public GetClientsByAge(int offSet, int minAge, int maxAge)
    {
        Offset = offSet;
        MinAge = minAge;
        MaxAge = maxAge;
    }
}
