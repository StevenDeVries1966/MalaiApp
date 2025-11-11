using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using DataLayer.Classes;
using System.Linq;

namespace AiWpf.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<DtoWorkedHours> _workedHours;
        private ObservableCollection<DtoClient> _clients;
        private ObservableCollection<DtoEmployee> _employees;
        private ObservableCollection<DtoJob> _jobs;
        private DtoWorkedHours? _selectedWorkedHour;
        private int _month = 10;
        private int _year = 2025;
        private string _clientCode = "IMC";
        private List<int> _months;
        private List<int> _years;
        private bool _isInitializing = true;

        public MainWindowViewModel()
        {
            _workedHours = new ObservableCollection<DtoWorkedHours>();
            _clients = new ObservableCollection<DtoClient>();
            _employees = new ObservableCollection<DtoEmployee>();
            _jobs = new ObservableCollection<DtoJob>();
            _months = Enumerable.Range(1, 12).ToList();
            _years = Enumerable.Range(2023, 8).ToList(); // 2023 to 2030

            // Load clients
            LoadClients();

            // Load employees
            LoadEmployees();

            // Load jobs for initial client
            LoadJobs();

            // Load initial data
            _isInitializing = false;
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

        public ObservableCollection<DtoClient> Clients
        {
            get => _clients;
            set
            {
                _clients = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<DtoEmployee> Employees
        {
            get => _employees;
            set
            {
                _employees = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<DtoJob> Jobs
        {
            get => _jobs;
            set
            {
                _jobs = value;
                OnPropertyChanged();
            }
        }

        public List<int> Months
        {
            get => _months;
            set
            {
                _months = value;
                OnPropertyChanged();
            }
        }

        public List<int> Years
        {
            get => _years;
            set
            {
                _years = value;
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
                if (_month != value)
                {
                    _month = value;
                    OnPropertyChanged();
                    if (!_isInitializing)
                    {
                        LoadData();
                    }
                }
            }
        }

        public int Year
        {
            get => _year;
            set
            {
                if (_year != value)
                {
                    _year = value;
                    OnPropertyChanged();
                    if (!_isInitializing)
                    {
                        LoadData();
                    }
                }
            }
        }

        public string ClientCode
        {
            get => _clientCode;
            set
            {
                if (_clientCode != value)
                {
                    _clientCode = value;
                    OnPropertyChanged();
                    if (!_isInitializing)
                    {
                        LoadJobs(); // Reload jobs for the new client
                        LoadData();
                    }
                }
            }
        }

        private void LoadClients()
        {
            if (Globals.ConMan == null)
            {
                return;
            }

            Globals.ConMan.GetAllClients();

            Clients.Clear();
            if (Globals.ConMan.LstClients != null)
            {
                foreach (var client in Globals.ConMan.LstClients)
                {
                    Clients.Add(client);
                }
            }
        }

        private void LoadEmployees()
        {
            if (Globals.ConMan == null)
            {
                return;
            }

            Globals.ConMan.GetAllEmployees();

            Employees.Clear();
            if (Globals.ConMan.LstEmployee != null)
            {
                foreach (var employee in Globals.ConMan.LstEmployee)
                {
                    Employees.Add(employee);
                }
            }
        }

        private void LoadJobs()
        {
            if (Globals.ConMan == null)
            {
                return;
            }

            Globals.ConMan.GetAllJobs();

            Jobs.Clear();
            if (Globals.ConMan.LstJobs != null)
            {
                // Filter jobs by current client code
                var filteredJobs = Globals.ConMan.LstJobs
                    .Where(job => job.clt_code == _clientCode)
                    .ToList();

                foreach (var job in filteredJobs)
                {
                    Jobs.Add(job);
                }
            }
        }

        private void LoadData()
        {
            if (Globals.ConMan == null)
            {
                return;
            }

            var data = Globals.ConMan.GetDataClientMonth<DtoWorkedHours>(
                "GetDataClientMonth",
                Month,
                Year,
                ClientCode,
                out string message);

            WorkedHours.Clear();
            foreach (var item in data)
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
