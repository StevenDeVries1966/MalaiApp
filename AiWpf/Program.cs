using System.Windows;

namespace AiWpf
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Launch the DtoClientWindow on startup
            var dtoClientWindow = new AddClientWindow();
            dtoClientWindow.Show();
            //// Launch the DtoClientWindow on startup
            //var dtoClientWindow = new DtoClientWindow();
            //dtoClientWindow.Show();
        }
    }
}
