using MediatR;
using Newtonsoft.Json;
using SportsCenter.Application.Abstractions;

namespace SportsCenter.Application.Activities.Commands.AddSportActivity;

public sealed record AddSportActivity : ICommand<Unit>
{
    public string SportActivityName { get; set; }
    public DateOnly StartDate { get; set; }
    public string DayOfWeek { get; set; }
    [JsonConverter(typeof(TimeOnlyConverter))]
    public TimeOnly StartHour { get; set; }
    public int DurationInMinutes { get; set; }
    public string LevelName { get; set; }
    public int EmployeeId { get; set; }
    public int ParticipantLimit { get; set; }
    public string CourtName { get; set; }
    public decimal CostWithoutEquipment { get; set; }
    public decimal CostWithEquipment { get; set; }

    public AddSportActivity(string sportActivityName, DateOnly startDAte, string dayOfWeek, TimeOnly startHour, int durationInMinutes, string levelName, int employeeId, int participantLimit, string courtName, decimal costWithoutEquipment, decimal costWithEquipment)
    {
        SportActivityName = sportActivityName;
        StartDate = startDAte;
        DayOfWeek = dayOfWeek;
        StartHour = startHour;
        DurationInMinutes = durationInMinutes;
        LevelName = levelName;
        EmployeeId = employeeId;
        ParticipantLimit = participantLimit;
        CourtName = courtName;
        CostWithoutEquipment = costWithoutEquipment;
        CostWithEquipment = costWithEquipment;
    }
}
