using System.Collections.Generic;
using SportsCenter.Application.Abstractions;

namespace SportsCenter.Application.Clients.Queries.GetClients;

public class GetClients : IQuery<IEnumerable<ClientDto>>
{
    public int Offset { get; set; } = 0;
    public GetClients(int offSet)
    {
        Offset = offSet;
    }
}