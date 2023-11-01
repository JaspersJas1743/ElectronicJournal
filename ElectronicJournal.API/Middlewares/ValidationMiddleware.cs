using FluentValidation;
using FluentValidation.Results;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace ElectronicJournal.API.Middlewares
{
    public class ValidationMiddleware : IMiddleware
    {
        private string GetControllerName(string? visiblePartOfName)
            => "ElectronicJournal.API.Controllers." + visiblePartOfName + "Controller";

        private string GetValidatorName(string? controllerName, string? typeName)
            => "ElectronicJournal.API.Validators." + controllerName + typeName + "Validator";

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            HttpRequest request = context.Request;
            string? controllerName = request.RouteValues[key: "controller"]?.ToString();
            Type? controller = Type.GetType(typeName: GetControllerName(visiblePartOfName: controllerName));
            string? actionName = request.RouteValues[key: "action"]?.ToString();
            MethodInfo? action = controller?.GetMethod(name: actionName, bindingAttr: BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            Type? typeInRequest = action?.GetParameters().FirstOrDefault()?.ParameterType;
            if (typeInRequest is null)
            {
                await next(context);
                return;
            }

            object? obj = FormatterServices.GetUninitializedObject(type: typeInRequest);

            if (request.Query.Count.Equals(0))
            {
                using (StreamReader reader = new StreamReader(stream: request.Body))
                {
                    var requestBody = await reader.ReadToEndAsync();
                    obj = JsonConvert.DeserializeObject(value: requestBody, type: typeInRequest, settings: new JsonSerializerSettings
                    {
                        Formatting = Formatting.Indented,
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    });
                    request.Body = new MemoryStream(buffer: Encoding.UTF8.GetBytes(s: requestBody));
                }
            }
            else
            {
                Dictionary<string, string?> properties = request.Query.ToDictionary(keySelector: k => k.Key, v => v.Value.First());
                foreach (KeyValuePair<string, string?> property in properties)
                {
                    Type? typeOfProperty = typeInRequest.GetProperty(name: property.Key)?.PropertyType;
                    typeInRequest.GetProperty(name: property.Key)?.SetValue(obj: obj, value: property.Value);
                }
            }

            Type? typeOfValidator = Type.GetType(typeName: GetValidatorName(controllerName: controllerName, typeName: typeInRequest.Name));
            IValidator? validator = (IValidator?)Activator.CreateInstance(type: typeOfValidator);
            ValidationResult? validationResult = validator?.Validate(context: new ValidationContext<object>(instanceToValidate: obj));
            if (!(validationResult?.IsValid ?? true))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new { Errors = validationResult.Errors.Select(e => new { e.ErrorMessage, e.PropertyName }) }));
                return;
            }

            await next(context);
        }
    }

    public static class ValidateMiddlewareExtension
    {
        public static IApplicationBuilder UseValidationMiddleware(this IApplicationBuilder app)
            => app.UseMiddleware<ValidationMiddleware>();
    }
}
