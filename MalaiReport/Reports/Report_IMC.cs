using DataLayer.Classes;

namespace MalaiReport.Reports
{
    public class Report_IMC
    {
        public List<DtoWorkedHours> lstWorkedHours { get; set; }
        public List<DtoWorkedHoursReport> lstWorkedHoursReports { get; set; }
        public string htmlContent { get; set; }

        public string Es001MinutesReportTotal =>
            AssistFormat.ConvertMinutesToString(Convert.ToInt32(lstWorkedHoursReports.Sum(o => o.Es001MinutesTotal)));

        public string As001MinutesReportTotal =>
            AssistFormat.ConvertMinutesToString(Convert.ToInt32(lstWorkedHoursReports.Sum(o => o.As001MinutesTotal)));
        public Report_IMC(MalaiContext conMan, int month, int year, string clt_code, string htmlFilePath)
        {
            string message = "";
            lstWorkedHours = conMan.GetDataClientMonth<DtoWorkedHours>("GetDataClientMonth", month, year, clt_code, out message);
            if (lstWorkedHours.Count == 0)
            {
                return;
            }
            IEnumerable<IGrouping<DateTime, DtoWorkedHours>> groupedByDate = lstWorkedHours.GroupBy(obj => obj.start_time.Date);
            lstWorkedHoursReports = new List<DtoWorkedHoursReport>();
            foreach (var groep in groupedByDate)
            {
                lstWorkedHoursReports.Add(new DtoWorkedHoursReport(groep.ToList(), groep.Key.Date));
            }

            string htmlTemplateContent = AssistHtml.GetHtmlResourceContent(@"C:\_GitHubMe\MalaiApp\MalaiReport\HtmlTemplate\HtmlTemplate_IMC.html");
            // Generate the HTML content
            htmlContent = AssistHtml.ConvertListToHtmlDataGrid(lstWorkedHoursReports, Es001MinutesReportTotal, As001MinutesReportTotal, lstWorkedHoursReports.Sum(item => item.Charge));
            htmlContent = htmlTemplateContent.Replace("%Content%", htmlContent);
            htmlFilePath = Path.Combine(htmlFilePath, $"{clt_code}_{month}{year}.html");

            // Save HTML content to a file
            AssistHtml.SaveHtmlToFile(htmlContent, htmlFilePath);
        }
    }
}
