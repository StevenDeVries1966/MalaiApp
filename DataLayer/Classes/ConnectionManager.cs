using MySql.Data.MySqlClient;

namespace DataLayer.Classes
{
    public class ConnectionManager
    {
        private readonly string connectionString;

        public ConnectionManager(string server, string database, string username, string password)
        {
            // Build the connection string
            connectionString = $"Server={server};Database={database};User ID={username};Password={password};";
        }

        public MySqlConnection GetConnection()
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();
                return connection;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public void CloseConnection(MySqlConnection connection)
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
