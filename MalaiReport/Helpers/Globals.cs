using DataLayer.Classes;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;

namespace MalaiReport.Helpers
{
    internal static class Globals
    {
        private static IConfiguration _config;
        private static string _server;
        private static string _database;
        private static string _username;
        private static string _password;
        private static string _logoPath;
        private static string _reportPath;
        public static IConfiguration Config
        {
            get
            {
                _config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", false, true)
                    .AddJsonFile("appsettings.Development.json", true, true)
                    .Build();
                return _config;
            }
            set => _config = value;
        }

        private static string GetConfigValue(string key)
        {
            return Config[key];
        }
        public static string Server
        {
            get
            {
                if (string.IsNullOrEmpty(_server))
                {
                    _server = GetConfigValue("Server");
                }
                return _server;
            }
        }
        public static string DataBase
        {
            get
            {
                if (string.IsNullOrEmpty(_database))
                {
                    _database = GetConfigValue("DataBase");
                }
                return _database;
            }
        }
        public static string UserName
        {
            get
            {
                if (string.IsNullOrEmpty(_username))
                {
                    _username = GetConfigValue("UserName");
                }
                return _username;
            }
        }
        public static string PassWord
        {
            get
            {
                if (string.IsNullOrEmpty(_password))
                {
                    _password = GetConfigValue("PassWord");
                }
                return _password;
            }
        }
        public static string LogoPath
        {
            get
            {
                if (string.IsNullOrEmpty(_logoPath))
                {
                    _logoPath = GetConfigValue("LogoPath");
                    _logoPath = GetPath(_logoPath);
                }
                return _logoPath;
            }
        }
        public static string ReportPath
        {
            get
            {
                if (string.IsNullOrEmpty(_reportPath))
                {
                    _reportPath = GetConfigValue("ReportPath");
                }
                return _reportPath;
            }
            set => _reportPath = value;
        }

        public static MalaiContext? ConMan { get; set; }

        public static string GetPath(string relativePath)
        {
            var appPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string pattern = @"^(.+\\)(.+exe)$";
            Regex regex = new Regex(pattern, RegexOptions.None);
            var match = regex.Match(appPath);
            return Path.GetFullPath(match.Groups[1].Value + relativePath);
        }
    }
}
