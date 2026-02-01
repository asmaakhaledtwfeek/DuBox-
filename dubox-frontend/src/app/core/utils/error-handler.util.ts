/**
 * Utility function to extract error message from HTTP error response
 * Handles various error formats including FluentValidation errors from .NET backend
 */
export function extractErrorMessage(error: any, defaultMessage: string = 'An error occurred'): string {
  if (!error) {
    return defaultMessage;
  }

  // If error is already a string
  if (typeof error === 'string') {
    return error;
  }

  // Check error.error (HTTP error response body)
  if (error.error) {
    // Direct message
    if (error.error.message) {
      return error.error.message;
    }

    // FluentValidation errors format: { errors: { "FieldName": ["error message"] } }
    if (error.error.errors && typeof error.error.errors === 'object') {
      const errors = error.error.errors;
      const allErrors: string[] = [];
      
      for (const field in errors) {
        if (Array.isArray(errors[field])) {
          allErrors.push(...errors[field]);
        } else if (typeof errors[field] === 'string') {
          allErrors.push(errors[field]);
        }
      }
      
      if (allErrors.length > 0) {
        return allErrors[0]; // Return first error, or join all: allErrors.join('. ')
      }
    }

    // Simple string error
    if (typeof error.error === 'string') {
      return error.error;
    }

    // Title from problem details
    if (error.error.title) {
      return error.error.title;
    }

    // Detail from problem details
    if (error.error.detail) {
      return error.error.detail;
    }
  }

  // Direct message on error object
  if (error.message) {
    return error.message;
  }

  // Status text
  if (error.statusText && error.statusText !== 'OK') {
    return error.statusText;
  }

  return defaultMessage;
}

/**
 * Extract all validation errors as an array
 */
export function extractAllValidationErrors(error: any): string[] {
  const errors: string[] = [];

  if (error?.error?.errors && typeof error.error.errors === 'object') {
    for (const field in error.error.errors) {
      if (Array.isArray(error.error.errors[field])) {
        errors.push(...error.error.errors[field]);
      } else if (typeof error.error.errors[field] === 'string') {
        errors.push(error.error.errors[field]);
      }
    }
  }

  if (errors.length === 0 && error?.error?.message) {
    errors.push(error.error.message);
  }

  if (errors.length === 0 && error?.message) {
    errors.push(error.message);
  }

  return errors;
}
