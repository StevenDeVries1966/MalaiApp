using DataLayer.Classes;
using MalaiReport.Helpers;
using System.Text;

namespace MalaiReport.Reports
{
    public class ReportAll
    {
        private List<DtoWorkedHours> LstWorkedHours { get; set; }
        private List<DtoWorkedHoursByEmpReport> LstWorkedHoursByEmpReports { get; set; }
        private List<DtoWorkedHoursByJobReport> LstWorkedHoursByJobReports { get; set; }
        private DtoClient Client { get; set; }
        private string HtmlContent { get; set; }
        private string HtmlContentHrsC { get; set; } // this report should be made for every client

        private int IntEs001MinutesReportTotal => Convert.ToInt32(LstWorkedHoursByEmpReports.Sum(o => o.DblEs001MinutesTotal));
        private int IntEs001PercentageReportTotal => Convert.ToInt32(Math.Round((double)(((double)IntEs001MinutesReportTotal / (double)IntMinutesReportTotal) * 100), 0));
        private string StrEs001MinutesReportTotal => AssistFormat.ConvertMinutesToString(IntEs001MinutesReportTotal);
        private double DblEs001TotalCharge => LstWorkedHoursByEmpReports.Sum(item => item.ChargeEs001);

        private int IntAs001MinutesReportTotal => Convert.ToInt32(LstWorkedHoursByEmpReports.Sum(o => o.DblAs001MinutesTotal));
        private int IntAs001PercentageReportTotal => Convert.ToInt32(Math.Round((double)(((double)IntAs001MinutesReportTotal / (double)IntMinutesReportTotal) * 100), 0));
        private string StrAs001MinutesReportTotal =>
            AssistFormat.ConvertMinutesToString(IntAs001MinutesReportTotal);
        private double DblAs001TotalCharge => LstWorkedHoursByEmpReports.Sum(item => item.ChargeAs001);


        private int IntHrMinutesReportTotal => Convert.ToInt32(LstWorkedHoursByEmpReports.Sum(o => o.DblHrMinutesTotal));
        private int IntHrPercentageReportTotal => Convert.ToInt32(Math.Round((double)(((double)IntHrMinutesReportTotal / (double)IntMinutesReportTotal) * 100), 0));
        private string StrHrMinutesReportTotal => AssistFormat.ConvertMinutesToString(IntHrMinutesReportTotal);
        private double DblHrTotalCharge { get; set; }

        private int IntMinutesReportTotal => IntEs001MinutesReportTotal + IntAs001MinutesReportTotal + IntHrMinutesReportTotal;
        private string StrMinutesReportTotal =>
            AssistFormat.ConvertMinutesToString(IntMinutesReportTotal);
        private double TotalCharge => LstWorkedHoursByEmpReports.Sum(item => item.Charge);
        private bool InclCharge { get; set; }

        private StringBuilder HtmlBuilder { get; set; }
        private string MonthString { get; set; }
        private string Period { get; set; }
        private IEnumerable<IGrouping<DateTime, DtoWorkedHours>> GroupedByDate { get; set; }
        private IEnumerable<IGrouping<string, DtoWorkedHours>> GroupedByJobName { get; set; }
        private string HtmlTemplateContent { get; set; }

        public ReportAll(int month, int year, string clt_code, string htmlFilePath, bool inclCharge = true)
        {
            try
            {
                InclCharge = inclCharge;
                MonthString = new DateTime(DateTime.Now.Year, month, 1).ToString("MM");
                Period = $"{year}-{MonthString}";
                LstWorkedHours = Globals.ConMan?.GetDataClientMonth<DtoWorkedHours>("GetDataClientMonth", month, year, clt_code, out _)!;
                Client = Globals.ConMan?.lstClients.FirstOrDefault(o => o.clt_code.Equals(clt_code, StringComparison.CurrentCultureIgnoreCase))!;
                if (LstWorkedHours != null && LstWorkedHours.Count == 0)
                {
                    Console.WriteLine($"No hours for {clt_code} in {Period}");
                    return;
                }
                if (Client == null)
                {
                    Console.WriteLine($"This client is not found in the database: {clt_code} in {Period}");
                    return;
                }
                Console.WriteLine($"process {Period} for client {Client.clt_name}");

                GroupedByDate = LstWorkedHours!.GroupBy(obj => obj.start_time.Date);
                LstWorkedHoursByEmpReports = new List<DtoWorkedHoursByEmpReport>();
                foreach (var groep in GroupedByDate)
                {
                    LstWorkedHoursByEmpReports.Add(new DtoWorkedHoursByEmpReport(Client, groep.ToList(), groep.Key.Date));
                }

                // verzamel data voor rapport Hrs_C
                GroupedByJobName = LstWorkedHours!.GroupBy(obj => obj.job_name);
                LstWorkedHoursByJobReports = new List<DtoWorkedHoursByJobReport>();
                foreach (var groep in GroupedByJobName)
                {
                    DtoJob job = Globals.ConMan.lstJobs
                        .FirstOrDefault(o => o.job_name.Equals(groep.Key, StringComparison.CurrentCultureIgnoreCase));
                    LstWorkedHoursByJobReports.Add(new DtoWorkedHoursByJobReport(Client, groep.ToList(), job));
                }

                //TotalCharge = LstWorkedHoursByEmpReports.Sum(item => item.Charge);
                //DblEs001TotalCharge = LstWorkedHoursByEmpReports.Sum(item => item.ChargeEs001);
                //DblAs001TotalCharge = LstWorkedHoursByEmpReports.Sum(item => item.ChargeAs001);

                HtmlTemplateContent = AssistHtml.GetHtmlResourceContent(Globals.HtmlTemplatePath);

                // this report must be created for all clients
                HtmlContentHrsC = CreateHtml_Report_Hrs_C();
                HtmlContentHrsC = HtmlTemplateContent.Replace("%Content%", HtmlContentHrsC);
                // Save HTML content to a file
                AssistHtml.SaveHtmlToFile(HtmlContentHrsC, Path.Combine(htmlFilePath, $"{Period}_{clt_code}_Hrs_C.html"));

                // Generate the HTML content
                if (Client.report_type.Equals("Hrs_B", StringComparison.CurrentCultureIgnoreCase))
                {
                    HtmlContent = CreateHtml_Report_Hrs_B();
                }
                else if (Client.report_type.Equals("Hrs_A", StringComparison.CurrentCultureIgnoreCase))
                {
                    HtmlContent = CreateHtml_Report_Hrs_A();
                }
                else if (Client.report_type.Equals("Ret", StringComparison.CurrentCultureIgnoreCase))
                {
                    HtmlContent = CreateHtml_Report_Retainer();
                }
                else if (Client.report_type.Equals("", StringComparison.CurrentCultureIgnoreCase))
                {
                    return;
                }

                HtmlContent = HtmlTemplateContent.Replace("%Content%", HtmlContent);
                // PHC && InclCharge = true
                if (!Client.clt_code.Equals("IMC", StringComparison.CurrentCultureIgnoreCase) && InclCharge)
                {
                    htmlFilePath = Path.Combine(htmlFilePath, $"{Period}_{clt_code}_{Client.report_type}.html");
                }
                // IMC && InclCharge = true
                else if (Client.clt_code.Equals("IMC", StringComparison.CurrentCultureIgnoreCase) && InclCharge)
                {
                    htmlFilePath = Path.Combine(htmlFilePath, $"{Period}_{clt_code}_{Client.report_type}_PLUS.html");
                }
                // IMC && InclCharge = false
                else if (Client.clt_code.Equals("IMC", StringComparison.CurrentCultureIgnoreCase) && !InclCharge)
                {
                    htmlFilePath = Path.Combine(htmlFilePath, $"{Period}_{clt_code}_{Client.report_type}.html");
                }
                // Save HTML content to a file
                AssistHtml.SaveHtmlToFile(HtmlContent, htmlFilePath);
            }
            catch (Exception e)
            {
                Globals.ConMan!.AddLog(e.Message, e!.StackTrace!, Globals.EmployeeCurrent!.emp_id);
            }
        }
        private string CreateHtml_Report_Hrs_B()
        {

            HtmlBuilder = new StringBuilder();

            // Start HTML table
            HtmlBuilder.AppendLine("<table>");
            HtmlBuilder.AppendLine(@"<tr>");
            HtmlBuilder.AppendLine(@"<td>");
            DoRateHeader0();
            HtmlBuilder.AppendLine("</td>");
            HtmlBuilder.AppendLine("</tr>");
            HtmlBuilder.AppendLine(@"<tr>");
            HtmlBuilder.AppendLine(@"<td>");
            DoData_Hrs_B();
            HtmlBuilder.AppendLine("</td>");
            HtmlBuilder.AppendLine("</tr>");
            HtmlBuilder.AppendLine("</table>");


            return HtmlBuilder.ToString();
        }

        private void DoData_Hrs_B()
        {
            HtmlBuilder.AppendLine(@"<br>");
            HtmlBuilder.AppendLine("<table class=\"data\" style=\"width:100%\">");
            HtmlBuilder.AppendLine("<tr>");
            HtmlBuilder.AppendLine($"<td style=\"font-weight:bold;text-align:center;color:#275D5D;\" colspan=\"2\"></td>");
            HtmlBuilder.AppendLine(
                $"<td style=\"font-weight:bold;text-align:center;color:#275D5D;\" colspan=\"2\">IMC - AP</td>");
            HtmlBuilder.AppendLine($"<td style=\"font-weight:bold;text-align:center;color:#275D5D;\">IMC - HR</td>");
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
            foreach (DtoWorkedHoursByEmpReport wh in LstWorkedHoursByEmpReports)
            {
                HtmlBuilder.AppendLine("<tr>");
                HtmlBuilder.AppendLine(
                    $"<td style=\"font-weight:bold;text-align:left\">{wh.Today.ToString("dd-MMM-yyyy").Replace(".", "")}</td>");
                HtmlBuilder.AppendLine($"<td style=\"font-weight:bold;text-align:left\">{wh.JobTotal}</td>");
                HtmlBuilder.AppendLine(
                    $"<td style=\"font-weight:bold;text-align:center;color:#808080;\">{wh.StrEs001MinutesTotal}</td>");
                HtmlBuilder.AppendLine(
                    $"<td style=\"font-weight:bold;text-align:center;color:#808080;\">{wh.StrAs001MinutesTotal}</td>");
                HtmlBuilder.AppendLine(
                    $"<td style=\"font-weight:bold;text-align:center;color:#808080;\">{wh.StrHrMinutesTotal}</td>");
                HtmlBuilder.AppendLine(
                    $"<td style=\"font-weight:bold;text-align:center;color:#808080;border-right-style:none;\">{wh.MinutesTotalString}</td>");
                if (InclCharge)
                {
                    HtmlBuilder.AppendLine(
                        $"<td style=\"border-left-style:none;border-right-style:none;font-weight:bold;\">$</td>");
                    HtmlBuilder.AppendLine(
                        $"<td style=\"border-left-style:none;text-align:right;font-weight:bold;\">{wh.Charge.ToString("0.00")}</td>");
                }

                HtmlBuilder.AppendLine("</tr>");
            }

            HtmlBuilder.AppendLine(@"<tr>");
            HtmlBuilder.AppendLine("<th></th>");
            HtmlBuilder.AppendLine("<th></th>");
            HtmlBuilder.AppendLine(
                $"<th style=\"font-weight:bold;text-align:center;color:#808080;\">{StrEs001MinutesReportTotal}</th>");
            HtmlBuilder.AppendLine(
                $"<th style=\"font-weight:bold;text-align:center;color:#808080;\">{StrAs001MinutesReportTotal}</th>");
            HtmlBuilder.AppendLine(
                $"<th style=\"font-weight:bold;text-align:center;color:#808080;\">{StrHrMinutesReportTotal}</th>");
            HtmlBuilder.AppendLine(
                $"<th style=\"border-right-style:none;font-weight:bold;text-align:center;color:#808080;\">{StrMinutesReportTotal}</th>");
            if (InclCharge)
            {
                HtmlBuilder.AppendLine(
                    $"<th style=\"border-left-style:none;border-right-style:none;font-weight:bold;color:#000000;\">$</th>");
                HtmlBuilder.AppendLine(
                    $"<th style=\"border-left-style:none;text-align:right;font-weight:bold;color:#000000;\">{TotalCharge.ToString("0.00")}</th>");
            }

            HtmlBuilder.AppendLine("</tr>");
            // End HTML table
            HtmlBuilder.AppendLine("</table>");
        }

        private string CreateHtml_Report_Hrs_A()
        {
            HtmlBuilder = new StringBuilder();

            // Start HTML table
            HtmlBuilder.AppendLine("<table>");
            HtmlBuilder.AppendLine(@"<tr>");
            HtmlBuilder.AppendLine(@"<td>");
            DoRateHeader0();
            HtmlBuilder.AppendLine("</td>");
            HtmlBuilder.AppendLine("</tr>");
            HtmlBuilder.AppendLine(@"<tr>");
            HtmlBuilder.AppendLine(@"<td>");
            DoData_Hrs_A();
            HtmlBuilder.AppendLine("</td>");
            HtmlBuilder.AppendLine("</tr>");
            HtmlBuilder.AppendLine("</table>");


            return HtmlBuilder.ToString();
        }

        private void DoData_Hrs_A()
        {
            HtmlBuilder.AppendLine(@"<br>");

            HtmlBuilder.AppendLine("<table class=\"data\" style=\"width:100%\">");
            HtmlBuilder.AppendLine("<tr>");
            HtmlBuilder.AppendLine($"<td></td>");
            HtmlBuilder.AppendLine($"<td style=\"font-weight:bold;color:#275D5D\">Accounting</td>");
            HtmlBuilder.AppendLine($"<td style=\"font-weight:bold;color:#275D5D\">Assistant</td>");
            HtmlBuilder.AppendLine($"<td style=\"font-weight:bold;color:#275D5D\">Totals</td>");
            HtmlBuilder.AppendLine("</tr>");
            // Create table header
            HtmlBuilder.AppendLine(@"<tr>");
            HtmlBuilder.AppendLine("<th>Date</th>");
            HtmlBuilder.AppendLine("<th>Rate 1 - Hours</th>");
            HtmlBuilder.AppendLine("<th>Rate 2 - Hours</th>");
            HtmlBuilder.AppendLine("<th>Hours</th>");
            HtmlBuilder.AppendLine("</tr>");

            // Create table rows
            foreach (DtoWorkedHoursByEmpReport wh in LstWorkedHoursByEmpReports)
            {
                HtmlBuilder.AppendLine("<tr>");
                HtmlBuilder.AppendLine(
                    $"<td style=\"font-weight:bold;text-align:left\">{wh.Today.ToString("dd-MMM-yyyy").Replace(".", "")}</td>");
                HtmlBuilder.AppendLine(
                    $"<td style=\"font-weight:bold;text-align:center;color:#808080;\">{wh.StrEs001MinutesTotal}</td>");
                HtmlBuilder.AppendLine(
                    $"<td style=\"font-weight:bold;text-align:center;color:#808080;\">{wh.StrAs001MinutesTotal}</td>");
                HtmlBuilder.AppendLine(
                    $"<td style=\"font-weight:bold;text-align:center;color:#808080;\">{wh.MinutesTotalString}</td>");
                HtmlBuilder.AppendLine("</tr>");
            }

            HtmlBuilder.AppendLine(@"<tr>");
            HtmlBuilder.AppendLine("<th></th>");
            HtmlBuilder.AppendLine(
                $"<th style=\"font-weight:bold;text-align:center;color:#808080;\">{StrEs001MinutesReportTotal}</th>");
            HtmlBuilder.AppendLine(
                $"<th style=\"font-weight:bold;text-align:center;color:#808080;\">{StrAs001MinutesReportTotal}</th>");
            HtmlBuilder.AppendLine(
                $"<th style=\"font-weight:bold;text-align:center;color:#808080;\">{StrMinutesReportTotal}</th>");
            HtmlBuilder.AppendLine("</tr>");

            HtmlBuilder.AppendLine(@"<tr>");
            HtmlBuilder.AppendLine("<td></td>");
            HtmlBuilder.AppendLine(
                $"<td style=\"font-weight:bold;text-align:center;color:#808080;\">{IntEs001PercentageReportTotal}%</td>");
            HtmlBuilder.AppendLine(
                $"<td style=\"font-weight:bold;text-align:center;color:#808080;\">{IntAs001PercentageReportTotal}%</td>");
            HtmlBuilder.AppendLine($"<td style=\"font-weight:bold;text-align:center;color:#808080;\"></td>");
            HtmlBuilder.AppendLine("</tr>");

            HtmlBuilder.AppendLine(@"<tr>");
            HtmlBuilder.AppendLine($"<th style=\"font-weight:bold;text-align:left;color:#000000;\">Total charge {Period}</th>");
            HtmlBuilder.AppendLine(
                $"<th style=\"font-weight:bold;text-align:center;color:#808080;\">${DblEs001TotalCharge.ToString("0.00")}</th>");
            HtmlBuilder.AppendLine(
                $"<th style=\"font-weight:bold;text-align:center;color:#808080;\">${DblAs001TotalCharge.ToString("0.00")}</th>");
            HtmlBuilder.AppendLine(
                $"<th style=\"font-weight:bold;text-align:right;color:#000000;\">${TotalCharge.ToString("0.00")}</th>");
            HtmlBuilder.AppendLine("</tr>");
            // End HTML table
            HtmlBuilder.AppendLine("</table>");
        }

        private string CreateHtml_Report_Hrs_C()
        {
            HtmlBuilder = new StringBuilder();

            // Start HTML table
            HtmlBuilder.AppendLine("<table>");
            HtmlBuilder.AppendLine(@"<tr>");
            HtmlBuilder.AppendLine(@"<td>");
            DoNameHeader();
            HtmlBuilder.AppendLine("</td>");
            HtmlBuilder.AppendLine("</tr>");
            HtmlBuilder.AppendLine(@"<tr>");
            HtmlBuilder.AppendLine(@"<td>");
            DoData_Hrs_C();
            HtmlBuilder.AppendLine("</td>");
            HtmlBuilder.AppendLine("</tr>");
            HtmlBuilder.AppendLine("</table>");


            return HtmlBuilder.ToString();
        }

        private void DoData_Hrs_C()
        {
            HtmlBuilder.AppendLine(@"<br>");

            HtmlBuilder.AppendLine("<table class=\"data\" style=\"width:100%\">");
            // Create table header
            HtmlBuilder.AppendLine(@"<tr>");
            HtmlBuilder.AppendLine("<th>Description</th>");
            HtmlBuilder.AppendLine("<th>Esther</th>");
            HtmlBuilder.AppendLine("<th>Aisha</th>");
            HtmlBuilder.AppendLine("<th>Total Hours</th>");
            HtmlBuilder.AppendLine("</tr>");

            // Create table rows
            foreach (DtoWorkedHoursByJobReport job in LstWorkedHoursByJobReports)
            {
                HtmlBuilder.AppendLine("<tr>");
                HtmlBuilder.AppendLine($"<td style=\"font-weight:bold;text-align:left\">{job.Job.job_name}</td>");
                HtmlBuilder.AppendLine(
                    $"<td style=\"font-weight:bold;text-align:center;color:#808080;\">{job.StrEs001MinutesTotal}</td>");
                HtmlBuilder.AppendLine(
                    $"<td style=\"font-weight:bold;text-align:center;color:#808080;\">{job.StrAs001MinutesTotal}</td>");
                HtmlBuilder.AppendLine(
                    $"<td style=\"font-weight:bold;text-align:center;color:#808080;\">{job.MinutesTotalString}</td>");
                HtmlBuilder.AppendLine("</tr>");
            }

            HtmlBuilder.AppendLine(@"<tr>");
            HtmlBuilder.AppendLine("<th></th>");
            HtmlBuilder.AppendLine(
                $"<th style=\"font-weight:bold;text-align:center;color:#808080;\">{StrEs001MinutesReportTotal}</th>");
            HtmlBuilder.AppendLine(
                $"<th style=\"font-weight:bold;text-align:center;color:#808080;\">{StrAs001MinutesReportTotal}</th>");
            HtmlBuilder.AppendLine(
                $"<th style=\"font-weight:bold;text-align:center;color:#808080;\">{StrMinutesReportTotal}</th>");
            HtmlBuilder.AppendLine("</tr>");
            // End HTML table
            HtmlBuilder.AppendLine("</table>");
        }

        private string CreateHtml_Report_Retainer()
        {

            double es001Additional = IntEs001MinutesReportTotal - (Client.retainer_ES001 * 60);
            double as001Additional = IntAs001MinutesReportTotal - (Client.retainer_AS001 * 60);
            double totalAdditional = es001Additional + as001Additional;

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
            foreach (DtoWorkedHoursByEmpReport wh in LstWorkedHoursByEmpReports)
            {
                HtmlBuilder.AppendLine("<tr>");
                HtmlBuilder.AppendLine($"<td style=\"font-weight:bold;text-align:left\">{wh.Today.ToString("dd-MMM-yyyy").Replace(".", "")}</td>");
                HtmlBuilder.AppendLine($"<td style=\"font-weight:bold;text-align:left\">{wh.JobTotal}</td>");
                HtmlBuilder.AppendLine($"<td style=\"font-weight:bold;text-align:center;color:#808080;\">{wh.StrEs001MinutesTotal}</td>");
                HtmlBuilder.AppendLine($"<td style=\"font-weight:bold;text-align:center;color:#808080;\">{wh.StrAs001MinutesTotal}</td>");
                HtmlBuilder.AppendLine($"<td style=\"font-weight:bold;text-align:center;color:#808080;\">{wh.MinutesTotalString}</td>");
                HtmlBuilder.AppendLine("</tr>");
            }
            HtmlBuilder.AppendLine(@"<tr>");
            HtmlBuilder.AppendLine("<th></th>");
            HtmlBuilder.AppendLine("<th></th>");
            HtmlBuilder.AppendLine($"<th style=\"font-weight:bold;text-align:center;color:#808080;\">{StrEs001MinutesReportTotal}</th>");
            HtmlBuilder.AppendLine($"<th style=\"font-weight:bold;text-align:center;color:#808080;\">{StrAs001MinutesReportTotal}</th>");
            HtmlBuilder.AppendLine($"<th style=\"font-weight:bold;text-align:center;color:#000000;\">{StrMinutesReportTotal}</th>");
            HtmlBuilder.AppendLine("</tr>");
            HtmlBuilder.AppendLine("<tr>");
            HtmlBuilder.AppendLine($"<td style=\"border:none\"></td>");
            HtmlBuilder.AppendLine($"<td style=\"border:none;color:#275D5D;font-weight:bold;text-align:right\">Retainer</td>");
            HtmlBuilder.AppendLine($"<td style=\"border:none;color:#275D5D\">{AssistFormat.ConvertHoursToString(Client.retainer_ES001)}</td>");
            HtmlBuilder.AppendLine($"<td style=\"border:none;color:#275D5D\" style=\"border:none\"d>{AssistFormat.ConvertHoursToString(Client.retainer_AS001)}</td>");
            HtmlBuilder.AppendLine($"<td style=\"border:none;color:#000000\">{AssistFormat.ConvertHoursToString(Client.retainer_ES001 + Client.retainer_AS001)}</td>");
            HtmlBuilder.AppendLine("</tr>");
            HtmlBuilder.AppendLine("<tr>");
            HtmlBuilder.AppendLine($"<td style=\"border:none;color:#275D5D\"></td>");
            HtmlBuilder.AppendLine($"<td style=\"border:none;color:#275D5D;font-weight:bold;text-align:right\">Additional hours</td>");
            HtmlBuilder.AppendLine($"<td style=\"border:none;color:#275D5D\">{AssistFormat.ConvertMinutesToString(es001Additional)}</td>");
            HtmlBuilder.AppendLine($"<td style=\"border:none;color:#275D5D\">{AssistFormat.ConvertMinutesToString(as001Additional)}</td>");
            HtmlBuilder.AppendLine($"<td style=\"border:none;color:#000000\">{AssistFormat.ConvertMinutesToString(totalAdditional)}</td>");
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
