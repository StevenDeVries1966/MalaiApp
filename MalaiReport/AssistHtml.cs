﻿using DataLayer.Classes;
using System.Text;

namespace MalaiReport
{
    public class AssistHtml
    {
        public static string CreateHtml_Report_IMC(DtoClient client, string period, List<DtoWorkedHoursReport> whs, string Es001MinutesReportTotal, string As001MinutesReportTotal)
        {
            double total_charge = whs.Sum(item => item.Charge);
            StringBuilder htmlBuilder = new StringBuilder();

            // Start HTML table

            DoRateHeader0(client, period, htmlBuilder);



            htmlBuilder.AppendLine(@"<br>");

            htmlBuilder.AppendLine("<table 'border:5px solid black;border-collapse:collapse;'>");

            // Create table header
            htmlBuilder.AppendLine(@"<tr>");
            htmlBuilder.AppendLine("<th>Date</th>");
            htmlBuilder.AppendLine("<th>Desciption</th>");
            htmlBuilder.AppendLine("<th>Esther</th>");
            htmlBuilder.AppendLine("<th>Ashia</th>");
            htmlBuilder.AppendLine("<th>HR - Hours</th>");
            htmlBuilder.AppendLine("<th>Total Hours</th>");
            htmlBuilder.AppendLine("<th>Carge</th>");
            htmlBuilder.AppendLine("</tr>");

            // Create table rows
            foreach (DtoWorkedHoursReport wh in whs)
            {
                htmlBuilder.AppendLine("<tr>");
                htmlBuilder.AppendLine($"<td>{wh.Today.ToString("dd-MMM-yyyy").Replace(".", "")}</td>");
                htmlBuilder.AppendLine($"<td>{wh.JobTotal}</td>");
                htmlBuilder.AppendLine($"<td>{wh.Es001MinutesTotalString}</td>");
                htmlBuilder.AppendLine($"<td>{wh.As001MinutesTotalString}</td>");
                htmlBuilder.AppendLine($"<td></td>");
                htmlBuilder.AppendLine($"<td>{wh.MinutesTotalString}</td>");
                htmlBuilder.AppendLine($"<td align='right'>${wh.Charge.ToString("0.00")}</td>");
                htmlBuilder.AppendLine("</tr>");
            }
            htmlBuilder.AppendLine(@"<tr>");
            htmlBuilder.AppendLine("<th></th>");
            htmlBuilder.AppendLine("<th></th>");
            htmlBuilder.AppendLine($"<th>{Es001MinutesReportTotal}</th>");
            htmlBuilder.AppendLine($"<th>{As001MinutesReportTotal}</th>");
            htmlBuilder.AppendLine($"<th></th>");
            htmlBuilder.AppendLine("<th>Total Hours</th>");
            htmlBuilder.AppendLine($"<th>$ {total_charge.ToString("0.00")}</th>");
            htmlBuilder.AppendLine("</tr>");
            // End HTML table
            htmlBuilder.AppendLine("</table>");

            //htmlBuilder.AppendLine($"<label>Total : {Math.Round(total_charge, 2)}</label>");

            return htmlBuilder.ToString();
        }

        public static string CreateHtml_Report_CHP(DtoClient client,
                                                   string period,
                                                   List<DtoWorkedHoursReport> whs,
                                                   string Es001MinutesReportTotal,
                                                   string As001MinutesReportTotal,
                                                   int Es001PercentageReportTotal,
                                                   int As001PercentageReportTotal)
        {

            double total_charge = whs.Sum(item => item.Charge);
            double Es001_total_charge = whs.Sum(item => item.ChargeEs001);
            double As001_total_charge = whs.Sum(item => item.ChargeAs001);
            string MinutesReportTotal = AssistFormat.ConvertMinutesToString(whs.Sum(item => item.MinutesTotal));
            StringBuilder htmlBuilder = new StringBuilder();

            // Start HTML table

            DoRateHeader0(client, period, htmlBuilder);



            htmlBuilder.AppendLine(@"<br>");

            htmlBuilder.AppendLine("<table 'border:5px solid black;border-collapse:collapse;'>");

            htmlBuilder.AppendLine("<tr>");
            htmlBuilder.AppendLine($"<td></td>");
            htmlBuilder.AppendLine($"<td>Accounting</td>");
            htmlBuilder.AppendLine($"<td>Assistant</td>");
            htmlBuilder.AppendLine($"<td>Totals</td>");
            htmlBuilder.AppendLine("</tr>");
            // Create table header
            htmlBuilder.AppendLine(@"<tr>");
            htmlBuilder.AppendLine("<th>Date</th>");
            htmlBuilder.AppendLine("<th>Rate 1 - Hours</th>");
            htmlBuilder.AppendLine("<th>Rate 2 - Hours</th>");
            htmlBuilder.AppendLine("<th>Hours</th>");
            htmlBuilder.AppendLine("</tr>");

            // Create table rows
            foreach (DtoWorkedHoursReport wh in whs)
            {
                htmlBuilder.AppendLine("<tr>");
                htmlBuilder.AppendLine($"<td>{wh.Today.ToString("dd-MMM-yyyy").Replace(".", "")}</td>");
                htmlBuilder.AppendLine($"<td>{wh.Es001MinutesTotalString}</td>");
                htmlBuilder.AppendLine($"<td>{wh.As001MinutesTotalString}</td>");
                htmlBuilder.AppendLine($"<td>{wh.MinutesTotalString}</td>");
                htmlBuilder.AppendLine("</tr>");
            }
            htmlBuilder.AppendLine(@"<tr>");
            htmlBuilder.AppendLine("<th></th>");
            htmlBuilder.AppendLine($"<th>{Es001MinutesReportTotal}</th>");
            htmlBuilder.AppendLine($"<th>{As001MinutesReportTotal}</th>");
            htmlBuilder.AppendLine($"<th>{MinutesReportTotal}</th>");
            htmlBuilder.AppendLine("</tr>");

            htmlBuilder.AppendLine(@"<tr>");
            htmlBuilder.AppendLine("<td></td>");
            htmlBuilder.AppendLine($"<td>{Es001PercentageReportTotal}%</td>");
            htmlBuilder.AppendLine($"<td>{As001PercentageReportTotal}%</td>");
            htmlBuilder.AppendLine($"<td></td>");
            htmlBuilder.AppendLine("</tr>");

            htmlBuilder.AppendLine(@"<tr>");
            htmlBuilder.AppendLine($"<th>Total charge {period}</th>");
            htmlBuilder.AppendLine($"<th>${Es001_total_charge.ToString("0.00")}</th>");
            htmlBuilder.AppendLine($"<th>${As001_total_charge.ToString("0.00")}</th>");
            htmlBuilder.AppendLine($"<th>${total_charge.ToString("0.00")}</th>");
            htmlBuilder.AppendLine("</tr>");
            // End HTML table
            htmlBuilder.AppendLine("</table>");

            //htmlBuilder.AppendLine($"<label>Total : {Math.Round(total_charge, 2)}</label>");

            return htmlBuilder.ToString();
        }
        public static string CreateHtml_Report_Retainer(DtoClient client,
                                           string period,
                                           List<DtoWorkedHoursReport> whs,
                                           string Es001MinutesReportTotal,
                                           string As001MinutesReportTotal,
                                           int Es001PercentageReportTotal,
                                           int As001PercentageReportTotal)
        {

            double total_charge = whs.Sum(item => item.Charge);
            double Es001_total_charge = whs.Sum(item => item.ChargeEs001);
            double As001_total_charge = whs.Sum(item => item.ChargeAs001);
            double Es001_ReportTotal = whs.Sum(item => item.Es001MinutesTotal);
            double As001_ReportTotal = whs.Sum(item => item.As001MinutesTotal);

            double Es001_Additional = Es001_ReportTotal - (client.retainer_ES001 * 60);
            double As001_Additional = As001_ReportTotal - (client.retainer_AS001 * 60);
            double Total_Additional = Es001_Additional + As001_Additional;
            string MinutesReportTotal = AssistFormat.ConvertMinutesToString(whs.Sum(item => item.MinutesTotal));
            StringBuilder htmlBuilder = new StringBuilder();
            DoNameHeader(client, period, htmlBuilder);

            htmlBuilder.AppendLine(@"<br>");
            // Start HTML table
            htmlBuilder.AppendLine("<table 'border:5px solid black;border-collapse:collapse;'>");
            // Create table header
            htmlBuilder.AppendLine(@"<tr>");
            htmlBuilder.AppendLine("<th>Date</th>");
            htmlBuilder.AppendLine("<th>Desciption</th>");
            htmlBuilder.AppendLine("<th>Esther</th>");
            htmlBuilder.AppendLine("<th>Aihsa</th>");
            htmlBuilder.AppendLine("<th>TotalHours</th>");
            htmlBuilder.AppendLine("</tr>");

            // Create table rows
            foreach (DtoWorkedHoursReport wh in whs)
            {
                htmlBuilder.AppendLine("<tr>");
                htmlBuilder.AppendLine($"<td>{wh.Today.ToString("dd-MMM-yyyy").Replace(".", "")}</td>");
                htmlBuilder.AppendLine($"<td>{wh.JobTotal}</td>");
                htmlBuilder.AppendLine($"<td>{wh.Es001MinutesTotalString}</td>");
                htmlBuilder.AppendLine($"<td>{wh.As001MinutesTotalString}</td>");
                htmlBuilder.AppendLine($"<td>{wh.MinutesTotalString}</td>");
                htmlBuilder.AppendLine("</tr>");
            }
            htmlBuilder.AppendLine(@"<tr>");
            htmlBuilder.AppendLine("<th></th>");
            htmlBuilder.AppendLine("<th></th>");
            htmlBuilder.AppendLine($"<th>{Es001MinutesReportTotal}</th>");
            htmlBuilder.AppendLine($"<th>{As001MinutesReportTotal}</th>");
            htmlBuilder.AppendLine($"<th>{MinutesReportTotal}</th>");
            htmlBuilder.AppendLine("</tr>");
            htmlBuilder.AppendLine("<tr>");
            htmlBuilder.AppendLine($"<td></td>");
            htmlBuilder.AppendLine($"<td>Retainer</td>");
            htmlBuilder.AppendLine($"<td>{AssistFormat.ConvertHoursToString(client.retainer_ES001)}</td>");
            htmlBuilder.AppendLine($"<td>{AssistFormat.ConvertHoursToString(client.retainer_AS001)}</td>");
            htmlBuilder.AppendLine($"<td>{AssistFormat.ConvertHoursToString(client.retainer_ES001 + client.retainer_AS001)}</td>");
            htmlBuilder.AppendLine("</tr>");
            htmlBuilder.AppendLine("<tr>");
            htmlBuilder.AppendLine($"<td></td>");
            htmlBuilder.AppendLine($"<td>Additional hours</td>");
            htmlBuilder.AppendLine($"<td>{AssistFormat.ConvertMinutesToString(Es001_Additional)}</td>");
            htmlBuilder.AppendLine($"<td>{AssistFormat.ConvertMinutesToString(As001_Additional)}</td>");
            htmlBuilder.AppendLine($"<td>{AssistFormat.ConvertMinutesToString(Total_Additional)}</td>");
            htmlBuilder.AppendLine("</tr>");
            htmlBuilder.AppendLine("</table>");
            htmlBuilder.AppendLine(@"<br>");
            return htmlBuilder.ToString();
        }
        private static void DoNameHeader(DtoClient client, string period, StringBuilder htmlBuilder)
        {
            htmlBuilder.AppendLine("<table 'border:5px solid black;border-collapse:collapse;'>");
            htmlBuilder.AppendLine("<tr>");
            htmlBuilder.AppendLine($"<td><img src=\"{Globals.ImagePath}\" alt=\"Your Image\" width=\"200\" height=\"200\"></td>");
            htmlBuilder.AppendLine($"<td><table>");
            htmlBuilder.AppendLine("<tr>");
            htmlBuilder.AppendLine($"<td>Client : {client.clt_name}</td>");
            htmlBuilder.AppendLine("</tr>");
            htmlBuilder.AppendLine("<tr>");
            htmlBuilder.AppendLine($"<td>Period : {period}</td>");
            htmlBuilder.AppendLine("</tr>");
            htmlBuilder.AppendLine($"</table></td>");
            htmlBuilder.AppendLine("</tr>");
            htmlBuilder.AppendLine("</table>");
        }
        private static void DoRateHeader0(DtoClient client, string period, StringBuilder htmlBuilder)
        {
            htmlBuilder.AppendLine("<table 'border:5px solid black;border-collapse:collapse;'>");
            htmlBuilder.AppendLine("<tr>");
            htmlBuilder.AppendLine($"<td><img src=\"{Globals.ImagePath}\" alt=\"Your Image\" width=\"200\" height=\"200\"></td>");
            htmlBuilder.AppendLine($"<td>");
            DoRateHeader1(client, period, htmlBuilder);
            htmlBuilder.AppendLine($"</td>");
            htmlBuilder.AppendLine("</tr>");
            htmlBuilder.AppendLine("</table>");
        }
        private static void DoRateHeader1(DtoClient client, string period, StringBuilder htmlBuilder)
        {
            if (!client.rate_ES001.Equals(client.rate_AS001))
            {
                htmlBuilder.AppendLine("<table 'border:5px solid black;border-collapse:collapse;'>");
                htmlBuilder.AppendLine("<tr>");
                htmlBuilder.AppendLine($"<td>Client : {client.clt_name}</td>");
                htmlBuilder.AppendLine("</tr>");
                htmlBuilder.AppendLine("<tr>");
                htmlBuilder.AppendLine($"<td>Period : {period}</td>");
                htmlBuilder.AppendLine($"<td>Rate 1: $ {client.rate_ES001}</td>");
                htmlBuilder.AppendLine("</tr>");
                htmlBuilder.AppendLine("<tr>");
                htmlBuilder.AppendLine($"<td></td>");
                htmlBuilder.AppendLine($"<td>Rate 2: $ {client.rate_AS001}</td>");
                htmlBuilder.AppendLine("</tr>");
                htmlBuilder.AppendLine("</table>");
            }
            else
            {
                htmlBuilder.AppendLine("<table 'border:5px solid black;border-collapse:collapse;'>");
                htmlBuilder.AppendLine("<tr>");
                htmlBuilder.AppendLine($"<td>Client : {client.clt_name}</td>");
                htmlBuilder.AppendLine($"<td></td>");
                htmlBuilder.AppendLine("</tr>");
                htmlBuilder.AppendLine("<tr>");
                htmlBuilder.AppendLine($"<td>Period : {period}</td>");
                htmlBuilder.AppendLine($"<td>Hourly Rate: $ {client.rate_ES001}</td>");
                htmlBuilder.AppendLine("</tr>");
                htmlBuilder.AppendLine("</table>");
            }
        }

        public static string GetHtmlResourceContent(string resourceName)
        {
            string content = "";

            // Read the content of the embedded resource
            using (StreamReader reader = new StreamReader(resourceName))
            {
                content = reader.ReadToEnd();
            }

            return content;
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
