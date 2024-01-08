using DataLayer.Classes;

namespace MalaiReport.Reports
{
    public class ReportAll
    {
        public List<DtoWorkedHours> lstWorkedHours { get; set; }
        public List<DtoWorkedHoursReport> lstWorkedHoursReports { get; set; }
        public DtoClient Client { get; set; }
        public string htmlContent { get; set; }

        public string Es001MinutesReportTotal =>
            AssistFormat.ConvertMinutesToString(Convert.ToInt32(lstWorkedHoursReports.Sum(o => o.Es001MinutesTotal)));

        public string As001MinutesReportTotal =>
            AssistFormat.ConvertMinutesToString(Convert.ToInt32(lstWorkedHoursReports.Sum(o => o.As001MinutesTotal)));
        public ReportAll(int month, int year, string clt_code, string htmlFilePath)
        {
            string message = "";
            try
            {
                string monthString = new DateTime(DateTime.Now.Year, month, 1).ToString("MMMM");
                string period = $"{monthString} {year}";
                lstWorkedHours = Globals.ConMan.GetDataClientMonth<DtoWorkedHours>("GetDataClientMonth", month, year, clt_code, out message);
                Client = Globals.ConMan.lstClients.FirstOrDefault(o => o.clt_code.Equals(clt_code, StringComparison.CurrentCultureIgnoreCase));
                if (lstWorkedHours.Count == 0)
                {
                    Console.WriteLine($"No hours for {clt_code} in {month}-{year}");
                    return;
                }
                if (Client == null)
                {
                    Console.WriteLine($"This client is not found in the database: {clt_code} in {month}-{year}");
                    return;
                }
                IEnumerable<IGrouping<DateTime, DtoWorkedHours>> groupedByDate = lstWorkedHours.GroupBy(obj => obj.start_time.Date);
                lstWorkedHoursReports = new List<DtoWorkedHoursReport>();
                foreach (var groep in groupedByDate)
                {
                    lstWorkedHoursReports.Add(new DtoWorkedHoursReport(Client, groep.ToList(), groep.Key.Date));
                }

                string htmlTemplateContent = AssistHtml.GetHtmlResourceContent(@"C:\_GitHubMe\MalaiApp\MalaiReport\HtmlTemplate\HtmlTemplate_IMC.html");
                // Generate the HTML content
                if (Client.report_type.Equals("A", StringComparison.CurrentCultureIgnoreCase))
                {
                    htmlContent = AssistHtml.CreateHtml_Report_IMC(Client, period, lstWorkedHoursReports, Es001MinutesReportTotal, As001MinutesReportTotal, lstWorkedHoursReports.Sum(item => item.Charge));
                }
                else if (Client.report_type.Equals("B", StringComparison.CurrentCultureIgnoreCase))
                {
                    htmlContent = AssistHtml.CreateHtml_Report_CHP(Client, period, lstWorkedHoursReports, Es001MinutesReportTotal, As001MinutesReportTotal, lstWorkedHoursReports.Sum(item => item.Charge));
                }

                htmlContent = htmlTemplateContent.Replace("%Content%", htmlContent);
                htmlFilePath = Path.Combine(htmlFilePath, $"{clt_code}_{month}{year}.html");

                // Save HTML content to a file
                AssistHtml.SaveHtmlToFile(htmlContent, htmlFilePath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }
    }
}
