namespace SportsCenter.Application.Reservations.Queries.GetCourtReservations
{
    public class CourtReservationsDto
    {
        public int ReservationId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}