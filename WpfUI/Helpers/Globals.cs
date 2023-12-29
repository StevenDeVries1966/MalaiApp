using Microsoft.Extensions.Configuration;

namespace WpfUI.Helpers
{
    internal static class Globals
    {
        private static string? _server;
        private static string _database;
        private static string _username;
        private static string _password;

        public static string Server
        {
            get
            {
                if (string.IsNullOrEmpty(_server))
                {
                    _server = GetConfigValue("dataparameters:server");
                }
                return _server;
            }
        }
        public static string Database
        {
            get
            {
                if (string.IsNullOrEmpty(_database))
                {
                    _database = GetConfigValue("dataparameters:database");
                }
                return _database;
            }
        }
        public static string Username
        {
            get
            {
                if (string.IsNullOrEmpty(_username))
                {
                    _username = GetConfigValue("dataparameters:username");
                }
                return _username;
            }
        }
        public static string Password
        {
            get
            {
                if (string.IsNullOrEmpty(_password))
                {
                    _password = GetConfigValue("dataparameters:password");
                }
                return _password;
            }
        }
        private static string GetConfigValue(string key)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile("appsettings.Development.json", true, true)
                .Build();
            return config[key];
        }
    }
}
