using System.Threading.Tasks;

namespace WpfUI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {

        private readonly WorkedHoursViewModel _workedHoursViewModel;
        private ViewModelBase? _selectedViewModel;

        public MainViewModel(WorkedHoursViewModel workedHoursViewModel)
        {
            _workedHoursViewModel = workedHoursViewModel;
            _selectedViewModel = _workedHoursViewModel;
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

        public async override Task LoadAsync()
        {
            if (SelectedViewModel is not null)
            {
                await SelectedViewModel.LoadAsync();
            }
        }
    }
}
