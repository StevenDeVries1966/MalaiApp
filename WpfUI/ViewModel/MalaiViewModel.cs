using DataLayer.Classes;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WpfUI.Data;

namespace WpfUI.ViewModel
{
    public class MalaiViewModel
    {
        private readonly IMalaiDataProvider _malaiDataProvider;

        public MalaiViewModel(IMalaiDataProvider malaiDataProvider)
        {
            _malaiDataProvider = malaiDataProvider;
        }
        public ObservableCollection<DtoEmployee> Employees { get; } = new();
        public ObservableCollection<DtoClient> Clients { get; } = new();
        public ObservableCollection<DtoJob> Jobs { get; } = new();
        public ObservableCollection<DtoWorkedHours> WorkedHours { get; } = new();

        public async Task LoadDataContextAsync()
        {
            if (Clients.Any())
            {
                return;
            }
            MalaiContext? ctx = await _malaiDataProvider.GetDataContextAsync();
            if (ctx is not null)
            {
                foreach (DtoEmployee emp in ctx.lstEmployee)
                {
                    Employees.Add(emp);
                }
                foreach (DtoClient clt in ctx.lstClients)
                {
                    Clients.Add(clt);
                }
                foreach (DtoJob job in ctx.lstJobs)
                {
                    Jobs.Add(job);
                }
                foreach (DtoWorkedHours wh in ctx.lstWorkedHours)
                {
                    WorkedHours.Add(wh);
                }
            }

        }
    }
}
