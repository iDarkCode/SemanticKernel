using Microsoft.SemanticKernel;

namespace SemanticKernel.Plugins;

public class TimePlugin
{
    [KernelFunction("get_time")]
    public static string GetTime()
    {
        return DateTime.Now.ToShortTimeString();
    }

    [KernelFunction("get_days_until_christmas")]
    public static string GetDaysUntilChristmas(DateTime dateTime)
    {
        DateTime christmas = new DateTime(dateTime.Year, 12, 25).ToUniversalTime();
        if (dateTime > christmas)
        {
            christmas = christmas.AddYears(1);
        }
        TimeSpan timeUntilChristmas = christmas - dateTime;
        return timeUntilChristmas.Days.ToString();
    }

}