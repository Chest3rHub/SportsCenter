using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Reservations.Queries.GetYourReservations
{
    public class YourReservationDto
    {
        public int ReservationId { get; set; }
        public int CourtId { get; set; }
        public string CourtName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int? TrainerId { get; set; }
        public bool IsEquipmentReserved { get; set; }
        public decimal Cost { get; set; }
        public bool IsReservationPaid { get; set; }
        public bool IsReservationCanceled { get; set; }
        public bool IsMoneyRefunded { get; set; }
    }
}