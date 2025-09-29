using DataLayer.Classes;

//DoCsv();

string excelPath = @"C:\Prive\Malai\Docs\Input\2412 - Timesheet.xlsx";
ImportExcel(excelPath, "Server=.;Database=malai_prod;Integrated Security=True;");



void ImportExcel(string excelPath, string connectionstring)
{
    AssistExcel excel = new AssistExcel();
    List<DtoWorkedHours> lst = excel.ReadExcel(excelPath);

    DateTime start = DateTime.Now;
    MalaiContext conMan = new MalaiContext(connectionstring, true);
    //var test = conMan.lstJobs;
    string msg;
    conMan.AddWorkedHours(lst, out msg);
    DateTime end = DateTime.Now;
    TimeSpan ts = end - start;
    Console.WriteLine($"Done {ts.TotalSeconds}");
    Console.ReadLine();
}
void DoCsv()
{
    DateTime start = DateTime.Now;
    MalaiContext conMan = new MalaiContext("Server=.;Database=malai_uat;Integrated Security=True;", true);
    var test = conMan.lstJobs;
    DateTime end = DateTime.Now;
    TimeSpan ts = end - start;
    Console.WriteLine($"Done {ts.TotalSeconds}");

    string filePath = @"C:\Prive\Malai\Docs\CsvImport\";
    conMan.ReadCsv(Path.Combine(filePath, "25_02.csv"));
    Console.WriteLine("Done");
    Console.ReadLine();
}

