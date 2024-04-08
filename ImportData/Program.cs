// See https://aka.ms/new-console-template for more information



//double decimalHours = (double)2.0833;

//// Calculate TimeSpan from decimal hours
//int hours = (int)decimalHours;
//int minutes = (int)((decimalHours - hours) * 60);
//string strminutes = hours < 10 ? $"0{minutes}" : Convert.ToString(minutes);
//var test = $"{hours}:{strminutes}";

using DataLayer.Classes;


DateTime start = DateTime.Now;
MalaiContext conMan = new MalaiContext("Server=.;Database=malai_prod;Integrated Security=True;", true);
var test = conMan.lstJobs;
DateTime end = DateTime.Now;
TimeSpan ts = end - start;
Console.WriteLine($"Done {ts.TotalSeconds}");

string filePath = @"C:\Prive\Malai\Docs\CsvImport\";
//conMan.ReadCsv(Path.Combine(filePath, "24_02.csv"));
conMan.ReadCsv(Path.Combine(filePath, "24_03.csv"));
Console.WriteLine("Done");
Console.ReadLine();

