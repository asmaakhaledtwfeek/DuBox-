/**
 * Converts all characters to uppercase
 * Example: "gf" => "GF", "1f" => "1F"
 * @param text The text to convert to uppercase
 * @returns The text in uppercase
 */
export function toUpperCase(text: string | null | undefined): string {
  if (!text) return '';
  return text.toUpperCase().trim();
}

/**
 * Capitalizes the first letter of each word in a string
 * Example: "zone a" => "Zone A", "john doe" => "John Doe"
 * @param text The text to capitalize
 * @returns The text with each word capitalized
 */
export function toTitleCase(text: string | null | undefined): string {
  if (!text) return '';
  
  return text
    .toLowerCase()
    .split(' ')
    .map(word => {
      if (word.length === 0) return word;
      return word.charAt(0).toUpperCase() + word.slice(1);
    })
    .join(' ')
    .trim();
}

/**
 * Capitalizes the first letter of each word in a string, preserving existing case for already capitalized letters
 * Useful for names that might have mixed case (like "McDonald")
 * Example: "zone a" => "Zone A", "john doe" => "John Doe", "mcDonald" => "McDonald"
 * @param text The text to capitalize
 * @returns The text with first letter of each word capitalized
 */
export function toTitleCasePreserve(text: string | null | undefined): string {
  if (!text) return '';
  
  return text
    .split(' ')
    .map(word => {
      if (word.length === 0) return word;
      // Only capitalize if the word is all lowercase
      if (word === word.toLowerCase()) {
        return word.charAt(0).toUpperCase() + word.slice(1);
      }
      // Otherwise, capitalize first letter and preserve the rest
      return word.charAt(0).toUpperCase() + word.slice(1);
    })
    .join(' ')
    .trim();
}

