using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Shop.API.Helpers;

public class AddFormFileParameterOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var formFileParams = context.ApiDescription.ParameterDescriptions
            .Where(p => p.ParameterDescriptor.ParameterType == typeof(IFormFile));

        foreach (var formFileParam in formFileParams)
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = formFileParam.Name,
                In = ParameterLocation.Path,
                Description = "File Upload",
                Required = true,
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Format = "binary"
                }
            });
    }
}