namespace SportsCenter.Application.Reservations.Queries.getCourtEvents
{
    public class CourtEventsDto
    {
        public int EventId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsReservation { get; set; }
    }
}