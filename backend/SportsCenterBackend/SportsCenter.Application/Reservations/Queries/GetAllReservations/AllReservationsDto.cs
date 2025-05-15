namespace SportsCenter.Application.Reservations.Queries.GetYourReservations
{
    public class AllReservationsDto
    {
        public int ReservationId { get; set; }
        public string ClientEmail { get; set; }
        public string CourtName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Trainer { get; set; }
        public bool IsEquipmentReserved { get; set; }
        public decimal Cost { get; set; }
        public bool IsReservationPaid { get; set; }
        public bool IsReservationCanceled { get; set; }
        public bool IsMoneyRefunded { get; set; }
    }
}
