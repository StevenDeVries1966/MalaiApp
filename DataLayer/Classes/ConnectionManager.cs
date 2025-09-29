using System.Data.SqlClient;

namespace DataLayer.Classes
{
    public class ConnectionManager
    {
        private readonly string _connectionString;

        public ConnectionManager(string server, string database, string username, string password)
        {
            // Build the connection string
            _connectionString = $"Server={server};Database={database};User ID={username};Password={password};";
        }
        public ConnectionManager(string connectionString)
        {
            // Build the connection string
            _connectionString = connectionString;
        }
        public SqlConnection? GetConnection()
        {
            try
            {
                SqlConnection connection = new SqlConnection(_connectionString);
                connection.Open();
                return connection;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            return null;
        }

        public void CloseConnection(SqlConnection connection)
        {
            try
            {
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

    }
}
