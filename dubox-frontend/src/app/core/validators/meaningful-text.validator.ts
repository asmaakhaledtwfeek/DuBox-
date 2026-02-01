import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

/**
 * Validator to ensure text input is meaningful and not fake/spam
 */
export class MeaningfulTextValidator {
  private static readonly MINIMUM_LENGTH = 50;
  private static readonly MAX_REPEATED_CHAR_RATIO = 0.4; // Max 40% repeated characters
  private static readonly MAX_CONSECUTIVE_SAME_CHAR = 5;
  private static readonly MIN_UNIQUE_CHAR_RATIO = 0.15; // At least 15% unique characters
  private static readonly MIN_WORD_LENGTH_AVERAGE = 2.5; // Average word length should be at least 2.5 chars

  /**
   * Validator function for Angular forms
   */
  static validate(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const value = control.value;

      if (!value) {
        return null; // Let required validator handle empty values
      }

      const result = MeaningfulTextValidator.isValid(value);
      
      if (!result.isValid) {
        return { meaningfulText: { message: result.errorMessage } };
      }

      return null;
    };
  }

  /**
   * Validates if the text is meaningful and meets minimum requirements
   */
  static isValid(text: string | null | undefined): { isValid: boolean; errorMessage?: string } {
    // Check if null or empty
    if (!text || !text.trim()) {
      return {
        isValid: false,
        errorMessage: `This field is required and must contain at least ${this.MINIMUM_LENGTH} meaningful characters.`
      };
    }

    // Remove extra whitespace for validation
    const cleanedText = text.trim().replace(/\s+/g, ' ');

    // Check minimum length
    if (cleanedText.length < this.MINIMUM_LENGTH) {
      return {
        isValid: false,
        errorMessage: `This field must contain at least ${this.MINIMUM_LENGTH} characters. Current length: ${cleanedText.length} characters.`
      };
    }

    // Check for excessive repeated characters
    if (this.hasExcessiveRepeatedCharacters(cleanedText)) {
      return {
        isValid: false,
        errorMessage: 'The text contains too many repeated characters. Please provide meaningful content.'
      };
    }

    // Check for keyboard mashing patterns
    if (this.isKeyboardMashing(cleanedText)) {
      return {
        isValid: false,
        errorMessage: 'The text appears to contain random keyboard input. Please provide meaningful content.'
      };
    }

    // Check for insufficient unique characters
    if (!this.hasSufficientUniqueCharacters(cleanedText)) {
      return {
        isValid: false,
        errorMessage: 'The text lacks variety in characters. Please provide more detailed and meaningful content.'
      };
    }

    // Check for unrealistic word patterns
    if (!this.hasRealisticWordPattern(cleanedText)) {
      return {
        isValid: false,
        errorMessage: 'The text does not appear to contain realistic words. Please provide meaningful sentences.'
      };
    }

    return { isValid: true };
  }

  /**
   * Checks if text has too many repeated characters
   */
  private static hasExcessiveRepeatedCharacters(text: string): boolean {
    if (!text) return false;

    // Check for consecutive repeated characters
    let maxConsecutive = 1;
    let currentConsecutive = 1;

    for (let i = 1; i < text.length; i++) {
      if (text[i].toLowerCase() === text[i - 1].toLowerCase()) {
        currentConsecutive++;
        maxConsecutive = Math.max(maxConsecutive, currentConsecutive);
      } else {
        currentConsecutive = 1;
      }
    }

    if (maxConsecutive > this.MAX_CONSECUTIVE_SAME_CHAR) {
      return true;
    }

    // Check overall repeated character ratio
    const charCount = new Map<string, number>();
    const alphanumericChars = text.toLowerCase().split('').filter(c => /[a-z0-9]/.test(c));

    alphanumericChars.forEach(c => {
      charCount.set(c, (charCount.get(c) || 0) + 1);
    });

    if (charCount.size > 0) {
      const totalChars = alphanumericChars.length;
      const maxCharCount = Math.max(...Array.from(charCount.values()));
      const ratio = maxCharCount / totalChars;

      if (ratio > this.MAX_REPEATED_CHAR_RATIO) {
        return true;
      }
    }

    return false;
  }

  /**
   * Checks if text appears to be keyboard mashing
   */
  private static isKeyboardMashing(text: string): boolean {
    const lowerText = text.toLowerCase();

    // Common keyboard mashing patterns
    const mashingPatterns = [
      'asdf', 'qwer', 'zxcv', 'hjkl', 'uiop',
      'jklj', 'asdasd', 'qweqwe', '123123', 'abcabc',
      'aaaa', 'bbbb', '1111', '2222'
    ];

    // Check if text contains repeated mashing patterns
    for (const pattern of mashingPatterns) {
      const regex = new RegExp(pattern, 'g');
      const matches = lowerText.match(regex);
      if (matches && matches.length >= 3) {
        return true;
      }
    }

    // Check for alternating character patterns
    if (text.length >= 10) {
      for (let i = 0; i < text.length - 9; i++) {
        const chunk = text.substring(i, i + 10);
        if (this.isAlternatingPattern(chunk)) {
          return true;
        }
      }
    }

    return false;
  }

  /**
   * Checks if a chunk of text is an alternating pattern
   */
  private static isAlternatingPattern(chunk: string): boolean {
    if (chunk.length < 4) return false;

    const pattern = chunk.substring(0, 2);
    const expectedPattern = pattern.repeat(Math.floor(chunk.length / 2));

    return chunk.startsWith(expectedPattern.substring(0, Math.min(expectedPattern.length, chunk.length)));
  }

  /**
   * Checks if text has sufficient unique characters
   */
  private static hasSufficientUniqueCharacters(text: string): boolean {
    const alphanumericChars = text.split('').filter(c => /[a-z0-9]/i.test(c));
    if (alphanumericChars.length === 0) return false;

    const uniqueChars = new Set(alphanumericChars.map(c => c.toLowerCase()));
    const ratio = uniqueChars.size / alphanumericChars.length;

    return ratio >= this.MIN_UNIQUE_CHAR_RATIO;
  }

  /**
   * Checks if text has realistic word patterns
   */
  private static hasRealisticWordPattern(text: string): boolean {
    // Split into words
    const words = text.split(/\s+/)
      .filter(w => w.trim() && /[a-z]/i.test(w));

    if (words.length < 5) {
      return false;
    }

    // Check average word length
    const avgWordLength = words.reduce((sum, w) => sum + w.length, 0) / words.length;
    if (avgWordLength < this.MIN_WORD_LENGTH_AVERAGE) {
      return false;
    }

    // Check for variety in word lengths
    const uniqueWordLengths = new Set(words.map(w => w.length));
    if (uniqueWordLengths.size < 2) {
      return false;
    }

    return true;
  }

  /**
   * Gets the minimum required length
   */
  static getMinimumLength(): number {
    return this.MINIMUM_LENGTH;
  }
}
