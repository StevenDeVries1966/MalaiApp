using ClosedXML.Excel;

namespace DataLayer.Classes
{
    public class AssistExcel
    {

        public List<DtoWorkedHours> ReadExcel(string filePath)
        {
            List<DtoWorkedHours> lst = new List<DtoWorkedHours>();
            // Open the Excel workbook
            using (var workbook = new XLWorkbook(filePath))
            {
                // Get the first worksheet
                var worksheet = workbook.Worksheet(1); // 1-based index for the first sheet

                // Determine the range of the used cells
                var range = worksheet.RangeUsed();
                if (range == null)
                {
                    Console.WriteLine("The worksheet is empty.");
                    return lst;
                }

                // Get the total rows and columns in the range
                int totalRows = range.RowCount();
                int totalColumns = range.ColumnCount();

                // Loop through the rows and columns
                for (int row = 4; row <= totalRows; row++) // Start at row 1 (ClosedXML is 1-based)
                {
                    //Console.WriteLine($"row : {row}");
                    //if (row == 188)
                    //{ }
                    try
                    {
                        if (string.IsNullOrEmpty(range.Cell(row, 2).GetValue<string>()))
                        {
                            continue;
                        }
                        DateTime date = Convert.ToDateTime(range.Cell(row, 2).GetValue<string>());
                        
                        string empCode = range.Cell(row, 3).GetValue<string>();
                        string cltCode = range.Cell(row, 4).GetValue<string>();
                        string cltJocCode = range.Cell(row, 5).GetValue<string>();
                        string notes = range.Cell(row, 6).GetValue<string>();
                        DateTime start;
                        if (range.Cell(row, 7).GetValue<string>() == "24:00:00")
                        {
                            start = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                        }
                        else
                        {
                            start = Convert.ToDateTime(range.Cell(row, 7).GetValue<string>());
                            start = new DateTime(date.Year, date.Month, date.Day, start.Hour, start.Minute, start.Second);
                        }

                        DateTime end;
                        if (range.Cell(row, 8).GetValue<string>() == "24:00:00")
                        {
                            end = date.AddDays(1).Date;
                            //end = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
                        }
                        else
                        {
                            end = Convert.ToDateTime(range.Cell(row, 8).GetValue<string>());
                            end = new DateTime(date.Year, date.Month, date.Day, end.Hour, end.Minute, end.Second);
                        }
                        DtoWorkedHours wh = new DtoWorkedHours(empCode, cltCode, cltJocCode, start, end, notes);
                        lst.Add(wh);
                        //if (cltCode.Equals("IMC"))
                        //{
                        //    if (date.Day == 25)
                        //    {}
                        //    lst.Add(wh);
                        //}
                        //if (row == 82 || row == 119)
                        //{ }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"ReadExcel eror in row {row} : {e.Message}");
                    }

                }
            }
            Console.WriteLine($"Read {lst.Count} records from {Path.GetFileName(filePath)}");
            return lst;
        }
    }
}
