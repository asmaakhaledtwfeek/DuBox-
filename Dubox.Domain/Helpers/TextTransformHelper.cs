using System.Globalization;

namespace Dubox.Domain.Helpers;

public static class TextTransformHelper
{
    /// <summary>
    /// Converts all characters to uppercase
    /// Example: "gf" => "GF", "1f" => "1F"
    /// </summary>
    /// <param name="text">The text to convert to uppercase</param>
    /// <returns>The text in uppercase</returns>
    public static string ToUpperCase(string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return string.Empty;

        return text.ToUpper().Trim();
    }

    /// <summary>
    /// Capitalizes the first letter of each word in a string
    /// Example: "zone a" => "Zone A", "john doe" => "John Doe"
    /// </summary>
    /// <param name="text">The text to capitalize</param>
    /// <returns>The text with each word capitalized</returns>
    public static string ToTitleCase(string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return string.Empty;

        var textInfo = CultureInfo.CurrentCulture.TextInfo;
        return textInfo.ToTitleCase(text.ToLower().Trim());
    }

    /// <summary>
    /// Capitalizes the first letter of each word in a string, preserving existing case for already capitalized letters
    /// Example: "zone a" => "Zone A", "McDonald" => "McDonald"
    /// </summary>
    /// <param name="text">The text to capitalize</param>
    /// <returns>The text with first letter of each word capitalized</returns>
    public static string ToTitleCasePreserve(string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return string.Empty;

        var words = text.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var result = new List<string>();

        foreach (var word in words)
        {
            if (word.Length == 0)
                continue;

            // Only capitalize if the word is all lowercase
            if (word == word.ToLower())
            {
                result.Add(char.ToUpper(word[0]) + word.Substring(1));
            }
            else
            {
                // Preserve existing case, just ensure first letter is capitalized
                result.Add(char.ToUpper(word[0]) + word.Substring(1));
            }
        }

        return string.Join(" ", result);
    }
}

