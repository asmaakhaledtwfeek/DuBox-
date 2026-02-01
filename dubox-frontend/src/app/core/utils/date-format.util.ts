/**
 * Formats a date for display with date on first line and time (hours:minutes) on second line.
 * Use with CSS class "date-time-display" (white-space: pre-line) to show on two lines.
 * @param date - Date, string (ISO), or null/undefined
 * @param emptyValue - Value to return when date is null, undefined, or invalid (default: '—')
 */
export function formatDateWithTime(
  date?: Date | string | null,
  emptyValue: string = '—'
): string {
  if (date == null) {
    return emptyValue;
  }
  const parsed = date instanceof Date ? date : new Date(date);
  if (isNaN(parsed.getTime())) {
    return emptyValue;
  }
  const datePart = parsed.toLocaleDateString('en-US', {
    year: 'numeric',
    month: 'short',
    day: 'numeric'
  });
  const timePart = parsed.toLocaleTimeString('en-US', {
    hour: 'numeric',
    minute: '2-digit',
    hour12: true
  });
  return `${datePart}\n${timePart}`;
}
