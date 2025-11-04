namespace Dubox.Domain.Shared;

public sealed class ValidationResult<TValue> : Result<TValue>, IValidationResult
{
    private ValidationResult(TValue value, string[] errors)
        : base(value, false, errors.Length > 0 ? errors[0] : "Validation failed",
               new Error("Validation.Error", errors.Length > 0 ? errors[0] : "Validation failed"))
    {
        ErrorMessages = errors;
    }

    public string[] ErrorMessages { get; }

    public static ValidationResult<TValue> WithErrors(TValue value, string[] errors) => new(value, errors);
}
