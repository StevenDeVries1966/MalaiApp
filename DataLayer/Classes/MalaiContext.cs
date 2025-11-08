        using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;

namespace DataLayer.Classes
{
    public class MalaiContext : DbContext
    {
        public ConnectionManager ConManager { get; set; }
        public List<DtoClient> LstClients { get; set; }
        public List<DtoJob> LstJobs { get; set; }
        public List<DtoEmployee> LstEmployee { get; set; }
        public List<DtoWorkedHours> LstWorkedHours { get; set; }
        public List<DtoWorkedHours> Lst { get; set; }

        public MalaiContext(string con, bool allData = false)
        {
            ConManager = new ConnectionManager(con);
            if (allData)
            {
                LstClients = GetRecords<DtoClient>("GetAllClients", out var message);
                if (message != "OK") WriteErrorMessage(message);
                LstJobs = GetRecords<DtoJob>("GetAllJobs", out message);
                if (message != "OK") WriteErrorMessage(message);
                LstEmployee = GetRecords<DtoEmployee>("GetAllEmployees", out message);
                if (message != "OK") WriteErrorMessage(message);
            }
        }
        public void WriteErrorMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor(); // Reset to default color
            AddLog("Test error message", "Stack trace test", 1);
        }
        public string GetAllClients()
        {
            LstClients = GetRecords<DtoClient>("GetAllClients", out var message);
            if (message != "OK") WriteErrorMessage(message);
            return message;
        }
        public string GetAllJobs()
        {
            LstJobs = GetRecords<DtoJob>("GetAllJobs", out var message);
            if (message != "OK") WriteErrorMessage(message);
            return message;
        }
        public string GetAllEmployees()
        {
            LstEmployee = GetRecords<DtoEmployee>("GetAllEmployees", out var message);
            if (message != "OK") WriteErrorMessage(message);
            return message;
        }
        public string GetAllWorkedHours(int month, int year)
        {
            string message = "";
            LstWorkedHours = GetDataClientMonth<DtoWorkedHours>("GetDataClientMonth", month, year, "", out _);

            foreach (var wh in LstWorkedHours)
            {
                wh.Client = LstClients.FirstOrDefault(o => o.clt_code == wh.clt_code);
                wh.Job = LstJobs.FirstOrDefault(o => o.job_id == wh.job_id);
                wh.Employee = LstEmployee.FirstOrDefault(o => o.emp_id == wh.emp_id);
            }
            if (message != "OK") WriteErrorMessage(message);
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
                using (SqlConnection? con = ConManager.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand(storedProcedure, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (IDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                T obj = MapToObject<T>(reader);
                                result.Add(obj);
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

        public List<T> GetDataClientMonth<T>(string storedProcedure, int month, int year, string cltCode, out string message) where T : new()
        {
            List<T> result = new List<T>();
            message = "OK";
            try
            {
                using (SqlConnection? con = ConManager.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand(storedProcedure, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@month", month);
                        cmd.Parameters.AddWithValue("@year", year);
                        cmd.Parameters.AddWithValue("@clt_code", cltCode);
                        using (IDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                T obj = MapToObject<T>(reader);
                                result.Add(obj);
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
        public bool AddLog(string errorMessage, string stack, int empId)
        {
            bool boolResult = false;
            try
            {
                using (SqlConnection? con = ConManager.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("AddLog", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Add parameters if your stored procedure has any
                        cmd.Parameters.AddWithValue("@message", errorMessage);
                        cmd.Parameters.AddWithValue("@stack", stack);
                        cmd.Parameters.AddWithValue("@emp_id", empId);
                        cmd.Parameters.AddWithValue("@date_created", DateTime.Now);

                        cmd.ExecuteNonQuery();

                        boolResult = true;
                    }
                }

            }
            catch (Exception e)
            {
                WriteErrorMessage(e.Message);
            }

            return boolResult;
        }
        public bool AddWorkedHours(List<DtoWorkedHours> workedHours, out string message)
        {
            bool result = false;
            message = "OK";
            try
            {
                using (SqlConnection? con = ConManager.GetConnection())
                {
                    foreach (DtoWorkedHours item in workedHours)
                    {
                        using (SqlCommand cmd = new SqlCommand("AddWorkedHours", con))
                        {
                            try
                            {
                                cmd.CommandType = CommandType.StoredProcedure;

                                // Add parameters if your stored procedure has any
                                cmd.Parameters.AddWithValue("@emp_code", item.emp_code);
                                cmd.Parameters.AddWithValue("@clt_code", item.clt_code);
                                cmd.Parameters.AddWithValue("@clt_job_code", item.clt_job_code);
                                // ReSharper disable once NullCoalescingConditionIsAlwaysNotNullAccordingToAPIContract
                                cmd.Parameters.AddWithValue("@notes", item.notes ?? "");
                                cmd.Parameters.AddWithValue("@start_time", item.start_time);
                                cmd.Parameters.AddWithValue("@end_time", item.end_time);

                                cmd.ExecuteNonQuery();

                                result = true;
                            }
                            catch (Exception e)
                            {
                                WriteErrorMessage(e.Message);
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
        
        public bool AddClient(DtoClient client, out string message)
        {
            bool result = false;
            message = "OK";
            try
            {
                using (SqlConnection? con = ConManager.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("AddClient", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Add parameters for the stored procedure
                        cmd.Parameters.AddWithValue("@clt_code", client.clt_code);
                        cmd.Parameters.AddWithValue("@clt_name", client.clt_name);
                        cmd.Parameters.AddWithValue("@address", client.address);
                        cmd.Parameters.AddWithValue("@postalcode", client.postalcode);
                        cmd.Parameters.AddWithValue("@city", client.city);
                        cmd.Parameters.AddWithValue("@country", client.country);
                        cmd.Parameters.AddWithValue("@email", client.email);
                        cmd.Parameters.AddWithValue("@phone", client.phone);
                        cmd.Parameters.AddWithValue("@rate_ES001", client.rate_ES001);
                        cmd.Parameters.AddWithValue("@rate_AS001", client.rate_AS001);
                        cmd.Parameters.AddWithValue("@retainer_ES001", client.retainer_ES001);
                        cmd.Parameters.AddWithValue("@retainer_AS001", client.retainer_AS001);
                        cmd.Parameters.AddWithValue("@report_type", client.report_type);

                        // Execute the stored procedure and check if it returns a result set
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                // Check if it's an error message or success message
                                if (reader.FieldCount > 0)
                                {
                                    string resultMessage = reader["Message"].ToString() ?? "";
                                    if (resultMessage.Contains("successfully"))
                                    {
                                        message = resultMessage;
                                        result = true;
                                    }
                                    else
                                    {
                                        message = reader["ErrorMessage"].ToString() ?? "Unknown error";
                                        result = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                message = e.Message;
                result = false;
            }

            return result;
        }
        
        public bool DeleteWorkedHours(List<int> months, int year, out string message)
        {
            bool result = false;
            message = "";
            try
            {
                using (SqlConnection? con = ConManager.GetConnection())
                {

                    using (SqlCommand cmd = new SqlCommand("DeleteWorkedHours", con))
                    {
                        try
                        {
                            foreach (int month in months)
                            {
                                cmd.CommandType = CommandType.StoredProcedure;

                                // Add parameters if your stored procedure has any
                                cmd.Parameters.AddWithValue("@month", month);
                                cmd.Parameters.AddWithValue("@year", year);

                                int rowsAffected = cmd.ExecuteNonQuery();

                                message += $"{rowsAffected} rows deleted from {month}-{year}";
                                result = true;
                            }

                        }
                        catch (Exception e)
                        {
                            result = false;
                            message += e.Message;
                            Console.WriteLine(e.Message);
                        }
                    }

                }

            }
            catch (Exception e)
            {
                result = false;
                message += e.Message;
                Console.WriteLine(e.Message);
            }
            return result;
        }
        // Add this method to MalaiContext to fix CS1061
        public bool UpdateClient(DtoClient client, out string message)
        {
            try
            {
                // Find the client by clt_code
                var existingClient = LstClients.FirstOrDefault(c => c.clt_code == client.clt_code);
                if (existingClient == null)
                {
                    message = "Client not found.";
                    return false;
                }

                // Update properties
                existingClient.clt_name = client.clt_name;
                existingClient.address = client.address;
                existingClient.postalcode = client.postalcode;
                existingClient.city = client.city;
                existingClient.country = client.country;
                existingClient.email = client.email;
                existingClient.phone = client.phone;
                existingClient.rate_ES001 = client.rate_ES001;
                existingClient.rate_AS001 = client.rate_AS001;
                existingClient.retainer_ES001 = client.retainer_ES001;
                existingClient.retainer_AS001 = client.retainer_AS001;
                existingClient.report_type = client.report_type;

                // If using EF, update and save changes
                // Entry(existingClient).State = EntityState.Modified;
                // SaveChanges();

                message = "OK";
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }
        public bool ReadCsv(string csvPath)
        {
            bool result = false;
            string debugLine = "";
            int intLine = 0;
            try
            {
                Lst = new List<DtoWorkedHours>();
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
                        string line = reader.ReadLine()!;
                        debugLine = line;
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
                        Lst.Add(wh);
                        ++addedRecords;
                        ++intLine;
                    }

                    if (!String.IsNullOrEmpty(message))
                    {
                        message = header + Environment.NewLine;
                    }
                    message += $"Read {addedRecords} records" + Environment.NewLine;
                    Console.WriteLine(message);
                    AddWorkedHours(Lst, out message);
                    Console.WriteLine(message);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message + Environment.NewLine + intLine.ToString() + " : " + debugLine);
            }

            return result;
        }

        private string ConvertToDateTime(string date, string time)
        {
            string result;
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
