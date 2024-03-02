using DataLayer.Classes;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Reflection;

namespace WpfUI.Helpers
{
    internal static class Globals
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
                return $"{assembly.GetName().Name} - v{assembly.GetName()?.Version?.Build}.{assembly.GetName()?.Version?.Major}.{assembly.GetName()?.Version?.Minor}";
            }
        }
        public static string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    _connectionString = GetConfigValue("connectionstring");
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
