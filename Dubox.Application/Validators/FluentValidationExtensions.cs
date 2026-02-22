using FluentValidation;

namespace Dubox.Application.Validators
{
    
    public static class FluentValidationExtensions
    {
      
        public static IRuleBuilderOptions<T, string?> MustBeMeaningfulText<T>(
            this IRuleBuilder<T, string?> ruleBuilder)
        {
            return ruleBuilder.Must(value =>
            {
                if (string.IsNullOrWhiteSpace(value))
                    return false;
                return MeaningfulTextValidator.IsValid(value, out _);
            }).WithMessage("This field must contain at least 50 characters.");
        }

     
        public static IRuleBuilderOptions<T, string?> MustBeMeaningfulTextWhenProvided<T>(
            this IRuleBuilder<T, string?> ruleBuilder)
        {
            return ruleBuilder.Must(value =>
            {
                // Only validate if value is provided
                if (string.IsNullOrWhiteSpace(value))
                    return true; // Optional field - empty is OK
                return MeaningfulTextValidator.IsValid(value, out _);
            }).WithMessage("When provided, this field must contain at least 5 characters.");
        }
    }
}
