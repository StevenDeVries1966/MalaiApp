//create report, types
//app.setting
using DataLayer.Classes;
using MalaiReport.Helpers;
using MalaiReport.Reports;
using System.Globalization;

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






