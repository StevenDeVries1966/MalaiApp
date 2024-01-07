

// See https://aka.ms/new-console-template for more information

using DataLayer.Classes;
using MalaiReport.Reports;

string message;
MalaiContext conMan = new MalaiContext("127.0.0.1", @"Malai_test", "root", "wqEQW5Ag/&6%JT+");
conMan.GetAllClients();
List<int> lstMonths = new List<int>() { 8, 10, 11 };
foreach (DtoClient clt in conMan.lstClients)
{
    foreach (int month in lstMonths)
    {
        Console.WriteLine($"process month {month} for client {clt.clt_name}");
        Report_IMC rIMC = new Report_IMC(conMan, month, 2023, clt.clt_code, @"C:\Prive\Malai\Docs\Reports");
    }
}
Console.WriteLine("Done");
Console.ReadLine();






