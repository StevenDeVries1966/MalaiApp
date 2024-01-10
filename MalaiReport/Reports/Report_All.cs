using DataLayer.Classes;
using MalaiReport.Helpers;
using System.Text;

namespace MalaiReport.Reports
{
    public class ReportAll
    {
        public List<DtoWorkedHours> LstWorkedHours { get; set; }
        public List<DtoWorkedHoursReport> LstWorkedHoursReports { get; set; }
        public DtoClient Client { get; set; }
        public string HtmlContent { get; set; }

        public int IntEs001MinutesReportTotal => Convert.ToInt32(LstWorkedHoursReports.Sum(o => o.Es001MinutesTotal));
        public int IntEs001PercentageReportTotal => Convert.ToInt32(Math.Round((double)(((double)IntEs001MinutesReportTotal / (double)IntMinutesReportTotal) * 100), 0));

        public int IntAs001MinutesReportTotal => Convert.ToInt32(LstWorkedHoursReports.Sum(o => o.As001MinutesTotal));
        public int IntAs001PercentageReportTotal => Convert.ToInt32(Math.Round((double)(((double)IntAs001MinutesReportTotal / (double)IntMinutesReportTotal) * 100), 0));
        public int IntMinutesReportTotal => IntEs001MinutesReportTotal + IntAs001MinutesReportTotal;

        public string StrEs001MinutesReportTotal =>
            AssistFormat.ConvertMinutesToString(IntEs001MinutesReportTotal);

        public string StrAs001MinutesReportTotal =>
            AssistFormat.ConvertMinutesToString(IntAs001MinutesReportTotal);
        public string StrMinutesReportTotal =>
            AssistFormat.ConvertMinutesToString(IntMinutesReportTotal);

        public double Es001_total_charge { get; set; }
        public double As001_total_charge { get; set; }
        public StringBuilder HtmlBuilder { get; set; }
        public string MonthString { get; set; }
        public string Period { get; set; }
        public IEnumerable<IGrouping<DateTime, DtoWorkedHours>> GroupedByDate { get; set; }
        public string HtmlTemplateContent { get; set; }
        public double TotalCharge { get; set; }
        public bool InclCharge { get; set; }


        public ReportAll(int month, int year, string clt_code, string htmlFilePath, bool inclCharge = true)
        {
            string message = "";
            try
            {
                InclCharge = inclCharge;
                MonthString = new DateTime(DateTime.Now.Year, month, 1).ToString("MMMM");
                Period = $"{MonthString} {year}";
                LstWorkedHours = Globals.ConMan.GetDataClientMonth<DtoWorkedHours>("GetDataClientMonth", month, year, clt_code, out message);
                Client = Globals.ConMan.lstClients.FirstOrDefault(o => o.clt_code.Equals(clt_code, StringComparison.CurrentCultureIgnoreCase));
                if (LstWorkedHours.Count == 0)
                {
                    Console.WriteLine($"No hours for {clt_code} in {month}-{year}");
                    return;
                }
                if (Client == null)
                {
                    Console.WriteLine($"This client is not found in the database: {clt_code} in {month}-{year}");
                    return;
                }

                GroupedByDate = LstWorkedHours.GroupBy(obj => obj.start_time.Date);
                LstWorkedHoursReports = new List<DtoWorkedHoursReport>();
                foreach (var groep in GroupedByDate)
                {
                    LstWorkedHoursReports.Add(new DtoWorkedHoursReport(Client, groep.ToList(), groep.Key.Date));
                }
                TotalCharge = LstWorkedHoursReports.Sum(item => item.Charge);
                Es001_total_charge = LstWorkedHoursReports.Sum(item => item.ChargeEs001);
                As001_total_charge = LstWorkedHoursReports.Sum(item => item.ChargeAs001);

                HtmlTemplateContent = AssistHtml.GetHtmlResourceContent(Globals.HtmlTemplatePath);
                // Generate the HTML content
                if (Client.report_type.Equals("A", StringComparison.CurrentCultureIgnoreCase))
                {
                    HtmlContent = CreateHtml_Report_IMC();
                }
                else if (Client.report_type.Equals("B", StringComparison.CurrentCultureIgnoreCase))
                {
                    HtmlContent = CreateHtml_Report_CHP();
                }
                else if (Client.report_type.Equals("C", StringComparison.CurrentCultureIgnoreCase))
                {
                    HtmlContent = CreateHtml_Report_Retainer();
                }

                HtmlContent = HtmlTemplateContent.Replace("%Content%", HtmlContent);

                // PHC && InclCharge = true
                if (!Client.clt_code.Equals("IMC", StringComparison.CurrentCultureIgnoreCase) && InclCharge)
                {
                    htmlFilePath = Path.Combine(htmlFilePath, $"{clt_code}_Report{Client.report_type}_{month}{year}.html");
                }
                // IMC && InclCharge = true
                else if (Client.clt_code.Equals("IMC", StringComparison.CurrentCultureIgnoreCase) && InclCharge)
                {
                    htmlFilePath = Path.Combine(htmlFilePath, $"{clt_code}_Report{Client.report_type}_{month}{year}.html");
                }
                // IMC && InclCharge = false
                else if (Client.clt_code.Equals("IMC", StringComparison.CurrentCultureIgnoreCase) && !InclCharge)
                {
                    htmlFilePath = Path.Combine(htmlFilePath, $"{clt_code}_Report{Client.report_type}_{month}{year}_PLUS.html");
                }
                else
                {

                }



                // Save HTML content to a file
                AssistHtml.SaveHtmlToFile(HtmlContent, htmlFilePath);

                AssistFormat.WriteToCsv(LstWorkedHours, htmlFilePath.Replace(".html", ".txt"));
            }
            catch (Exception e)
            {
                Globals.ConMan.AddLog(e.Message, e!.StackTrace!, Globals.EmployeeCurrent!.emp_id);
            }
        }
        private string CreateHtml_Report_IMC()
        {

            HtmlBuilder = new StringBuilder();
            // Start HTML table
            DoRateHeader0();
            HtmlBuilder.AppendLine(@"<br>");
            HtmlBuilder.AppendLine("<table  class=\"data\" 'border:5px solid black;border-collapse:collapse;'>");
            HtmlBuilder.AppendLine("<tr>");
            HtmlBuilder.AppendLine($"<td colspan=\"2\"></td>");
            HtmlBuilder.AppendLine($"<td colspan=\"2\">IMC - AP</td>");
            HtmlBuilder.AppendLine($"<td>IMC - HR</td>");
            if (InclCharge)
            {
                HtmlBuilder.AppendLine($"<td colspan=\"3\"></td>");
            }
            else
            {
                //HtmlBuilder.AppendLine($"<td></td>");
            }

            HtmlBuilder.AppendLine("</tr>");

            // Create table header
            HtmlBuilder.AppendLine(@"<tr>");
            HtmlBuilder.AppendLine("<th>Date</th>");
            HtmlBuilder.AppendLine("<th>Desciption</th>");
            HtmlBuilder.AppendLine("<th>Esther</th>");
            HtmlBuilder.AppendLine("<th>Ashia</th>");
            HtmlBuilder.AppendLine("<th>HR - Hours</th>");
            HtmlBuilder.AppendLine("<th style=\"border-right-style:none;\">Total Hours</th>");
            if (InclCharge)
            {
                HtmlBuilder.AppendLine("<th style=\"border-left-style:none;border-right-style:none;\"></th>");
                HtmlBuilder.AppendLine("<th style=\"border-left-style: none;text-align:right;\">Carge</th>");
            }

            HtmlBuilder.AppendLine("</tr>");

            // Create table rows
            foreach (DtoWorkedHoursReport wh in LstWorkedHoursReports)
            {
                HtmlBuilder.AppendLine("<tr>");
                HtmlBuilder.AppendLine($"<td>{wh.Today.ToString("dd-MMM-yyyy").Replace(".", "")}</td>");
                HtmlBuilder.AppendLine($"<td>{wh.JobTotal}</td>");
                HtmlBuilder.AppendLine($"<td>{wh.Es001MinutesTotalString}</td>");
                HtmlBuilder.AppendLine($"<td>{wh.As001MinutesTotalString}</td>");
                HtmlBuilder.AppendLine($"<td></td>");
                HtmlBuilder.AppendLine($"<td style=\"border-right-style:none;\">{wh.MinutesTotalString}</td>");
                if (InclCharge)
                {
                    HtmlBuilder.AppendLine($"<td style=\"border-left-style:none;border-right-style:none;font-weight:bold;\">$</td>");
                    HtmlBuilder.AppendLine($"<td style=\"border-left-style:none;text-align:right;font-weight:bold;\">{wh.Charge.ToString("0.00")}</td>");
                }
                HtmlBuilder.AppendLine("</tr>");
            }
            HtmlBuilder.AppendLine(@"<tr>");
            HtmlBuilder.AppendLine("<th></th>");
            HtmlBuilder.AppendLine("<th></th>");
            HtmlBuilder.AppendLine($"<th>{StrEs001MinutesReportTotal}</th>");
            HtmlBuilder.AppendLine($"<th>{StrAs001MinutesReportTotal}</th>");
            HtmlBuilder.AppendLine($"<th></th>");
            HtmlBuilder.AppendLine($"<th style=\"border-right-style:none;\">{StrMinutesReportTotal}</th>");
            if (InclCharge)
            {
                HtmlBuilder.AppendLine($"<th style=\"border-left-style:none;border-right-style:none;\">$</th>");
                HtmlBuilder.AppendLine($"<th style=\"border-left-style: none;text-align:right;\">{TotalCharge.ToString("0.00")}</th>");
            }
            HtmlBuilder.AppendLine("</tr>");
            // End HTML table
            HtmlBuilder.AppendLine("</table>");

            //htmlBuilder.AppendLine($"<label>Total : {Math.Round(total_charge, 2)}</label>");

            return HtmlBuilder.ToString();
        }
        private string CreateHtml_Report_CHP()
        {
            HtmlBuilder = new StringBuilder();

            // Start HTML table

            DoRateHeader0();
            HtmlBuilder.AppendLine(@"<br>");

            HtmlBuilder.AppendLine("<table class=\"data\" 'border:5px solid black;border-collapse:collapse;'>");
            HtmlBuilder.AppendLine("<tr>");
            HtmlBuilder.AppendLine($"<td></td>");
            HtmlBuilder.AppendLine($"<td style=\"color:#275D5D\">Accounting</td>");
            HtmlBuilder.AppendLine($"<td style=\"color:#275D5D\">Assistant</td>");
            HtmlBuilder.AppendLine($"<td style=\"color:#275D5D\">Totals</td>");
            HtmlBuilder.AppendLine("</tr>");
            // Create table header
            HtmlBuilder.AppendLine(@"<tr>");
            HtmlBuilder.AppendLine("<th>Date</th>");
            HtmlBuilder.AppendLine("<th>Rate 1 - Hours</th>");
            HtmlBuilder.AppendLine("<th>Rate 2 - Hours</th>");
            HtmlBuilder.AppendLine("<th>Hours</th>");
            HtmlBuilder.AppendLine("</tr>");

            // Create table rows
            foreach (DtoWorkedHoursReport wh in LstWorkedHoursReports)
            {
                HtmlBuilder.AppendLine("<tr>");
                HtmlBuilder.AppendLine($"<td>{wh.Today.ToString("dd-MMM-yyyy").Replace(".", "")}</td>");
                HtmlBuilder.AppendLine($"<td>{wh.Es001MinutesTotalString}</td>");
                HtmlBuilder.AppendLine($"<td>{wh.As001MinutesTotalString}</td>");
                HtmlBuilder.AppendLine($"<td>{wh.MinutesTotalString}</td>");
                HtmlBuilder.AppendLine("</tr>");
            }
            HtmlBuilder.AppendLine(@"<tr>");
            HtmlBuilder.AppendLine("<th></th>");
            HtmlBuilder.AppendLine($"<th>{StrEs001MinutesReportTotal}</th>");
            HtmlBuilder.AppendLine($"<th>{StrAs001MinutesReportTotal}</th>");
            HtmlBuilder.AppendLine($"<th>{StrMinutesReportTotal}</th>");
            HtmlBuilder.AppendLine("</tr>");

            HtmlBuilder.AppendLine(@"<tr>");
            HtmlBuilder.AppendLine("<td></td>");
            HtmlBuilder.AppendLine($"<td>{IntEs001PercentageReportTotal}%</td>");
            HtmlBuilder.AppendLine($"<td>{IntAs001PercentageReportTotal}%</td>");
            HtmlBuilder.AppendLine($"<td></td>");
            HtmlBuilder.AppendLine("</tr>");

            HtmlBuilder.AppendLine(@"<tr>");
            HtmlBuilder.AppendLine($"<th>Total charge {Period}</th>");
            HtmlBuilder.AppendLine($"<th>${Es001_total_charge.ToString("0.00")}</th>");
            HtmlBuilder.AppendLine($"<th>${As001_total_charge.ToString("0.00")}</th>");
            HtmlBuilder.AppendLine($"<th>${TotalCharge.ToString("0.00")}</th>");
            HtmlBuilder.AppendLine("</tr>");
            // End HTML table
            HtmlBuilder.AppendLine("</table>");

            return HtmlBuilder.ToString();
        }
        private string CreateHtml_Report_Retainer()
        {

            //double Es001_total_charge = whs.Sum(item => item.ChargeEs001);
            //double As001_total_charge = whs.Sum(item => item.ChargeAs001);
            //double Es001_ReportTotal = whs.Sum(item => item.Es001MinutesTotal);
            //double As001_ReportTotal = whs.Sum(item => item.As001MinutesTotal);

            double Es001_Additional = IntEs001MinutesReportTotal - (Client.retainer_ES001 * 60);
            double As001_Additional = IntEs001MinutesReportTotal - (Client.retainer_AS001 * 60);
            double Total_Additional = Es001_Additional + As001_Additional;
            //string MinutesReportTotal = AssistFormat.ConvertMinutesToString(whs.Sum(item => item.MinutesTotal));
            HtmlBuilder = new StringBuilder();
            DoNameHeader();

            HtmlBuilder.AppendLine(@"<br>");
            // Start HTML table
            HtmlBuilder.AppendLine("<table  class=\"data\" 'border:5px solid black;border-collapse:collapse;'>");
            // Create table header
            HtmlBuilder.AppendLine(@"<tr>");
            HtmlBuilder.AppendLine("<th>Date</th>");
            HtmlBuilder.AppendLine("<th>Desciption</th>");
            HtmlBuilder.AppendLine("<th>Esther</th>");
            HtmlBuilder.AppendLine("<th>Aihsa</th>");
            HtmlBuilder.AppendLine("<th>TotalHours</th>");
            HtmlBuilder.AppendLine("</tr>");

            // Create table rows
            foreach (DtoWorkedHoursReport wh in LstWorkedHoursReports)
            {
                HtmlBuilder.AppendLine("<tr>");
                HtmlBuilder.AppendLine($"<td>{wh.Today.ToString("dd-MMM-yyyy").Replace(".", "")}</td>");
                HtmlBuilder.AppendLine($"<td>{wh.JobTotal}</td>");
                HtmlBuilder.AppendLine($"<td>{wh.Es001MinutesTotalString}</td>");
                HtmlBuilder.AppendLine($"<td>{wh.As001MinutesTotalString}</td>");
                HtmlBuilder.AppendLine($"<td>{wh.MinutesTotalString}</td>");
                HtmlBuilder.AppendLine("</tr>");
            }
            HtmlBuilder.AppendLine(@"<tr>");
            HtmlBuilder.AppendLine("<th></th>");
            HtmlBuilder.AppendLine("<th></th>");
            HtmlBuilder.AppendLine($"<th>{StrEs001MinutesReportTotal}</th>");
            HtmlBuilder.AppendLine($"<th>{StrAs001MinutesReportTotal}</th>");
            HtmlBuilder.AppendLine($"<th>{StrMinutesReportTotal}</th>");
            HtmlBuilder.AppendLine("</tr>");
            HtmlBuilder.AppendLine("<tr>");
            HtmlBuilder.AppendLine($"<td style=\"border:none\"></td>");
            HtmlBuilder.AppendLine($"<td style=\"border:none;color:#275D5D;font-weight:bold;text-align:right\">Retainer</td>");
            HtmlBuilder.AppendLine($"<td style=\"border:none;color:#275D5D\">{AssistFormat.ConvertHoursToString(Client.retainer_ES001)}</td>");
            HtmlBuilder.AppendLine($"<td style=\"border:none;color:#275D5D\" style=\"border:none\"d>{AssistFormat.ConvertHoursToString(Client.retainer_AS001)}</td>");
            HtmlBuilder.AppendLine($"<td style=\"border:none;color:#275D5D\">{AssistFormat.ConvertHoursToString(Client.retainer_ES001 + Client.retainer_AS001)}</td>");
            HtmlBuilder.AppendLine("</tr>");
            HtmlBuilder.AppendLine("<tr>");
            HtmlBuilder.AppendLine($"<td style=\"border:none;color:#275D5D\"></td>");
            HtmlBuilder.AppendLine($"<td style=\"border:none;color:#275D5D;font-weight:bold;text-align:right\">Additional hours</td>");
            HtmlBuilder.AppendLine($"<td style=\"border:none;color:#275D5D\">{AssistFormat.ConvertMinutesToString(Es001_Additional)}</td>");
            HtmlBuilder.AppendLine($"<td style=\"border:none;color:#275D5D\">{AssistFormat.ConvertMinutesToString(As001_Additional)}</td>");
            HtmlBuilder.AppendLine($"<td style=\"border:none;color:#275D5D\">{AssistFormat.ConvertMinutesToString(Total_Additional)}</td>");
            HtmlBuilder.AppendLine("</tr>");
            HtmlBuilder.AppendLine("</table>");
            HtmlBuilder.AppendLine(@"<br>");
            return HtmlBuilder.ToString();
        }
        private void DoNameHeader()
        {
            HtmlBuilder.AppendLine("<table class=\"header\" 'border:5px solid black;border-collapse:collapse;'>");
            HtmlBuilder.AppendLine("<tr>");
            HtmlBuilder.AppendLine($"<td><img src=\"{Globals.LogoPath}\" alt=\"Your Image\" width=\"125\" height=\"125\"></td>");
            HtmlBuilder.AppendLine($"<td style=\"align=right\"><table>");
            HtmlBuilder.AppendLine("<tr>");
            HtmlBuilder.AppendLine($"<td style=\"font-size:21px;important;\">Client : {Client.clt_name}</td>");
            HtmlBuilder.AppendLine("</tr>");
            HtmlBuilder.AppendLine("<tr>");
            HtmlBuilder.AppendLine($"<td>Period : {Period}</td>");
            HtmlBuilder.AppendLine("</tr>");
            HtmlBuilder.AppendLine($"</table></td>");
            HtmlBuilder.AppendLine("</tr>");
            HtmlBuilder.AppendLine("</table>");
        }
        private void DoRateHeader0()
        {
            HtmlBuilder.AppendLine("<table class=\"header\"'>");
            HtmlBuilder.AppendLine("<tr>");
            HtmlBuilder.AppendLine($"<td><img src=\"{Globals.LogoPath}\" alt=\"Your Image\" width=\"125\" height=\"125\"></td>");
            HtmlBuilder.AppendLine($"<td style=\"align=right\"><table>");
            DoRateHeader1();
            HtmlBuilder.AppendLine($"</td>");
            HtmlBuilder.AppendLine("</tr>");
            HtmlBuilder.AppendLine("</table>");
        }
        private void DoRateHeader1()
        {
            if (!Client.rate_ES001.Equals(Client.rate_AS001))
            {
                HtmlBuilder.AppendLine("<table class=\"header\"'>");
                HtmlBuilder.AppendLine("<tr>");
                HtmlBuilder.AppendLine($"<td style=\"font-size:21px;important;\">Client : {Client.clt_name}</td>");
                HtmlBuilder.AppendLine("</tr>");
                HtmlBuilder.AppendLine("<tr>");
                HtmlBuilder.AppendLine($"<td>Period : {Period}</td>");
                if (InclCharge) HtmlBuilder.AppendLine($"<td>Rate 1: $ {Client.rate_ES001.ToString("0.00")}</td>");
                HtmlBuilder.AppendLine("</tr>");
                HtmlBuilder.AppendLine("<tr>");
                HtmlBuilder.AppendLine($"<td></td>");
                if (InclCharge) HtmlBuilder.AppendLine($"<td>Rate 2: $ {Client.rate_AS001.ToString("0.00")}</td>");
                HtmlBuilder.AppendLine("</tr>");
                HtmlBuilder.AppendLine("</table>");
            }
            else
            {
                HtmlBuilder.AppendLine("<table class=\"header\" 'border:5px solid black;border-collapse:collapse;'>");
                HtmlBuilder.AppendLine("<tr>");
                HtmlBuilder.AppendLine($"<td style=\"font-size:21px;important;\">Client : {Client.clt_name}</td>");
                HtmlBuilder.AppendLine($"<td></td>");
                HtmlBuilder.AppendLine("</tr>");
                HtmlBuilder.AppendLine("<tr>");
                HtmlBuilder.AppendLine($"<td>Period : {Period}</td>");
                if (InclCharge) HtmlBuilder.AppendLine($"<td>Hourly Rate: $ {Client.rate_ES001.ToString("0.00")}</td>");
                HtmlBuilder.AppendLine("</tr>");
                HtmlBuilder.AppendLine("</table>");
            }
        }
    }
}
