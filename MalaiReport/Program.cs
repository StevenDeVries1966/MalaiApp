//create report, types
//app.setting
using DataLayer.Classes;
using MalaiReport.Helpers;
using MalaiReport.Reports;
using System.Globalization;

List<string> timeString1 = new();
timeString1.Add("5:00");
timeString1.Add("4:25");
timeString1.Add("1:10");
timeString1.Add("3:55");
timeString1.Add("3:55");

string format = @"h\:mm";
TimeSpan timeSpan1 = new TimeSpan();
foreach (var time in timeString1)
{

    TimeSpan ts = TimeSpan.ParseExact(time, format, null);
    timeSpan1 = timeSpan1 + ts;
}

List<string> timeString2 = new();
timeString2.Add("5:00");
timeString2.Add("4:25");
timeString2.Add("0:25");
timeString2.Add("3:35");
timeString2.Add("3:35");
// Specify the format for the time string
TimeSpan timeSpan2 = new TimeSpan();
foreach (var time in timeString2)
{

    TimeSpan ts = TimeSpan.ParseExact(time, format, null);
    timeSpan2 = timeSpan2 + ts;
}

TimeSpan timeSpan3 = timeSpan1 + timeSpan2;
// Parse the string to a TimeSpan

string message;
var culture = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

Globals.ReportPath = Path.Combine(Globals.ReportPath, DateTime.Now.ToString("yyyyMMdd_HHmm"));
if (!Directory.Exists(Globals.ReportPath)) Directory.CreateDirectory(Globals.ReportPath);
Globals.ConMan = new MalaiContext(Globals.Server, Globals.DataBase, Globals.UserName, Globals.PassWord);
Globals.ConMan.GetAllEmployees();
Globals.ConMan.GetAllJobs();
Globals.EmployeeCurrent = Globals.ConMan.lstEmployee.FirstOrDefault(o => o.login.Equals(Environment.UserName, StringComparison.CurrentCultureIgnoreCase));

if (Globals.ConMan == null)
{
    Console.BackgroundColor = ConsoleColor.White;
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Problem with database connection");
    Console.BackgroundColor = ConsoleColor.Black;
    Console.ForegroundColor = ConsoleColor.White;
    Console.ReadLine(); return;
}

Globals.ConMan.GetAllClients();
foreach (DtoClient clt in Globals.ConMan.lstClients)
{
    foreach (int month in Globals.Months)
    {

        ReportAll rIMC = new ReportAll(month, Globals.Year, clt.clt_code, Globals.ReportPath);
        if (clt.clt_code.Equals("IMC", StringComparison.CurrentCultureIgnoreCase))
        {
            rIMC = new ReportAll(month, Globals.Year, clt.clt_code, Globals.ReportPath, false);
        }
    }
}
Console.WriteLine("Done");
Console.ReadLine();






