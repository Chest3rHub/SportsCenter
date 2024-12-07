using MediatR;
using SportsCenter.Application.Abstractions;

namespace SportsCenter.Application.Activities.Commands.AddSportActivity;

public sealed record AddSportActivity : ICommand<Unit>
{
    public string SportActivityName { get; set; }
    public int LevelId { get; set; }
    public int Duration { get; set; }
    public int EmployeeId { get; set; }
    public int ParticipantLimit { get; set; }
    public int CourtId { get; set; }
    public decimal CostWithoutEquipment { get; set; }
    public decimal CostWithEquipment { get; set; }
    
    public AddSportActivity(string sportActivityName, int levelId, int duration, int employeeId, int participantLimit, int courtId, decimal costWithoutEquipment, decimal costWithEquipment)
    {
        SportActivityName = sportActivityName;
        LevelId = levelId;
        Duration = duration;
        EmployeeId = employeeId;
        ParticipantLimit = participantLimit;
        CourtId = courtId;
        CostWithoutEquipment = costWithoutEquipment;
        CostWithEquipment = costWithEquipment;
    }
}
