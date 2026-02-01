namespace Dubox.Application.Utilities;


public static class DurationFormatter
{
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

            // If remainingHours rounds to 24, convert it to an additional day
            if (remainingHours == 24)
            {
                days += 1;
                remainingHours = 0;
            }

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

            // If hours rounds to 24, convert it to an additional day
            if (hours == 24)
            {
                days += 1;
                hours = 0;
            }

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

public class DurationValues
{
    public int Days { get; init; }
    public int Hours { get; init; }
    public double TotalHours { get; init; }
}

