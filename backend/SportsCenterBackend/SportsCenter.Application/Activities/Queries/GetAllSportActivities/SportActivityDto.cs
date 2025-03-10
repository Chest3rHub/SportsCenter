namespace SportsCenter.Application.Activities.Queries.GetAllSportActivities
{

    public class SportActivityDto
    {
        public int SportActivityId { get; set; }
        public string DayOfWeek { get; set; }
        public TimeOnly StartHour { get; set; }
        public int DurationInMinutes { get; set; }
        public string ActivityName { get; set; }
        public string LevelName { get; set; }
        public string CourtName { get; set; }
        public int MaxParticipants { get; set; }
        public decimal CostWithoutEquipment { get; set; }
        public decimal CostWithEquipment { get; set; }
    }
}