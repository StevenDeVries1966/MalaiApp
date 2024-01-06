using DataLayer.Classes;
using System.Text;

namespace MalaiReport
{
    public class AssistHtml
    {
        public static string ConvertListToHtmlDataGrid(List<DtoWorkedHoursReport> whs, double total_charge)
        {
            StringBuilder htmlBuilder = new StringBuilder();

            // Start HTML table
            htmlBuilder.AppendLine("<table border='1'>");

            // Create table header
            htmlBuilder.AppendLine("<tr>");
            htmlBuilder.AppendLine("<th>Date</th>");
            htmlBuilder.AppendLine("<th>Desciption</th>");
            htmlBuilder.AppendLine("<th>Esther</th>");
            htmlBuilder.AppendLine("<th>Ashia</th>");
            htmlBuilder.AppendLine("<th>Total Hours</th>");
            htmlBuilder.AppendLine("<th>Carge</th>");
            htmlBuilder.AppendLine("</tr>");

            // Create table rows
            foreach (DtoWorkedHoursReport wh in whs)
            {
                htmlBuilder.AppendLine("<tr>");
                htmlBuilder.AppendLine($"<td>{wh.today.ToString("dd-MM-yyyy")}</td>");
                htmlBuilder.AppendLine($"<td>{wh.job_total}</td>");
                htmlBuilder.AppendLine($"<td>{wh.esther_minutes_total_string}</td>");
                htmlBuilder.AppendLine($"<td>{wh.aisha_minutes_total_string}</td>");
                htmlBuilder.AppendLine($"<td>{wh.minutes_total_string}</td>");
                htmlBuilder.AppendLine($"<td>{wh.charge}</td>");
                htmlBuilder.AppendLine("</tr>");
            }

            // End HTML table
            htmlBuilder.AppendLine("</table>");

            htmlBuilder.AppendLine($"<label>Total : {Math.Round(total_charge, 2)}</label>");

            return htmlBuilder.ToString();
        }
        public static string GenerateHtmlTable(object[] data)
        {
            StringBuilder html = new StringBuilder();

            // Start HTML table
            html.AppendLine("<table border='1'>");

            // Create table header
            html.AppendLine("<tr>");
            foreach (var prop in data[0].GetType().GetProperties())
            {
                html.AppendLine($"<th>{prop.Name}</th>");
            }
            html.AppendLine("</tr>");

            // Create table rows
            foreach (var item in data)
            {
                html.AppendLine("<tr>");
                foreach (var prop in item.GetType().GetProperties())
                {
                    html.AppendLine($"<td>{prop.GetValue(item)}</td>");
                }
                html.AppendLine("</tr>");
            }

            // End HTML table
            html.AppendLine("</table>");

            return html.ToString();
        }

        public static void SaveHtmlToFile(string htmlContent, string fileName)
        {
            File.WriteAllText(fileName, htmlContent);
        }
    }
}
