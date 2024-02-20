// See https://aka.ms/new-console-template for more information



//double decimalHours = (double)2.0833;

//// Calculate TimeSpan from decimal hours
//int hours = (int)decimalHours;
//int minutes = (int)((decimalHours - hours) * 60);
//string strminutes = hours < 10 ? $"0{minutes}" : Convert.ToString(minutes);
//var test = $"{hours}:{strminutes}";

using DataLayer.Classes;

DateTime start = DateTime.Now;
MalaiContext conMan = new MalaiContext("malaidevelopment.database.windows.net", @"malai_dev", "write_malai", "tZRj.3-t$EB)8jF", true);
//MalaiContext conMan = new MalaiContext("127.0.0.1", @"Malai_test", "root", "wqEQW5Ag/&6%JT+");
//conMan.GetAllWorkedHours();
var test = conMan.lstJobs;
DateTime end = DateTime.Now;
TimeSpan ts = end - start;
Console.WriteLine($"Done {ts.TotalSeconds}");

string filePath = @"C:\Prive\Malai\Docs\CsvImport\";
//conMan.ReadCsv(Path.Combine(filePath, "23_08.csv"));
//conMan.ReadCsv(Path.Combine(filePath, "23_09.csv"));
//conMan.ReadCsv(Path.Combine(filePath, "23_10.csv"));
//conMan.ReadCsv(Path.Combine(filePath, "23_11.csv"));
//conMan.ReadCsv(Path.Combine(filePath, "23_12.csv"));
conMan.ReadCsv(Path.Combine(filePath, "24_01_new3.csv"));
Console.WriteLine("Done");
Console.ReadLine();

