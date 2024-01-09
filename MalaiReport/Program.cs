

// See https://aka.ms/new-console-template for more information

using DataLayer.Classes;
using MalaiReport.Helpers;
using MalaiReport.Reports;

string message;

Globals.ReportPath = Path.Combine(Globals.ReportPath, DateTime.Now.ToString("yyyyMMdd_HHmm"));
if (!Directory.Exists(Globals.ReportPath)) Directory.CreateDirectory(Globals.ReportPath);
Globals.ConMan = new MalaiContext(Globals.Server, Globals.DataBase, Globals.UserName, Globals.PassWord);
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
List<int> lstMonths = new List<int>() { 8, 10, 11 };
foreach (DtoClient clt in Globals.ConMan.lstClients)
{
    foreach (int month in lstMonths)
    {
        Console.WriteLine($"process month {month} for client {clt.clt_name}");
        ReportAll rIMC = new ReportAll(month, 2023, clt.clt_code, Globals.ReportPath);
    }
}
Console.WriteLine("Done");
Console.ReadLine();






