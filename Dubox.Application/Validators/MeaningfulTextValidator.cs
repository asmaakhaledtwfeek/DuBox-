using System.Text.RegularExpressions;

namespace Dubox.Application.Validators
{
    public static class MeaningfulTextValidator
    {
        private const int MinimumLength = 50;

        public static bool IsValid(string? text, out string errorMessage)
        {
            errorMessage = string.Empty;

            // Check if null or empty
            if (string.IsNullOrWhiteSpace(text))
            {
                errorMessage = $"This field is required and must contain at least {MinimumLength} characters.";
                return false;
            }

            // Remove extra whitespace for validation
            var cleanedText = Regex.Replace(text.Trim(), @"\s+", " ");

            // Check minimum length
            if (cleanedText.Length < MinimumLength)
            {
                errorMessage = $"This field must contain at least {MinimumLength} characters. Current length: {cleanedText.Length} characters.";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Gets the minimum required length
        /// </summary>
        public static int GetMinimumLength() => MinimumLength;
    }
}
