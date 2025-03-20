using Newtonsoft.Json;
using System;

public class TimeOnlyConverter : JsonConverter<TimeOnly>
{
    public override TimeOnly ReadJson(JsonReader reader, Type objectType, TimeOnly existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var timeString = (string)reader.Value;
        return TimeOnly.ParseExact(timeString, "HH:mm", null);
    }

    public override void WriteJson(JsonWriter writer, TimeOnly value, JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString("HH:mm"));
    }
}
