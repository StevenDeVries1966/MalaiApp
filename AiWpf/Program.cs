using DataLayer.Classes;
using DocumentFormat.OpenXml.Bibliography;
using System.Windows;

namespace AiWpf
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Globals.ConMan = new MalaiContext(Globals.ConnectionString);
            List<DtoWorkedHours> lstWorkedHours  = Globals.ConMan?.GetDataClientMonth<DtoWorkedHours>("GetDataClientMonth", 10, 2025, "IMC", out _)!;
        }
    }
}
