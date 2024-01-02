using System.Threading.Tasks;
using WpfUI.Command;

namespace WpfUI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {


        private ViewModelBase? _selectedViewModel;

        public MainViewModel(
            WorkedHoursViewModel workedHoursViewModel,
            JobsViewModel jobsViewModel,
            EmployeeViewModel employeeViewModel,
            ClientsViewModel clientsViewModel)
        {
            WorkedHoursViewModel = workedHoursViewModel;
            JobsViewModel = jobsViewModel;
            EmployeeViewModel = employeeViewModel;
            ClientsViewModel = clientsViewModel;
            _selectedViewModel = employeeViewModel;
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
        public EmployeeViewModel EmployeeViewModel { get; }
        public ClientsViewModel ClientsViewModel { get; }
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
