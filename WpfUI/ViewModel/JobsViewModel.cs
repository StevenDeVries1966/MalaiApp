using DataLayer.Classes;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WpfUI.Data;

namespace WpfUI.ViewModel
{
    public class JobsViewModel : ViewModelBase
    {
        private readonly IMalaiDataProvider _malaiDataProvider;
        private JobsItemViewModel? _selectedJob;

        public bool IsEmployeeSelected => SelectedJob is not null;

        public JobsViewModel(IMalaiDataProvider malaiDataProvider)
        {
            _malaiDataProvider = malaiDataProvider;
        }
        public ObservableCollection<JobsItemViewModel> Jobs { get; } = new();
        public JobsItemViewModel? SelectedJob
        {
            get => _selectedJob;
            set
            {
                _selectedJob = value;
                RaisePropertyChanged();
            }
        }

        public async override Task LoadAsync()
        {
            if (Jobs.Any())
            {
                return;
            }
            List<DtoJob>? lst = await _malaiDataProvider.GetJobsAsync();
            if (lst is not null)
            {
                foreach (DtoJob job in lst)
                {
                    Jobs.Add(new JobsItemViewModel(job));
                }
            }
        }
    }
}
