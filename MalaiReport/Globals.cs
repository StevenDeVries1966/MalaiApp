using DataLayer.Classes;
using System.Text.RegularExpressions;

namespace MalaiReport
{
    internal static class Globals
    {
        public static MalaiContext? ConMan { get; set; }
        public static string ImagePath => GetPath(@"Images\MalaiEffinciency_200x200.png");
        public static string GetPath(string relativePath)
        {
            var appPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string pattern = @"^(.+\\)(.+exe)$";
            Regex regex = new Regex(pattern, RegexOptions.None);
            var match = regex.Match(appPath);
            return System.IO.Path.GetFullPath(match.Groups[1].Value + relativePath);
        }
    }
}
