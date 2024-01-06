

// See https://aka.ms/new-console-template for more information

using DataLayer.Classes;
using MalaiReport;

Console.WriteLine("Hello, World!");
string message;
MalaiContext conMan = new MalaiContext("127.0.0.1", @"Malai_test", "root", "wqEQW5Ag/&6%JT+");
List<DtoWorkedHours> lstWorkedHours = conMan.GetDataClientMonth<DtoWorkedHours>("GetDataClientMonth", out message);

var groupedByDate = lstWorkedHours.GroupBy(obj => obj.start_time.Date);
List<DtoWorkedHoursReport> lstWorkedHoursReports = new List<DtoWorkedHoursReport>();
foreach (var groep in groupedByDate)
{
    lstWorkedHoursReports.Add(new DtoWorkedHoursReport(groep.ToList(), groep.Key.Date));
}

string htmlFilePath = @"C:/Temp/input.html";
string outputPdfPath = @"C:/Temp/input.pdf";

// Generate the HTML content
string htmlContent = AssistHtml.ConvertListToHtmlDataGrid(lstWorkedHoursReports, lstWorkedHoursReports.Sum(item => item.charge));

// Save HTML content to a file
AssistHtml.SaveHtmlToFile(htmlContent, htmlFilePath);




