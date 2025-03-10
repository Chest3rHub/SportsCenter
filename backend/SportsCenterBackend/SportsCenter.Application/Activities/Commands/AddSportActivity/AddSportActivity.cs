using MediatR;
using SportsCenter.Application.Abstractions;

namespace SportsCenter.Application.Activities.Commands.AddSportActivity;

public sealed record AddSportActivity : ICommand<Unit>
{
    public string SportActivityName { get; set; }
    public string LevelName { get; set; }
    public int DurationInMinutes { get; set; }
    public int EmployeeId { get; set; }
    public int ParticipantLimit { get; set; }
    public string CourtName { get; set; }
    public decimal CostWithoutEquipment { get; set; }
    public decimal CostWithEquipment { get; set; }
    public DateTime StartDate { get; set; }
    public string Recurrence { get; set; }

    public AddSportActivity(string sportActivityName, string levelName, int durationInMinutes, int employeeId, int participantLimit, string courtName, decimal costWithoutEquipment, decimal costWithEquipment, DateTime startDate, string recurrence)
    {
        SportActivityName = sportActivityName;
        LevelName = levelName;
        DurationInMinutes = durationInMinutes;
        EmployeeId = employeeId;
        ParticipantLimit = participantLimit;
        CourtName = courtName;
        CostWithoutEquipment = costWithoutEquipment;
        CostWithEquipment = costWithEquipment;
        StartDate = startDate;
        Recurrence = recurrence;
    }
}
