using Newtonsoft.Json;
using JsonSubTypes;
using SportsCenter.Application.Schedule.Queries.GetScheduleInfo;

[JsonConverter(typeof(JsonSubtypes), "Type")]
[JsonSubtypes.KnownSubType(typeof(ScheduleInfoAdminDto), "Admin")]
[JsonSubtypes.KnownSubType(typeof(ScheduleInfoTrainerDto), "Trainer")]
[JsonSubtypes.KnownSubType(typeof(ScheduleInfoBasicDto), "Basic")]
public abstract class ScheduleInfoBaseDto
{
    [JsonProperty(Order = 1)]
    public string Description { get; set; }
    [JsonProperty(Order = 2)]
    public int Id { get; set; }
    [JsonProperty(Order = 3)]
    public DateTime Date { get; set; }
    [JsonProperty(Order = 4)]
    public TimeSpan StartTime { get; set; }
    [JsonProperty(Order = 5)]
    public TimeSpan EndTime { get; set; }
    [JsonProperty(Order = 6)]
    public string CourtName { get; set; }
    [JsonProperty(Order = 7)]
    public string TrainerName { get; set; }
    [JsonProperty(Order = 8)]
    public string  GroupName { get; set; }
    [JsonProperty(Order = 9)]
    public string SkillLevel { get; set; }
    [JsonIgnore]
    public string Type { get; set; }
}
