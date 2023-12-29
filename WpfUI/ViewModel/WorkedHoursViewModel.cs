using DataLayer.Classes;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WpfUI.Data;

namespace WpfUI.ViewModel
{
    internal class WorkedHoursViewModel
    {
        private readonly IMalaiDataProvider _malaiDataProvider;

        public WorkedHoursViewModel(IMalaiDataProvider malaiDataProvider)
        {
            _malaiDataProvider = malaiDataProvider;
        }
        public ObservableCollection<DtoWorkedHours> WorkedHours { get; } = new();
        public DtoWorkedHours? SelectedWorkedHours { get; set; }
        public async Task LoadWorkedHoursAsync()
        {
            if (WorkedHours.Any())
            {
                return;
            }
            List<DtoWorkedHours>? lst = await _malaiDataProvider.GetWorkedHoursAsync();
            if (lst is not null)
            {
                foreach (DtoWorkedHours emp in lst)
                {
                    WorkedHours.Add(emp);
                }
            }
        }
    }

}
