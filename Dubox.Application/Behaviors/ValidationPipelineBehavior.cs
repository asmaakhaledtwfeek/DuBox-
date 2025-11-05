using Dubox.Domain.Shared;
using FluentValidation;
using MediatR;


namespace Dubox.Application.Behaviors
{
    public class ValidationPipelineBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (!_validators.Any())
            {
                return await next();
            }

            var validationTasks = _validators
                .Select(validator => validator.ValidateAsync(request, cancellationToken))
                .ToList();

            await Task.WhenAll(validationTasks);

            var errors = validationTasks
                .SelectMany(validationTask => validationTask.Result.Errors)
                .Where(validationFailure => validationFailure != null)
                .Select(failure
                        => $"{failure.PropertyName} {failure.ErrorMessage}")
                .Distinct()
                .ToArray();

            if (errors.Any())
            {
                return CreateValidationResult<TResponse>(errors);
            }

            return await next();
        }

        private static TResult CreateValidationResult<TResult>(string[] errorMessages)
            where TResult : Result
        {
            if (typeof(TResult) == typeof(Result))
            {
                return (ValidationResult.WithErrors(errorMessages) as TResult)!;
            }

            var valueType = typeof(TResult).GenericTypeArguments[0];
            var defaultValue = valueType.IsValueType ? Activator.CreateInstance(valueType) : null;

            object validationResult = typeof(ValidationResult<>)
                .GetGenericTypeDefinition()
                .MakeGenericType(valueType)
                .GetMethod(nameof(ValidationResult.WithErrors))!
                .Invoke(null, new object?[] { defaultValue, errorMessages })!;

            return (TResult)validationResult;
        }
    }
}
