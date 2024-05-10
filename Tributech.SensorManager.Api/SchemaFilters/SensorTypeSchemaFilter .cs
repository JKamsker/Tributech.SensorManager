using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

using Tributech.SensorManager.Domain.ValueTypes;

namespace Tributech.SensorManager.Api.SchemaFilters;

public class SensorTypeSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type == typeof(SensorType))
        {
            schema.Type = "string";
            schema.Example = new OpenApiString("example-sensor-type");
        }
    }
}