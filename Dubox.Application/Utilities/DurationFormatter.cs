namespace Dubox.Application.Utilities;

/// <summary>
/// Utility class for flexible duration calculation and formatting
/// </summary>
public static class DurationFormatter
{
    /// <summary>
    /// Calculate and format duration between two timestamps with flexible formatting:
    /// - Less than 1 hour: shows minutes (e.g., "45 minutes")
    /// - 1-23 hours: shows hours (e.g., "5.5 hours", "12 hours")
    /// - 24+ hours: shows days and hours (e.g., "2 days 5 hours")
    /// </summary>
    /// <param name="startDate">Start date/timestamp</param>
    /// <param name="endDate">End date/timestamp</param>
    /// <returns>Formatted duration string or null if dates are invalid</returns>
    public static string? FormatDuration(DateTime? startDate, DateTime? endDate)
    {
        if (!startDate.HasValue || !endDate.HasValue)
            return null;

        try
        {
            var start = startDate.Value;
            var end = endDate.Value;

            // Calculate difference in milliseconds
            var diffMs = (end - start).TotalMilliseconds;

            // If negative duration, return null (invalid)
            if (diffMs < 0)
                return null;

            // Calculate total hours
            var totalHours = (end - start).TotalHours;

            // If less than 24 hours, show in hours
            if (totalHours < 24)
            {
                if (totalHours < 1)
                {
                    // Less than 1 hour, show in minutes
                    var minutes = (int)Math.Round((end - start).TotalMinutes);
                    return minutes == 1 ? "1 minute" : $"{minutes} minutes";
                }

                // Show hours with 1 decimal place
                var hours = Math.Round(totalHours, 1);
                return hours == 1 ? "1 hour" : $"{hours} hours";
            }

            // If 24 hours or more, show days and hours
            var days = (int)Math.Floor(totalHours / 24);
            var remainingHours = (int)Math.Round(totalHours % 24);

            if (remainingHours == 0)
            {
                return days == 1 ? "1 day" : $"{days} days";
            }

            var daysText = days == 1 ? "1 day" : $"{days} days";
            var hoursText = remainingHours == 1 ? "1 hour" : $"{remainingHours} hours";
            return $"{daysText} {hoursText}";
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Calculate duration values between two timestamps
    /// </summary>
    /// <param name="startDate">Start date/timestamp</param>
    /// <param name="endDate">End date/timestamp</param>
    /// <returns>DurationValues object with days, hours, and total hours, or null if invalid</returns>
    public static DurationValues? CalculateDurationValues(DateTime? startDate, DateTime? endDate)
    {
        if (!startDate.HasValue || !endDate.HasValue)
            return null;

        try
        {
            var start = startDate.Value;
            var end = endDate.Value;

            var diffMs = (end - start).TotalMilliseconds;
            if (diffMs < 0)
                return null;

            var totalHours = (end - start).TotalHours;
            var days = (int)Math.Floor(totalHours / 24);
            var hours = (int)Math.Round(totalHours % 24);

            return new DurationValues
            {
                Days = days,
                Hours = hours,
                TotalHours = totalHours
            };
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Calculate duration in calendar days (legacy method for backward compatibility)
    /// Returns (end date - start date) + 1
    /// </summary>
    /// <param name="startDate">Start date</param>
    /// <param name="endDate">End date</param>
    /// <returns>Number of calendar days + 1, or null if dates are invalid</returns>
    public static int? CalculateDurationInDays(DateTime? startDate, DateTime? endDate)
    {
        if (!startDate.HasValue || !endDate.HasValue)
            return null;

        try
        {
            var start = startDate.Value;
            var end = endDate.Value;

            // Check if same calendar day (ignoring time)
            var startDateOnly = start.Date;
            var endDateOnly = end.Date;

            if (startDateOnly == endDateOnly)
            {
                return 1; // Same day = 1 day
            }

            // Calculate difference in days and add 1 for inclusive range
            var diff = (endDateOnly - startDateOnly).Days;
            var days = diff + 1;

            return days >= 1 ? days : 1;
        }
        catch
        {
            return null;
        }
    }
}

/// <summary>
/// Duration values structure
/// </summary>
public class DurationValues
{
    /// <summary>
    /// Full days (24-hour periods)
    /// </summary>
    public int Days { get; init; }

    /// <summary>
    /// Remaining hours (0-23)
    /// </summary>
    public int Hours { get; init; }

    /// <summary>
    /// Total duration in hours (including fractional hours)
    /// </summary>
    public double TotalHours { get; init; }
}

