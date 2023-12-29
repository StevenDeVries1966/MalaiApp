using MySql.Data.MySqlClient;
using System.Data;
using System.Reflection;

namespace DataLayer.Classes
{
    public class MalaiContext
    {
        public ConnectionManager ConManager { get; set; }
        public List<DtoClient> lstClients { get; set; }
        public List<DtoJob> lstJobs { get; set; }
        public List<DtoEmployee> lstEmployee { get; set; }
        public List<DtoWorkedHours> lstWorkedHours { get; set; }
        public List<DtoWorkedHours> lst { get; set; }

        public MalaiContext(string server, string database, string username, string password, bool allData = false)
        {
            ConManager = new ConnectionManager(server, database, username, password);
            string message = "";
            if (allData)
            {
                lstClients = GetRecords<DtoClient>("GetAllClients", out message);
                lstJobs = GetRecords<DtoJob>("GetAllJobs", out message);
                lstEmployee = GetRecords<DtoEmployee>("GetAllEmployees", out message);
                lstWorkedHours = GetRecords<DtoWorkedHours>("GetAllWorkedHours", out message);
            }
        }
        public string GetAllClients()
        {
            string message = "";
            lstClients = GetRecords<DtoClient>("GetAllClients", out message);
            return message;
        }
        public string GetAllJobs()
        {
            string message = "";
            lstJobs = GetRecords<DtoJob>("GetAllJobs", out message);
            return message;
        }
        public string GetAllEmployees()
        {
            string message = "";
            lstEmployee = GetRecords<DtoEmployee>("GetAllEmployees", out message);
            return message;
        }
        public string GetAllWorkedHours()
        {
            string message = "";
            lstWorkedHours = GetRecords<DtoWorkedHours>("GetAllWorkedHours", out message);
            return message;
        }
        public List<T> GetRecords<T>(string storedProcedure, out string message)
        {
            List<T> result = new List<T>();
            message = "OK";
            try
            {
                using (MySqlConnection con = ConManager.GetConnection())
                {
                    using (MySqlCommand cmd = new MySqlCommand(storedProcedure, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        using (IDataReader reader = cmd.ExecuteReader())
                        {
                            result = DataReaderMapToList<T>(reader, out message);
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
        public static List<T> DataReaderMapToList<T>(IDataReader dr, out string message)
        {
            message = "OK";
            List<T> list = new List<T>();
            T obj = default(T);
            try
            {
                while (dr.Read())
                {
                    obj = Activator.CreateInstance<T>();
                    foreach (PropertyInfo prop in obj.GetType().GetProperties())
                    {
                        try
                        {
                            if (!object.Equals(dr[prop.Name], DBNull.Value))
                            {
                                prop.SetValue(obj, dr[prop.Name], null);
                            }
                        }
                        catch (Exception e)
                        {
                            if (!e.Message.StartsWith("Could not find specified column in results"))
                            {
                                message = message + e.Message + Environment.NewLine;
                            }
                            continue;
                        }
                    }
                    list.Add(obj);
                }
            }
            catch (Exception e)
            {
                message = message + e.Message + Environment.NewLine;
            }
            return list;
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
                            cmd.Parameters.AddWithValue("_emp_code", item.emp_code);
                            cmd.Parameters.AddWithValue("_clt_code", item.clt_code);
                            cmd.Parameters.AddWithValue("_notes", item.notes);
                            cmd.Parameters.AddWithValue("_start_time", item.start_time);
                            cmd.Parameters.AddWithValue("_end_time", item.end_time);

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
