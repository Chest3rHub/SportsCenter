using SportsCenter.Application.Abstractions;

namespace SportsCenter.Application.Reservations.Queries.getCourtEvents
{
    public class getCourtEvents : IQuery<IEnumerable<CourtEventsDto>>
    {
        public int CourtId { get; set; }
        public DateTime Date { get; set; } 

        public getCourtEvents(int courtId, DateTime date)
        {
            CourtId = courtId;
            Date = date;
        }
    }
}