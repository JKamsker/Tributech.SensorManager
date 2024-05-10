using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

using Tributech.SensorManager.Application.Sensors.Queries.Common;

namespace Tributech.SensorManager.Api.SchemaFilters;

// SensorMetadataVm filter
public class SensorMetadataVmSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        var isSensorMetadataVm = context.Type == typeof(IEnumerable<SensorMetadataVm>)
            || context.Type.IsAssignableTo(typeof(IEnumerable<SensorMetadataVm>));

        if (!isSensorMetadataVm)
        {
            return;
        }
        /*
            Instead of:
            [
                {
                    "key": "temperature",
                    "value": "75°F"
                },
                {
                    "key": "humidity",
                    "value": "50%"
                }
            ]
            We have:
            {
                "temperature": "75°F",
                "humidity": "50%"
            }

         */

        schema.Type = "object";
        schema.Properties = new Dictionary<string, OpenApiSchema>
        {
            {
                "somekey",
                new OpenApiSchema
                {
                    Type = "string",
                    Example = new OpenApiString("somevalue")
                }
            }
        };
    }
}