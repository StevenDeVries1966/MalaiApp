namespace MalaiReport
{
    public class AssistHtml
    {
        public static string GetHtmlResourceContent(string resourceName)
        {
            string content = "";

            // Read the content of the embedded resource
            using (StreamReader reader = new StreamReader(resourceName))
            {
                content = reader.ReadToEnd();
            }

            return content;
        }
        public static void SaveHtmlToFile(string htmlContent, string fileName)
        {
            File.WriteAllText(fileName, htmlContent);
        }
    }
}
