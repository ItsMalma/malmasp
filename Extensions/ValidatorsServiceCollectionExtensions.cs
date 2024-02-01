using FluentValidation;
using Malmasp.Dtos;
using Malmasp.Validators;

namespace Malmasp.Extensions;

public static class ValidatorsServiceCollectionExtensions
{
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddSingleton<IValidator<UserRequestDto>, UserRequestDtoValidator>();
        
        return services;
    }
}