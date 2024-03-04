using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;

namespace DataLayer.Classes
{
    public class MalaiContext : DbContext
    {
        public ConnectionManager ConManager { get; set; }
        public List<DtoClient> lstClients { get; set; }
        public List<DtoJob> lstJobs { get; set; }
        public List<DtoEmployee> lstEmployee { get; set; }
        public List<DtoWorkedHours> lstWorkedHours { get; set; }
        public List<DtoWorkedHours> lst { get; set; }

        //public MalaiContext(string server, string database, string username, string password, bool allData = false)
        //{
        //    ConManager = new ConnectionManager(server, database, username, password);
        //    string message = "";
        //    if (allData)
        //    {
        //        lstClients = GetRecords<DtoClient>("GetAllClients", out message);
        //        lstJobs = GetRecords<DtoJob>("GetAllJobs", out message);
        //        lstEmployee = GetRecords<DtoEmployee>("GetAllEmployees", out message);
        //        lstWorkedHours = GetRecords<DtoWorkedHours>("GetAllWorkedHours", out message);
        //    }
        //}
        public MalaiContext(string connectionstring, bool allData = false)
        {
            ConManager = new ConnectionManager(connectionstring);
            string message = "";
            if (allData)
            {
                lstClients = GetRecords<DtoClient>("GetAllClients", out message);
                lstJobs = GetRecords<DtoJob>("GetAllJobs", out message);
                lstEmployee = GetRecords<DtoEmployee>("GetAllEmployees", out message);
                //lstWorkedHours = GetRecords<DtoWorkedHours>("GetAllWorkedHours", out message);
                GetAllWorkedHours();
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
            foreach (var wh in lstWorkedHours)
            {
                wh.Client = lstClients.Where(o => o.clt_code == wh.clt_code).FirstOrDefault();
                wh.Job = lstJobs.Where(o => o.job_id == wh.job_id).FirstOrDefault();
                wh.Employee = lstEmployee.Where(o => o.emp_id == wh.emp_id).FirstOrDefault();
            }
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
                using (SqlConnection con = ConManager.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand(storedProcedure, con))
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

        public List<T> GetDataClientMonth<T>(string storedProcedure, int month, int year, string clt_code, out string message) where T : new()
        {
            List<T> result = new List<T>();
            message = "OK";
            try
            {
                using (SqlConnection con = ConManager.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand(storedProcedure, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@month", month);
                        cmd.Parameters.AddWithValue("@year", year);
                        cmd.Parameters.AddWithValue("@clt_code", clt_code);
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
        public bool AddLog(string error_message, string stack, int emp_id)
        {
            bool bool_result = false;
            try
            {
                using (SqlConnection con = ConManager.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("AddLog", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        // Add parameters if your stored procedure has any
                        cmd.Parameters.AddWithValue("@message", error_message);
                        cmd.Parameters.AddWithValue("@stack", stack);
                        cmd.Parameters.AddWithValue("@emp_id", emp_id);
                        cmd.Parameters.AddWithValue("@date_created", DateTime.Now);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        string str_result = $"OK {rowsAffected} rows affected";
                        bool_result = true;
                    }
                }

            }
            catch (Exception e)
            {
                //str_result = e.Message;
            }

            return bool_result;
        }
        public bool AddWorkedHours(List<DtoWorkedHours> workedHours, out string message)
        {
            bool result = false;
            message = "OK";
            try
            {
                using (SqlConnection con = ConManager.GetConnection())
                {
                    foreach (DtoWorkedHours item in workedHours)
                    {
                        using (SqlCommand cmd = new SqlCommand("AddWorkedHours", con))
                        {
                            try
                            {
                                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                                // Add parameters if your stored procedure has any
                                cmd.Parameters.AddWithValue("@emp_code", item.emp_code);
                                cmd.Parameters.AddWithValue("@clt_code", item.clt_code);
                                cmd.Parameters.AddWithValue("@clt_job_code", item.clt_job_code);
                                cmd.Parameters.AddWithValue("@notes", item.notes ?? "");
                                cmd.Parameters.AddWithValue("@start_time", item.start_time);
                                cmd.Parameters.AddWithValue("@end_time", item.end_time);

                                int rowsAffected = cmd.ExecuteNonQuery();

                                //message += $"OK {rowsAffected} rows affected";
                                result = true;
                            }
                            catch (Exception e)
                            {
                            }

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
                        // values[0] = week
                        // values[1] = date
                        // values[2] = emp_id
                        // values[3] = clt_code
                        // values[4] = clientJobCode
                        // values[5] = notes
                        // values[6] = start
                        // values[7] = end
                        // values[8] = total hours

                        //string dateString = $"{values[1]} {values[6]}:00";
                        string dateString = ConvertToDateTime(values[1], values[6]);

                        DateTime start = DateTime.Parse(dateString);

                        //dateString = $"{values[1]} {values[7]}:00";
                        dateString = ConvertToDateTime(values[1], values[7]);

                        DateTime end = DateTime.Parse(dateString);
                        if (start.Year == 0001 || end.Year == 0001 || String.IsNullOrEmpty(values[2]) ||
                            String.IsNullOrEmpty(values[3]) ||
                            String.IsNullOrEmpty(values[4]))
                        {
                            message += "Error" + line + Environment.NewLine;
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

        private string ConvertToDateTime(string date, string time)
        {
            string result = default;
            if (time.Count(c => c == ':') > 1)
            {
                result = $"{date} {time}";
            }
            else
            {
                result = $"{date} {time}:00";
            }

            return result;
        }
    }
}
