using MediatR.NotificationPublishers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Activities.Queries.GetTrainerSportActivitiesByWeeks
{
    public class TrainerSportActivityByWeeksDto
    {
        public int? InstanceOfActivityId { get; set; }
        public int? ReservationId { get; set; }
        public string SportActivityName { get; set; }
        public DateOnly DateOfActivity { get; set; }
        public string DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public int DurationInMinutes { get; set; }
        public TimeSpan EndTime { get; set; }
        public string LevelName { get; set; }
        public int EmployeeId { get; set; }
        public string ClientName { get; set; }
        public string ClientSurname { get; set; }
        public string CourtName { get; set; }
        public bool IsEquipmentReserved { get; set; }
        public bool IsActivityCanceled { get; set; }
        public string Type { get; set; }
    }
}
