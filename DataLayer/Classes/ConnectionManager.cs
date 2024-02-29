using System.Data.SqlClient;

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

        public SqlConnection GetConnection()
        {
            try
            {
                string connectionStringTest = "Server=.;Database=malai_prod;Integrated Security=True;";
                SqlConnection connection = new SqlConnection(connectionStringTest);
                connection.Open();
                return connection;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
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
