using System.Reflection;
using System.Text;

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
        public static void WriteToCsv<T>(List<T> objects, string filePath)
        {
            try
            {
                // Create a StringBuilder to store the CSV data
                StringBuilder csvData = new StringBuilder();

                // Get the properties of the type T
                PropertyInfo[] properties = typeof(T).GetProperties();

                // Add header row to the CSV data
                csvData.AppendLine(string.Join(",", properties.Select(p => p.Name)));

                // Add data for each object in the list
                foreach (var obj in objects)
                {
                    // Get the values of the properties
                    var values = properties.Select(p => p.GetValue(obj));

                    // Format and add the values to the CSV data
                    csvData.AppendLine(string.Join(",", values.Select(v => v?.ToString())));
                }

                // Write the CSV data to the file
                File.WriteAllText(filePath, csvData.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
    }
}
