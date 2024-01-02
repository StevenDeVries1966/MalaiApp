// See https://aka.ms/new-console-template for more information



//double decimalHours = (double)2.0833;

//// Calculate TimeSpan from decimal hours
//int hours = (int)decimalHours;
//int minutes = (int)((decimalHours - hours) * 60);
//string strminutes = hours < 10 ? $"0{minutes}" : Convert.ToString(minutes);
//var test = $"{hours}:{strminutes}";

using DataLayer.Classes;

DateTime start = DateTime.Now;
MalaiContext conMan = new MalaiContext("127.0.0.1", @"Malai_test", "root", "wqEQW5Ag/&6%JT+");
conMan.GetAllWorkedHours();
DateTime end = DateTime.Now;
TimeSpan ts = end - start;
Console.WriteLine($"Done {ts.TotalSeconds}");
//string mesagge = "";

//string filePath = @"C:\Prive\Malai\Docs\23_10.csv";
//conMan.ReadCsv(filePath);
//filePath = @"C:\Prive\Malai\Docs\23_11.csv";
//conMan.ReadCsv(filePath);
Console.WriteLine("Done");
Console.ReadLine();

