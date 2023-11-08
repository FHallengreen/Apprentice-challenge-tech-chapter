using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace Service.Tests;

public class HolidayTest
{
    private readonly HolidayCalendar _holidayCalendar;

    public HolidayTest()
    {
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<HolidayTest>()
            .Build();

        _holidayCalendar = new HolidayCalendar(configuration);
    }

    [Fact]
    public async Task GIVEN_XmasDay_WHEN_IsHoliday_THEN_return_true()
    {
        // Arrange
        var date = new DateTime(2023, 12, 25);

        // Act
        var result = await _holidayCalendar.IsHoliday(date);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task GIVEN_regular_weekday_WHEN_IsHoliday_THEN_return_false()
    {
        // Arrange
        var date = new DateTime(2023, 4, 21);

        // Act
        var result = await _holidayCalendar.IsHoliday(date);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GIVEN_April2023_WHEN_GetHolidays_THEN_return_EasterDays()
    {
        // Arrange
        var startDate = new DateTime(2023, 4, 1);
        var endDate = new DateTime(2023, 4, 30);

        // Act
        var result = await _holidayCalendar.GetHolidays(startDate, endDate);

        // Assert
        Assert.Contains(result, h => h.Date == new DateTime(2023, 4, 6).ToShortDateString()); // Maundy Thursday
        Assert.Contains(result, h => h.Date == new DateTime(2023, 4, 7).ToShortDateString()); // Good Friday
        Assert.Contains(result, h => h.Date == new DateTime(2023, 4, 9).ToShortDateString()); // Easter Sunday
        Assert.Contains(result, h => h.Date == new DateTime(2023, 4, 10).ToShortDateString()); // Easter Monday
        Assert.Equal(5, result.Count);

    }
}