using System.Threading.Tasks;
using WpfUI.Command;

namespace WpfUI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {


        private ViewModelBase? _selectedViewModel;

        public MainViewModel(WorkedHoursViewModel workedHoursViewModel, JobsViewModel jobsViewModel)
        {
            WorkedHoursViewModel = workedHoursViewModel;
            JobsViewModel = jobsViewModel;
            _selectedViewModel = WorkedHoursViewModel;
            SelectViewModelCommand = new DelegateCommand(SelectViewModel);
        }

        public ViewModelBase? SelectedViewModel
        {
            get => _selectedViewModel;
            set
            {
                _selectedViewModel = value;
                RaisePropertyChanged();
            }
        }
        public WorkedHoursViewModel WorkedHoursViewModel { get; }
        public JobsViewModel JobsViewModel { get; }
        public DelegateCommand SelectViewModelCommand { get; }
        public async override Task LoadAsync()
        {
            if (SelectedViewModel is not null)
            {
                await SelectedViewModel.LoadAsync();
            }
        }
        private async void SelectViewModel(object? parameter)
        {
            SelectedViewModel = parameter as ViewModelBase;
            await LoadAsync();
        }
    }
}
