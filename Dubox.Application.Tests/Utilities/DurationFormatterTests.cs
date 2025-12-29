using Dubox.Application.Utilities;
using Xunit;

namespace Dubox.Application.Tests.Utilities;

public class DurationFormatterTests
{
    [Fact]
    public void FormatDuration_LessThanOneHour_ReturnsMinutes()
    {
        // Arrange
        var start = new DateTime(2024, 1, 1, 10, 0, 0);
        var end = new DateTime(2024, 1, 1, 10, 30, 0);

        // Act
        var result = DurationFormatter.FormatDuration(start, end);

        // Assert
        Assert.Equal("30 minutes", result);
    }

    [Fact]
    public void FormatDuration_ExactlyOneMinute_ReturnsSingularMinute()
    {
        // Arrange
        var start = new DateTime(2024, 1, 1, 10, 0, 0);
        var end = new DateTime(2024, 1, 1, 10, 1, 0);

        // Act
        var result = DurationFormatter.FormatDuration(start, end);

        // Assert
        Assert.Equal("1 minute", result);
    }

    [Fact]
    public void FormatDuration_LessThan24Hours_ReturnsHours()
    {
        // Arrange
        var start = new DateTime(2024, 1, 1, 10, 0, 0);
        var end = new DateTime(2024, 1, 1, 15, 30, 0);

        // Act
        var result = DurationFormatter.FormatDuration(start, end);

        // Assert
        Assert.Equal("5.5 hours", result);
    }

    [Fact]
    public void FormatDuration_ExactlyOneHour_ReturnsSingularHour()
    {
        // Arrange
        var start = new DateTime(2024, 1, 1, 10, 0, 0);
        var end = new DateTime(2024, 1, 1, 11, 0, 0);

        // Act
        var result = DurationFormatter.FormatDuration(start, end);

        // Assert
        Assert.Equal("1 hour", result);
    }

    [Fact]
    public void FormatDuration_Exactly24Hours_ReturnsOneDay()
    {
        // Arrange
        var start = new DateTime(2024, 1, 1, 10, 0, 0);
        var end = new DateTime(2024, 1, 2, 10, 0, 0);

        // Act
        var result = DurationFormatter.FormatDuration(start, end);

        // Assert
        Assert.Equal("1 day", result);
    }

    [Fact]
    public void FormatDuration_MultipleDaysWithHours_ReturnsDaysAndHours()
    {
        // Arrange
        var start = new DateTime(2024, 1, 1, 10, 0, 0);
        var end = new DateTime(2024, 1, 3, 15, 0, 0);

        // Act
        var result = DurationFormatter.FormatDuration(start, end);

        // Assert
        Assert.Equal("2 days 5 hours", result);
    }

    [Fact]
    public void FormatDuration_MultipleDaysWithOneHour_ReturnsDaysAndSingularHour()
    {
        // Arrange
        var start = new DateTime(2024, 1, 1, 10, 0, 0);
        var end = new DateTime(2024, 1, 3, 11, 0, 0);

        // Act
        var result = DurationFormatter.FormatDuration(start, end);

        // Assert
        Assert.Equal("2 days 1 hour", result);
    }

    [Fact]
    public void FormatDuration_ExactDaysNoHours_ReturnsOnlyDays()
    {
        // Arrange
        var start = new DateTime(2024, 1, 1, 10, 0, 0);
        var end = new DateTime(2024, 1, 4, 10, 0, 0);

        // Act
        var result = DurationFormatter.FormatDuration(start, end);

        // Assert
        Assert.Equal("3 days", result);
    }

    [Fact]
    public void FormatDuration_NullStartDate_ReturnsNull()
    {
        // Arrange
        DateTime? start = null;
        var end = new DateTime(2024, 1, 1, 10, 0, 0);

        // Act
        var result = DurationFormatter.FormatDuration(start, end);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void FormatDuration_NullEndDate_ReturnsNull()
    {
        // Arrange
        var start = new DateTime(2024, 1, 1, 10, 0, 0);
        DateTime? end = null;

        // Act
        var result = DurationFormatter.FormatDuration(start, end);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void FormatDuration_NegativeDuration_ReturnsNull()
    {
        // Arrange
        var start = new DateTime(2024, 1, 2, 10, 0, 0);
        var end = new DateTime(2024, 1, 1, 10, 0, 0);

        // Act
        var result = DurationFormatter.FormatDuration(start, end);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void CalculateDurationValues_ValidDates_ReturnsCorrectValues()
    {
        // Arrange
        var start = new DateTime(2024, 1, 1, 10, 0, 0);
        var end = new DateTime(2024, 1, 3, 15, 0, 0);

        // Act
        var result = DurationFormatter.CalculateDurationValues(start, end);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Days);
        Assert.Equal(5, result.Hours);
        Assert.Equal(53, result.TotalHours);
    }

    [Fact]
    public void CalculateDurationValues_NullDates_ReturnsNull()
    {
        // Arrange
        DateTime? start = null;
        DateTime? end = null;

        // Act
        var result = DurationFormatter.CalculateDurationValues(start, end);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void CalculateDurationInDays_SameCalendarDay_ReturnsOne()
    {
        // Arrange
        var start = new DateTime(2024, 1, 1, 10, 0, 0);
        var end = new DateTime(2024, 1, 1, 15, 0, 0);

        // Act
        var result = DurationFormatter.CalculateDurationInDays(start, end);

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public void CalculateDurationInDays_DifferentDays_ReturnsCalendarDaysPlusOne()
    {
        // Arrange
        var start = new DateTime(2024, 1, 1, 10, 0, 0);
        var end = new DateTime(2024, 1, 3, 15, 0, 0);

        // Act
        var result = DurationFormatter.CalculateDurationInDays(start, end);

        // Assert
        Assert.Equal(3, result); // 2 days difference + 1 = 3
    }

    [Fact]
    public void CalculateDurationInDays_NullDates_ReturnsNull()
    {
        // Arrange
        DateTime? start = null;
        DateTime? end = null;

        // Act
        var result = DurationFormatter.CalculateDurationInDays(start, end);

        // Assert
        Assert.Null(result);
    }

    [Theory]
    [InlineData(0, 45, "45 minutes")]
    [InlineData(2, 30, "2.5 hours")]
    [InlineData(12, 0, "12 hours")]
    [InlineData(23, 30, "23.5 hours")]
    public void FormatDuration_VariousShortDurations_FormatsCorrectly(int hours, int minutes, string expected)
    {
        // Arrange
        var start = new DateTime(2024, 1, 1, 10, 0, 0);
        var end = start.AddHours(hours).AddMinutes(minutes);

        // Act
        var result = DurationFormatter.FormatDuration(start, end);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(1, 0, "1 day")]
    [InlineData(2, 0, "2 days")]
    [InlineData(1, 5, "1 day 5 hours")]
    [InlineData(5, 12, "5 days 12 hours")]
    public void FormatDuration_VariousLongDurations_FormatsCorrectly(int days, int hours, string expected)
    {
        // Arrange
        var start = new DateTime(2024, 1, 1, 10, 0, 0);
        var end = start.AddDays(days).AddHours(hours);

        // Act
        var result = DurationFormatter.FormatDuration(start, end);

        // Assert
        Assert.Equal(expected, result);
    }
}

