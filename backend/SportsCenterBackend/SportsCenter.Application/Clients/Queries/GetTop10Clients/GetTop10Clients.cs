using SportsCenter.Application.Abstractions;

namespace SportsCenter.Application.Clients.Queries.GetTop10Clients
{
    public class GetTop10Clients : IQuery<IEnumerable<Top10ClientDto>>
    {
        public string? Text { get; set; }

        public GetTop10Clients(string text)
        {
            Text = text;
        }
    }
}
