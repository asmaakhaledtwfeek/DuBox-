namespace Dubox.Domain.Shared;


public sealed class ValidationResult<TValue> : Result<TValue>, IValidationResult
{
    private ValidationResult(TValue value, string[] errors)
        : base(value, errors.Length > 0 ? errors[0] : "Validation failed")
    {
        ErrorMessages = errors;
    }
    public string[] ErrorMessages { get; }

    public static ValidationResult<TValue> WithErrors(TValue value, string[] errors) => new ValidationResult<TValue>(value, errors);
}
