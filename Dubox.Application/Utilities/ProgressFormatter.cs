namespace Dubox.Application.Utilities;


public static class ProgressFormatter
{
    public static string FormatProgress(decimal? value)
    {
        if (!value.HasValue)
        {
            return "-";
        }

        var rounded = Math.Round(value.Value, 2, MidpointRounding.AwayFromZero);
        return $"{rounded:F2}%";
    }
}
