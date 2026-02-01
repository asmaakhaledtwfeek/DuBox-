namespace Dubox.Domain.Shared;

public interface IValidationResult
{
    string[] ErrorMessages { get; }
}
