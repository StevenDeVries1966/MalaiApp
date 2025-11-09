using DataLayer.Classes;
using DocumentFormat.OpenXml.Bibliography;
using System.Windows;
using AiWpf.Views;

namespace AiWpf
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Globals.ConMan = new MalaiContext(Globals.ConnectionString);
            
            // Create and show the main window
            var mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
