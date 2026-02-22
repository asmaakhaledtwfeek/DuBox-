import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

/**
 * Validator to ensure text input meets minimum length requirement
 */
export class MeaningfulTextValidator {
  private static readonly MINIMUM_LENGTH = 5;

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
   * Validates if the text meets minimum length requirement
   */
  static isValid(text: string | null | undefined): { isValid: boolean; errorMessage?: string } {
    // Check if null or empty
    if (!text || !text.trim()) {
      return {
        isValid: false,
        errorMessage: `This field is required and must contain at least ${this.MINIMUM_LENGTH} characters.`
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

    return { isValid: true };
  }

  /**
   * Gets the minimum required length
   */
  static getMinimumLength(): number {
    return this.MINIMUM_LENGTH;
  }
}
