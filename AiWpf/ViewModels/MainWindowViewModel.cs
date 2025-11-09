using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using DataLayer.Classes;

namespace AiWpf.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<DtoWorkedHours> _workedHours;
        private DtoWorkedHours? _selectedWorkedHour;
        private int _month = 10;
        private int _year = 2025;
        private string _clientCode = "IMC";

        public MainWindowViewModel()
        {
            _workedHours = new ObservableCollection<DtoWorkedHours>();
            LoadDataCommand = new RelayCommand(LoadData);
      
            // Load initial data
            LoadData();
        }

        public ObservableCollection<DtoWorkedHours> WorkedHours
        {
            get => _workedHours;
            set
            {
                _workedHours = value;
                OnPropertyChanged();
            }
        }

        public DtoWorkedHours? SelectedWorkedHour
        {
            get => _selectedWorkedHour;
            set
            {
                _selectedWorkedHour = value;
                OnPropertyChanged();
            }
        }

        public int Month
        {
            get => _month;
            set
            {
                _month = value;
                OnPropertyChanged();
            }
        }

        public int Year
        {
            get => _year;
            set
            {
                _year = value;
                OnPropertyChanged();
            }
        }

        public string ClientCode
        {
            get => _clientCode;
            set
            {
                _clientCode = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoadDataCommand { get; }

        private void LoadData()
        {
            if (Globals.ConMan == null)
            {
                return;
            }

            List<DtoWorkedHours> hours = Globals.ConMan.GetDataClientMonth<DtoWorkedHours>(
                "GetDataClientMonth", 
                Month, 
                Year, 
                ClientCode, 
                out string message);
            Globals.ConMan.GetAllClients();

            WorkedHours.Clear();
            foreach (var item in hours)
            {
                WorkedHours.Add(item);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
