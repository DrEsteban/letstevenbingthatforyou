using System;
using Lmbtfy.Api.Services;
using Xunit;

namespace Lmbtfy.UnitTests.Services;

public class DailyKeywordServiceTests
{
    [Theory]
    [InlineData("2026-03-15", "nature")]
    [InlineData("2026-03-16", "city")]
    [InlineData("2026-03-17", "skyline")]
    [InlineData("2026-03-18", "island")]
    [InlineData("2026-03-19", "food")]
    [InlineData("2026-03-20", "airplanes")]
    [InlineData("2026-03-21", "technology")]
    public void GetDailyKeyword_UsesUtcDayOfWeek(string dateText, string expectedKeyword)
    {
        var service = new DailyKeywordService();

        var keyword = service.GetDailyKeyword(DateTime.Parse(dateText, null, System.Globalization.DateTimeStyles.AdjustToUniversal));

        Assert.Equal(expectedKeyword, keyword);
    }
}