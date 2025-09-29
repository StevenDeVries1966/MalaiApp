using System.Globalization;
using System.Text.RegularExpressions;

namespace Malai.UI.Helpers
{
    public class Assist
    {
        public static int GetIso8601WeekNumber(DateTime date)
        {
            // Using the ISO 8601 definition for week number
            CultureInfo culture = CultureInfo.InvariantCulture;
            System.Globalization.Calendar calendar = culture.Calendar;

            int weekNumber = calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            return weekNumber;
        }
        public static DateTime RoundToNearest(DateTime dateTime, TimeSpan interval)
        {
            long ticks = (dateTime.Ticks + (interval.Ticks / 2) + 1) / interval.Ticks;
            return new DateTime(ticks * interval.Ticks);
        }
        public static DateTime SetDateTime(DateTime date, string time)
        {
            int hour = Convert.ToInt32(time.Split(':')[0]);
            int minute = Convert.ToInt32(time.Split(':')[1]); ;

            // Create a DateTime object with the specified hour and minute
            DateTime dateTimeInput = new DateTime(date.Year, date.Month, date.Day, hour, minute, 0);
            return dateTimeInput;
        }
        public static string CalcHoursWorked(DateTime dateStart, DateTime dateEnd)
        {
            TimeSpan ts = dateEnd - dateStart;
            string time = $"{ts.Hours}{ts.Minutes}";
            return time;
        }
        public static bool IsValidTime(string input)
        {
            // Regular expression for time in hh:mm format
            string pattern = @"^(?:[01]\d|2[0-3]):[0-5]\d$";
            Regex regex = new Regex(pattern);

            return regex.IsMatch(input);
        }
    }
}
