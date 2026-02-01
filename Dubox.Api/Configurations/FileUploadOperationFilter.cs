using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Dubox.Api.Configurations;

public class FileUploadOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var fileParameters = context.MethodInfo.GetParameters()
            .Where(p => p.ParameterType == typeof(IFormFile) || 
                       (Nullable.GetUnderlyingType(p.ParameterType) == null && p.ParameterType.Name == "IFormFile"))
            .ToList();

        if (!fileParameters.Any())
            return;

        operation.RequestBody = new OpenApiRequestBody
        {
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["multipart/form-data"] = new OpenApiMediaType
                {
                    Schema = new OpenApiSchema
                    {
                        Type = "object",
                        Properties = new Dictionary<string, OpenApiSchema>(),
                        Required = new HashSet<string>()
                    }
                }
            }
        };

        var schema = operation.RequestBody.Content["multipart/form-data"].Schema;

        foreach (var fileParameter in fileParameters)
        {
            var parameterName = fileParameter.Name ?? "file";
            
            schema.Properties[parameterName] = new OpenApiSchema
            {
                Type = "string",
                Format = "binary",
                Description = "Upload file"
            };

            // Mark as required if parameter is not nullable
            var nullabilityInfo = new System.Reflection.NullabilityInfoContext().Create(fileParameter);
            if (nullabilityInfo.ReadState != System.Reflection.NullabilityState.Nullable)
            {
                schema.Required.Add(parameterName);
            }
        }

        // Add other parameters from the method
        var otherParameters = context.MethodInfo.GetParameters()
            .Where(p => p.ParameterType != typeof(IFormFile) && p.ParameterType.Name != "IFormFile")
            .Where(p => p.GetCustomAttributes(typeof(Microsoft.AspNetCore.Mvc.FromFormAttribute), false).Any())
            .ToList();

        foreach (var parameter in otherParameters)
        {
            var parameterName = parameter.Name ?? parameter.ParameterType.Name;
            schema.Properties[parameterName] = new OpenApiSchema
            {
                Type = "string"
            };
        }
    }
}

