using MySql.Data.MySqlClient;
using System.Data;

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
        public static List<T> MapToList<T>(IDataReader reader) where T : new()
        {
            List<T> resultList = new List<T>();

            while (reader.Read())
            {
                T obj = MapToObject<T>(reader);
                resultList.Add(obj);
            }
            return resultList;
        }

        private static T MapToObject<T>(IDataRecord record) where T : new()
        {
            T obj = new T();

            for (int i = 0; i < record.FieldCount; i++)
            {
                if (!record.IsDBNull(i))
                {
                    string propertyName = record.GetName(i);
                    object value = record[i];

                    // Use reflection to set the property value
                    typeof(T).GetProperty(propertyName)?.SetValue(obj, value, null);
                }
            }

            return obj;
        }
        public List<T> GetRecords<T>(string storedProcedure, out string message) where T : new()
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

                        DateTime start = DateTime.Now;
                        using (IDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                T obj = MapToObject<T>(reader);
                                result.Add(obj);
                            }
                        }

                        DateTime end = DateTime.Now;
                        TimeSpan ts = end - start;
                    }
                }
            }
            catch (Exception e)
            {
                message = e.Message;
            }
            return result;
        }

        public List<T> GetDataClientMonth<T>(string storedProcedure, out string message) where T : new()
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
                        cmd.Parameters.AddWithValue("_month", 8);
                        cmd.Parameters.AddWithValue("_clt_code", "IMC");
                        DateTime start = DateTime.Now;
                        using (IDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                T obj = MapToObject<T>(reader);
                                result.Add(obj);
                            }
                        }

                        DateTime end = DateTime.Now;
                        TimeSpan ts = end - start;
                    }
                }
            }
            catch (Exception e)
            {
                message = e.Message;
            }
            return result;
        }
        //public List<DtoWorkedHours> GetDataClientMonth(int month, string clt_code, out string message)
        //{
        //    bool result = false;
        //    message = "OK";
        //    try
        //    {
        //        using (MySqlConnection con = ConManager.GetConnection())
        //        {

        //            using (MySqlCommand cmd = new MySqlCommand("GetDataClientMonth", con))
        //            {
        //                cmd.CommandType = System.Data.CommandType.StoredProcedure;

        //                // Add parameters if your stored procedure has any
        //                cmd.Parameters.AddWithValue("_month", month);
        //                cmd.Parameters.AddWithValue("_clt_code", clt_code);
        //                int rowsAffected = cmd.ExecuteNonQuery();

        //                message += $"OK {rowsAffected} rows affected";
        //                result = true;
        //            }
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        message = e.Message;
        //    }

        //    return result;
        //}
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
                            cmd.Parameters.AddWithValue("_clt_job_code", item.clt_job_code);
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


                        DtoWorkedHours wh = new DtoWorkedHours(values[2], values[3], values[4], start, end, values[5]);
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
