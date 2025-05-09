using SportsCenter.Application.Abstractions;

namespace SportsCenter.Application.Reservations.Queries.GetCourtReservations
{
    public class GetCourtReservations : IQuery<IEnumerable<CourtReservationsDto>>
    {
        public int CourtId { get; set; }
        public DateTime Date { get; set; } 

        public GetCourtReservations(int courtId, DateTime date)
        {
            CourtId = courtId;
            Date = date;
        }
    }
}