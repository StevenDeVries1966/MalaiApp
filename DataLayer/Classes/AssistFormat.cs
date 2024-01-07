namespace DataLayer.Classes
{
    public class AssistFormat
    {
        public static string ConvertMinutesToString(double minutesWorked)
        {
            int hours = (int)minutesWorked / 60;
            int minutesInHours = (int)hours * 60;
            int minutes = (int)minutesWorked - minutesInHours;
            string strminutes = minutes < 10 ? $"0{minutes}" : Convert.ToString(minutes);
            return $"{hours}:{strminutes}";
        }
        public static string ConvertHoursToString(double hours_worked)
        {
            int hours = (int)hours_worked;
            int minutes = (int)((hours_worked - hours) * 60);
            string strminutes = minutes < 10 ? $"0{minutes}" : Convert.ToString(minutes);
            return $"{hours}:{strminutes}";
        }
    }
}
