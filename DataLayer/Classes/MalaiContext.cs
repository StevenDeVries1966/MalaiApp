using MySql.Data.MySqlClient;

namespace DataLayer.Classes
{
    public class MalaiContext
    {
        public ConnectionManager ConManager { get; set; }
        public List<DtoWorkedHours> lst { get; set; }


        public MalaiContext(string server, string database, string username, string password)
        {
            ConManager = new ConnectionManager(server, database, username, password);

        }

        public bool AddWorkedHours(List<DtoWorkedHours> workedHours, out string message)
        {
            bool result = false;
            message = "OK";
            try
            {
                using (MySqlConnection con = ConManager.GetConnection())
                {
                    foreach (DtoWorkedHours item in workedHours)
                    {
                        using (MySqlCommand cmd = new MySqlCommand("AddWorkedHours", con))
                        {
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;

                            // Add parameters if your stored procedure has any
                            cmd.Parameters.AddWithValue("_emp_code", item.EmployeeCode);
                            cmd.Parameters.AddWithValue("_clt_code", item.ClientCode);
                            cmd.Parameters.AddWithValue("_notes", item.Notes);
                            cmd.Parameters.AddWithValue("_start_time", item.Start);
                            cmd.Parameters.AddWithValue("_end_time", item.End);

                            int rowsAffected = cmd.ExecuteNonQuery();

                            message += $"OK {rowsAffected} rows affected";
                            result = true;
                        }
                    }
                }

            }
            catch (Exception e)
            {
                message = e.Message;
            }

            return result;
        }
        public bool ReadCsv(string csvPath)
        {
            bool result = false;
            try
            {
                lst = new List<DtoWorkedHours>();
                // Open the file with a StreamReader
                using (StreamReader reader = new StreamReader(csvPath))
                {
                    // Read each line until the end of the file
                    Console.WriteLine($"Reading {Path.GetFileName(csvPath)}");
                    bool skipHeaders = false;
                    string header = "";
                    string message = "";
                    int addedRecords = 0;
                    while (!reader.EndOfStream)
                    {
                        // Read the line
                        string line = reader.ReadLine();

                        if (!skipHeaders)
                        {
                            header = line + Environment.NewLine;
                            skipHeaders = true;
                            continue;
                        }

                        // Split the line into an array of strings using a comma as the delimiter
                        string[] values = line.Split(';');

                        string dateString = $"{values[1]} {values[6]}:00";
                        DateTime start = DateTime.Parse(dateString);
                        dateString = $"{values[1]} {values[7]}:00";

                        DateTime end = DateTime.Parse(dateString);
                        if (start.Year == 0001 || end.Year == 0001 || String.IsNullOrEmpty(values[2]) ||
                            String.IsNullOrEmpty(values[3]))
                        {
                            message += line + Environment.NewLine;
                            continue;
                        }

                        DtoWorkedHours wh = new DtoWorkedHours(values[2], values[3], start, end, values[5]);
                        lst.Add(wh);
                        ++addedRecords;
                    }

                    if (!String.IsNullOrEmpty(message))
                    {
                        message = header + Environment.NewLine;
                    }
                    message += $"Read {addedRecords} records" + Environment.NewLine;
                    Console.WriteLine(message);
                    AddWorkedHours(lst, out message);
                    Console.WriteLine(message);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            return result;
        }
    }
}
