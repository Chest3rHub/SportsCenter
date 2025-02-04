using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Schedule.Queries.GetScheduleInfo
{
    public class ScheduleInfoDto
    {
        public DateTime Date { get; set; } // Data zajęć lub rezerwacji
        public TimeSpan StartTime { get; set; } // Czas rozpoczęcia
        public TimeSpan EndTime { get; set; } // Czas zakończenia
        public int CourtNumber { get; set; } // Numer kortu
        public string TrainerName { get; set; } // Imię i nazwisko trenera

        // Dla rezerwacji
        public List<string> Participants { get; set; } = new List<string>(); // Lista uczestników
        public decimal ReservationCost { get; set; } // Koszt rezerwacji (jeśli dotyczy)
        public decimal Discount { get; set; } // Zniżka na rezerwację (jeśli dotyczy)
        public bool IsRecurring { get; set; } // Czy jest to rezerwacja cykliczna?

        // Dla zajęć stałych
        public string GroupName { get; set; } // Nazwa grupy (jeśli dotyczy)
        public string SkillLevel { get; set; } // Poziom zaawansowania (jeśli dotyczy)
    }
}
