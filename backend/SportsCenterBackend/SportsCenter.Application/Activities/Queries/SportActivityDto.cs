namespace SportsCenter.Application.Activities.Queries;

public class SportActivityDto
{
    public int SportActivityId { get; set; }
    public string Name { get; set; }
    public int Level { get; set; }
    public int MaxParticipants { get; set; }
    public decimal CostWithoutEquipment { get; set; }
    public decimal CostWithEquipment { get; set; }
}