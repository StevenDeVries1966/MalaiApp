

// See https://aka.ms/new-console-template for more information

using DataLayer.Classes;
using MalaiReport;
using MalaiReport.Reports;

string message;
string pathReport = @"C:\Prive\Malai\Docs\Reports";
pathReport = Path.Combine(pathReport, DateTime.Now.ToString("yyyyMMdd_HHmm"));
if (!Directory.Exists(pathReport)) Directory.CreateDirectory(pathReport);
Globals.ConMan = new MalaiContext("127.0.0.1", @"Malai_test", "root", "wqEQW5Ag/&6%JT+");
if (Globals.ConMan != null) Console.WriteLine("Problem with database connection");
Globals.ConMan.GetAllClients();
List<int> lstMonths = new List<int>() { 8, 10, 11 };
foreach (DtoClient clt in Globals.ConMan.lstClients)
{
    foreach (int month in lstMonths)
    {
        Console.WriteLine($"process month {month} for client {clt.clt_name}");
        if (clt.report_type.Equals("A", StringComparison.CurrentCultureIgnoreCase))
        {
            Report_IMC rIMC = new Report_IMC(month, 2023, clt.clt_code, pathReport);
        }
        else if (clt.report_type.Equals("B", StringComparison.CurrentCultureIgnoreCase))
        {
            Report_IMC rIMC = new Report_IMC(month, 2023, clt.clt_code, pathReport);
        }
    }
}
Console.WriteLine("Done");
Console.ReadLine();






