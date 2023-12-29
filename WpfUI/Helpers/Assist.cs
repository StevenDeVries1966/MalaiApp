using System;
using System.Globalization;

namespace WpfUI.Helpers
{
    public class Assist
    {
        public static int GetIso8601WeekNumber(DateTime date)
        {
            // Using the ISO 8601 definition for week number
            CultureInfo culture = CultureInfo.InvariantCulture;
            Calendar calendar = culture.Calendar;

            int weekNumber = calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            return weekNumber;
        }
    }
}
