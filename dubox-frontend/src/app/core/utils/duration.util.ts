/**
 * Utility functions for duration calculations and formatting
 */

/**
 * Calculate duration between two timestamps and return a formatted string
 * - If < 24 hours: shows "X hours" or "X.Y hours"
 * - If >= 24 hours: shows "X days Y hours"
 * 
 * @param startDate - Start date/timestamp
 * @param endDate - End date/timestamp
 * @returns Formatted duration string or undefined if dates are invalid
 */
export function calculateAndFormatDuration(
  startDate?: Date | string,
  endDate?: Date | string
): string | undefined {
  if (!startDate || !endDate) return undefined;

  try {
    const start = typeof startDate === 'string' ? new Date(startDate) : startDate;
    const end = typeof endDate === 'string' ? new Date(endDate) : endDate;

    if (isNaN(start.getTime()) || isNaN(end.getTime())) return undefined;

    // Calculate difference in milliseconds
    const diffMs = end.getTime() - start.getTime();
    
    // If negative duration, return undefined (invalid)
    if (diffMs < 0) return undefined;

    // Calculate total hours
    const totalHours = diffMs / (1000 * 60 * 60);

    // If less than 24 hours, show in hours
    if (totalHours < 24) {
      if (totalHours < 1) {
        // Less than 1 hour, show in minutes
        const minutes = Math.round(diffMs / (1000 * 60));
        return minutes === 1 ? '1 minute' : `${minutes} minutes`;
      }
      // Show hours with 1 decimal place
      const hours = Math.round(totalHours * 10) / 10;
      return hours === 1 ? '1 hour' : `${hours} hours`;
    }

    // If 24 hours or more, show days and hours
    const days = Math.floor(totalHours / 24);
    const remainingHours = Math.round(totalHours % 24);

    if (remainingHours === 0) {
      return days === 1 ? '1 day' : `${days} days`;
    }

    const daysText = days === 1 ? '1 day' : `${days} days`;
    const hoursText = remainingHours === 1 ? '1 hour' : `${remainingHours} hours`;
    return `${daysText} ${hoursText}`;
  } catch {
    return undefined;
  }
}

/**
 * Calculate raw duration values between two timestamps
 * Returns an object with days, hours, and total hours
 * 
 * @param startDate - Start date/timestamp
 * @param endDate - End date/timestamp
 * @returns Object with duration values or undefined
 */
export function calculateDurationValues(
  startDate?: Date | string,
  endDate?: Date | string
): { days: number; hours: number; totalHours: number } | undefined {
  if (!startDate || !endDate) return undefined;

  try {
    const start = typeof startDate === 'string' ? new Date(startDate) : startDate;
    const end = typeof endDate === 'string' ? new Date(endDate) : endDate;

    if (isNaN(start.getTime()) || isNaN(end.getTime())) return undefined;

    const diffMs = end.getTime() - start.getTime();
    if (diffMs < 0) return undefined;

    const totalHours = diffMs / (1000 * 60 * 60);
    const days = Math.floor(totalHours / 24);
    const hours = Math.round(totalHours % 24);

    return { days, hours, totalHours };
  } catch {
    return undefined;
  }
}

/**
 * Legacy function for backward compatibility: Calculate duration in days only
 * This matches the old behavior (calendar days + 1)
 * 
 * @param startDate - Start date
 * @param endDate - End date
 * @returns Number of days or undefined
 */
export function calculateDurationInDays(
  startDate?: Date | string,
  endDate?: Date | string
): number | undefined {
  if (!startDate || !endDate) return undefined;

  try {
    const start = typeof startDate === 'string' ? new Date(startDate) : startDate;
    const end = typeof endDate === 'string' ? new Date(endDate) : endDate;

    if (isNaN(start.getTime()) || isNaN(end.getTime())) return undefined;

    // Check if same calendar day (ignoring time)
    const startDateOnly = new Date(start.getFullYear(), start.getMonth(), start.getDate());
    const endDateOnly = new Date(end.getFullYear(), end.getMonth(), end.getDate());

    if (startDateOnly.getTime() === endDateOnly.getTime()) {
      return 1; // Same day = 1 day
    }

    // Calculate difference in days and add 1 for inclusive range
    const diff = endDateOnly.getTime() - startDateOnly.getTime();
    const days = Math.ceil(diff / (1000 * 60 * 60 * 24)) + 1;
    
    return days >= 1 ? days : 1;
  } catch {
    return undefined;
  }
}

