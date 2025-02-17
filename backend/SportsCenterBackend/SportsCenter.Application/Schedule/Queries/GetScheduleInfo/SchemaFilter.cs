using Microsoft.OpenApi.Models;
using SportsCenter.Application.Schedule.Queries.GetScheduleInfo;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;

public class SchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type == typeof(ScheduleInfoBaseDto))
        {
            schema.Discriminator = new OpenApiDiscriminator { PropertyName = "Type" };
            schema.Properties.Add("Type", new OpenApiSchema { Type = "string" });
            schema.AnyOf = new List<OpenApiSchema>
            {
                context.SchemaGenerator.GenerateSchema(typeof(ScheduleInfoAdminDto), context.SchemaRepository),
                context.SchemaGenerator.GenerateSchema(typeof(ScheduleInfoTrainerDto), context.SchemaRepository),
                context.SchemaGenerator.GenerateSchema(typeof(ScheduleInfoBasicDto), context.SchemaRepository)
            };
        }
    }
}
