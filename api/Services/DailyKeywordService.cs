namespace Lmbtfy.Api.Services;

public class DailyKeywordService
{
    private static readonly string[] KeywordList =
    {
        "nature",
        "city",
        "skyline",
        "island",
        "food",
        "airplanes",
        "technology"
    };

    public string GetDailyKeyword(DateTime? utcNow = null)
    {
        var currentUtc = utcNow ?? DateTime.UtcNow;
        var weekDay = (int)currentUtc.DayOfWeek;

        return KeywordList[weekDay];
    }
}