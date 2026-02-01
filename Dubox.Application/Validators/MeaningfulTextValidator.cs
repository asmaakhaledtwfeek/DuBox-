using System.Text.RegularExpressions;

namespace Dubox.Application.Validators
{
    public static class MeaningfulTextValidator
    {
        private const int MinimumLength = 50;
        private const double MaxRepeatedCharRatio = 0.4; // Max 40% repeated characters
        private const int MaxConsecutiveSameChar = 5;
        private const double MinUniqueCharRatio = 0.15; // At least 15% unique characters
        private const double MinWordLengthAverage = 2.5; // Average word length should be at least 2.5 chars

      
        public static bool IsValid(string? text, out string errorMessage)
        {
            errorMessage = string.Empty;

            // Check if null or empty
            if (string.IsNullOrWhiteSpace(text))
            {
                errorMessage = $"This field is required and must contain at least {MinimumLength} meaningful characters.";
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

            // Check for excessive repeated characters (e.g., "aaaaaaa", "111111")
            if (HasExcessiveRepeatedCharacters(cleanedText))
            {
                errorMessage = "The text contains too many repeated characters. Please provide meaningful content.";
                return false;
            }

            // Check for keyboard mashing patterns (e.g., "asdfasdfasdf", "qwertyqwerty")
            if (IsKeyboardMashing(cleanedText))
            {
                errorMessage = "The text appears to contain random keyboard input. Please provide meaningful content.";
                return false;
            }

            // Check for insufficient unique characters
            if (!HasSufficientUniqueCharacters(cleanedText))
            {
                errorMessage = "The text lacks variety in characters. Please provide more detailed and meaningful content.";
                return false;
            }

            // Check for unrealistic word patterns
            if (!HasRealisticWordPattern(cleanedText))
            {
                errorMessage = "The text does not appear to contain realistic words. Please provide meaningful sentences.";
                return false;
            }

            return true;
        }

        private static bool HasExcessiveRepeatedCharacters(string text)
        {
            if (string.IsNullOrEmpty(text)) return false;

            // Check for consecutive repeated characters
            int maxConsecutive = 1;
            int currentConsecutive = 1;

            for (int i = 1; i < text.Length; i++)
            {
                if (char.ToLower(text[i]) == char.ToLower(text[i - 1]))
                {
                    currentConsecutive++;
                    maxConsecutive = Math.Max(maxConsecutive, currentConsecutive);
                }
                else
                {
                    currentConsecutive = 1;
                }
            }

            if (maxConsecutive > MaxConsecutiveSameChar)
                return true;

            // Check overall repeated character ratio
            var charCount = new Dictionary<char, int>();
            foreach (var c in text.ToLower().Where(char.IsLetterOrDigit))
            {
                charCount[c] = charCount.GetValueOrDefault(c, 0) + 1;
            }

            if (charCount.Any())
            {
                var totalChars = charCount.Values.Sum();
                var maxCharCount = charCount.Values.Max();
                var ratio = (double)maxCharCount / totalChars;

                if (ratio > MaxRepeatedCharRatio)
                    return true;
            }

            return false;
        }

      
        private static bool IsKeyboardMashing(string text)
        {
            var lowerText = text.ToLower();

            // Common keyboard mashing patterns
            var mashingPatterns = new[]
            {
                "asdf", "qwer", "zxcv", "hjkl", "uiop",
                "jklj", "asdasd", "qweqwe", "123123", "abcabc",
                "aaaa", "bbbb", "1111", "2222"
            };

            // Check if text contains repeated mashing patterns
            foreach (var pattern in mashingPatterns)
            {
                var patternCount = Regex.Matches(lowerText, Regex.Escape(pattern)).Count;
                if (patternCount >= 3) // Pattern appears 3+ times
                    return true;
            }

            // Check for alternating character patterns (e.g., "ababababab")
            if (text.Length >= 10)
            {
                for (int i = 0; i < text.Length - 9; i++)
                {
                    var chunk = text.Substring(i, 10);
                    if (IsAlternatingPattern(chunk))
                        return true;
                }
            }

            return false;
        }

        
        private static bool IsAlternatingPattern(string chunk)
        {
            if (chunk.Length < 4) return false;

            // Check if it's a 2-character alternating pattern (e.g., "abababab")
            var pattern = chunk.Substring(0, 2);
            var expectedPattern = string.Concat(Enumerable.Repeat(pattern, chunk.Length / 2));
            
            return chunk.StartsWith(expectedPattern.Substring(0, Math.Min(expectedPattern.Length, chunk.Length)));
        }

        private static bool HasSufficientUniqueCharacters(string text)
        {
            var alphanumericChars = text.Where(char.IsLetterOrDigit).ToList();
            if (alphanumericChars.Count == 0) return false;

            var uniqueChars = alphanumericChars.Select(char.ToLower).Distinct().Count();
            var ratio = (double)uniqueChars / alphanumericChars.Count;

            return ratio >= MinUniqueCharRatio;
        }

        /// <summary>
        /// Checks if text has realistic word patterns
        /// </summary>
        private static bool HasRealisticWordPattern(string text)
        {
            // Split into words
            var words = Regex.Split(text, @"\s+")
                .Where(w => !string.IsNullOrWhiteSpace(w) && w.Any(char.IsLetter))
                .ToList();

            if (words.Count < 5) // Should have at least 5 words for 50+ characters
                return false;

            // Check average word length
            var avgWordLength = words.Average(w => w.Length);
            if (avgWordLength < MinWordLengthAverage)
                return false;

            // Check for at least some variety in word lengths
            var uniqueWordLengths = words.Select(w => w.Length).Distinct().Count();
            if (uniqueWordLengths < 2) // Should have at least 2 different word lengths
                return false;

            return true;
        }

        /// <summary>
        /// Gets the minimum required length
        /// </summary>
        public static int GetMinimumLength() => MinimumLength;
    }
}
