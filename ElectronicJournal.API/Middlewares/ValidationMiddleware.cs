using ElectronicJournal.API.Validators;
using FluentValidation;
using FluentValidation.Results;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace ElectronicJournal.API.Middlewares
{
    public class ValidationMiddleware : IMiddleware
    {
        private readonly ILogger<ValidationMiddleware> _logger;
        public ValidationMiddleware(ILogger<ValidationMiddleware> logger)
        {
            _logger = logger;
        }

        private string GetControllerName(string? visiblePartOfName)
            => "ElectronicJournal.API.Controllers." + visiblePartOfName + "Controller";

        private string GetValidatorName(string? controllerName, string? typeName)
            => "ElectronicJournal.API.Validators." + controllerName + typeName + "Validator";

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            _logger.LogInformation("ValidationMiddleware");
            HttpRequest request = context.Request;
            string? controllerName = request.RouteValues[key: "controller"]?.ToString();
            Type? controller = Type.GetType(typeName: GetControllerName(visiblePartOfName: controllerName));
            string? actionName = request.RouteValues[key: "action"]?.ToString();
            MethodInfo? action = controller?.GetMethod(name: actionName, bindingAttr: BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            Type? typeInRequest = action?.GetParameters().FirstOrDefault()?.ParameterType;

            if (typeInRequest is null || !typeInRequest.Name.Contains("Request"))
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
                    TypeConverter converter = TypeDescriptor.GetConverter(type: typeOfProperty);
                    if (converter is null || !converter.CanConvertFrom(typeof(String)))
                    {
                        ValidationError err = new ValidationError { ErrorMessage = $"Не удалось конвертировать значение {property.Key} из {typeof(String)} в {typeInRequest.Name}", PropertyName = "data" };
                        await WriteBadRequestAsync(context, new[] { err });
                        return;
                    }

                    try
                    {
                        typeInRequest.GetProperty(name: property.Key)?.SetValue(obj: obj, value: converter.ConvertFrom(property.Value));
                    }
                    catch (Exception ex)
                    {
                        await WriteBadRequestAsync(context, new[] { new ValidationError { ErrorMessage = ex.Message, PropertyName = "data" } });
                        return;
                    }

                }
            }

            if (obj is null)
            {
                await WriteBadRequestAsync(context, new[] { new ValidationError { ErrorMessage = "Отсутствует необходимое содержимое", PropertyName = "data" } });
                return;
            }

            Type? typeOfValidator = Type.GetType(typeName: GetValidatorName(controllerName: controllerName, typeName: typeInRequest.Name));
            if (typeOfValidator == null)
            {
                await next(context);
                _logger.LogWarning(message: $"[ValidationMiddleware]: Отсутствует валидатор для типа {typeInRequest.Name}");
                return;
            }

            IValidator? validator = (IValidator?)Activator.CreateInstance(type: typeOfValidator);
            ValidationResult? validationResult = validator?.Validate(context: new ValidationContext<object>(instanceToValidate: obj));
            if (!(validationResult?.IsValid ?? true))
            {
                await WriteBadRequestAsync(context, validationResult?.Errors.Select(selector: e => new ValidationError { ErrorMessage = e.ErrorMessage, PropertyName = e.PropertyName }));
                return;
            }

            await next(context);
        }

        private static async Task WriteBadRequestAsync(HttpContext context, IEnumerable<ValidationError> err = null)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(text: JsonConvert.SerializeObject(value: new { Errors = err }));
        }

        private class ValidationError
        {
            public string ErrorMessage { get; set; }
            public string PropertyName { get; set; }
        }
    }

    public static class ValidateMiddlewareExtension
    {
        public static IApplicationBuilder UseValidationMiddleware(this IApplicationBuilder app)
            => app.UseMiddleware<ValidationMiddleware>();
    }
}
