using Newtonsoft.Json;
using JsonSubTypes;
using SportsCenter.Application.Schedule.Queries.GetScheduleInfo;

[JsonConverter(typeof(JsonSubtypes), "Type")]
[JsonSubtypes.KnownSubType(typeof(ScheduleInfoAdminDto), "Admin")]
[JsonSubtypes.KnownSubType(typeof(ScheduleInfoTrainerDto), "Trainer")]
[JsonSubtypes.KnownSubType(typeof(ScheduleInfoBasicDto), "Basic")]
public abstract class ScheduleInfoBaseDto
{
    public DateTime Date { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public int CourtNumber { get; set; }
    public string TrainerName { get; set; }
    public string  GroupName { get; set; }
    public string SkillLevel { get; set; }
    [JsonIgnore]
    public string Type { get; set; }
}
