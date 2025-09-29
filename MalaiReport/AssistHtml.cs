//using DinkToPdf;

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
        //public static void ConvertHtmlToPdf(string htmlContent, string outputPath)
        //{
        //    var converter = new SynchronizedConverter(new PdfTools());

        //    // Configure the PDF document
        //    var doc = new HtmlToPdfDocument()
        //    {
        //        GlobalSettings = new GlobalSettings
        //        {
        //            ColorMode = ColorMode.Color,
        //            Orientation = Orientation.Portrait,
        //            PaperSize = PaperKind.A4,
        //            Out = outputPath // Output file path
        //        },
        //        Objects = {
        //            new ObjectSettings
        //            {
        //                HtmlContent = htmlContent, // HTML content to convert
        //                WebSettings = { DefaultEncoding = "utf-8" }, // Set encoding
        //            }
        //        }
        //    };

        //    // Convert the HTML to PDF
        //    converter.Convert(doc);

        //    Console.WriteLine($"PDF generated successfully at {outputPath}");
        //}
    }

}
