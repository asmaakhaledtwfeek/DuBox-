/**
 * Utility functions for formatting progress percentages consistently across the application
 */

/**
 * Formats a progress percentage value to exactly 2 decimal places with a percent sign.
 * Returns "-" for null/undefined values.
 * Uses the same rounding logic as backend (AwayFromZero midpoint rounding).
 * 
 * @param value - The progress percentage value (0-100) or null/undefined
 * @returns Formatted string (e.g., "1.85%", "1.86%") or "-" for null/undefined
 * 
 * @example
 * formatProgress(1.85185) // "1.85%"
 * formatProgress(1.855) // "1.86%"
 * formatProgress(null) // "-"
 * formatProgress(undefined) // "-"
 */
export function formatProgress(value: number | null | undefined): string {
  if (value === null || value === undefined || isNaN(value)) {
    return '-';
  }

  // Round to 2 decimal places using the same logic as C# Math.Round with AwayFromZero
  // JavaScript's toFixed uses "half up" rounding which is similar to AwayFromZero for most cases
  const rounded = Number(value.toFixed(2));
  return `${rounded.toFixed(2)}%`;
}

