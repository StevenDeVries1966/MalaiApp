using DataLayer.Classes;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Malai.UI.Helpers
{
    public static class Globals
    {
        private static string? _connectionString;
        private static string? _server;
        private static string _database;
        private static string _username;
        private static string _password;
        public static List<DtoEmployee>? Employees { get; set; }
        public static DtoEmployee? Employee_Current;
        public static string Current_Emp_Code => "ES001";
        public static string? MainFormTitle
        {
            get
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                return $"{assembly.GetName().Name} - v{assembly.GetName()?.Version?.Build}.{assembly.GetName()?.Version?.Major}.{assembly.GetName()?.Version?.Minor} (server = {_server} || database = {_database})";
            }
        }
        public static string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    _connectionString = GetConfigValue("connectionstring");
                    string[] arConStr = _connectionString.Split(';');

                    _server = arConStr[0].Split('=')[1];
                    _database = arConStr[1].Split('=')[1];
                }
                return _connectionString;
            }
        }
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

        public static string SelectedMonth { get; set; }
        public static int SelectedYear { get; set; }
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
