using System.Text.Json;
using FluentValidation.Results;

namespace Malmasp.Dtos;

public class Payload
{
    public object? Data { get; set; }
    public object? Error { get; set; }

    public static Payload FromValidationResult(ValidationResult validationResult)
    {
        Payload payload = new();
        
        if (validationResult.IsValid)
        {
            return payload;
        }

        var error = new Dictionary<string, string>();
        validationResult.Errors.ForEach(f =>
        {
            error.TryAdd(
                JsonNamingPolicy.CamelCase.ConvertName(f.PropertyName),
                f.ErrorMessage);
        });
        payload.Error = error;
        
        return payload;
    }
}