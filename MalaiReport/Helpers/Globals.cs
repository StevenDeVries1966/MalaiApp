using DataLayer.Classes;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;

namespace MalaiReport.Helpers
{
    internal static class Globals
    {
        private static IConfiguration _config;
        private static string _connectionstring;
        private static string _pathInputTimeSheet;
        private static string _logoPath;
        private static string _reportPath;
        private static string _htmlTemplatePath;
        private static int _reportYear;
        private static List<int> _months;
        public static IConfiguration Config
        {
            get
            {
                _config = new ConfigurationBuilder()
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

        public static string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionstring))
                {
                    _connectionstring = GetConfigValue("DataBase:connectionstring");
                }
                return _connectionstring;
            }
        }
        public static string PathInputTimeSheet
        {
            get
            {
                if (string.IsNullOrEmpty(_pathInputTimeSheet))
                {
                    _pathInputTimeSheet = Path.Combine(GetConfigValue("Input:Path"), GetConfigValue("Input:ExcelTimeSheet"));
                }
                return _pathInputTimeSheet;
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
                return @"..\\Images\\MalaiEffinciency_1563x1563.png";
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
        public static string HtmlTemplatePath
        {
            get
            {
                if (string.IsNullOrEmpty(_htmlTemplatePath))
                {
                    _htmlTemplatePath = GetConfigValue("HtmlTemplatePath");
                }
                return _htmlTemplatePath;
            }
            set => _htmlTemplatePath = value;
        }
        public static int Year
        {
            get
            {
                _reportYear = Convert.ToInt32(GetConfigValue("Report:Year"));
                return _reportYear;
            }
            set => _reportYear = value;
        }
        public static List<int> Months
        {
            get
            {
                string strMonths = GetConfigValue("Report:Months");
                _months = strMonths.Split(',')
                    .Select(s => int.Parse(s))
                    .ToList();
                return _months;
            }
            set => _months = value;
        }

        public static MalaiContext? ConMan { get; set; }
        public static List<DtoEmployee>? Employees { get; set; }
        public static DtoEmployee? EmployeeCurrent;

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
