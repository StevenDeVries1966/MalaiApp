using Microsoft.Extensions.Configuration;

namespace AiWpf
{
    /// <summary>
    /// Global configuration and utility methods for the AiWpf application
    /// </summary>
    public static class Globals
    {
        /// <summary>
        /// Gets the connection string from the appsettings.json configuration file
        /// </summary>
        /// <returns>The database connection string</returns>
        /// <exception cref="InvalidOperationException">Thrown when configuration cannot be read</exception>
        public static string GetConnectionString()
        {
            try
            {
                IConfiguration config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", false, true)
                    .Build();
                return config["connectionstring"] ?? "";
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to read configuration: {ex.Message}", ex);
            }
        }
    }
}