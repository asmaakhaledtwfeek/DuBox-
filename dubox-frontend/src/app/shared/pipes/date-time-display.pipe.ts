import { Pipe, PipeTransform } from '@angular/core';
import { formatDateWithTime } from '../../core/utils/date-format.util';

/**
 * Formats a date for display with date on first line and time (hours:minutes) on second line.
 * Use with CSS class "date-time-display" on the host element so the newline renders (white-space: pre-line).
 */
@Pipe({
  name: 'dateTimeDisplay',
  standalone: true
})
export class DateTimeDisplayPipe implements PipeTransform {
  transform(value: Date | string | null | undefined, emptyValue: string = '—'): string {
    return formatDateWithTime(value, emptyValue);
  }
}
