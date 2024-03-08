using DataLayer.Classes;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfUI.Command;
using WpfUI.Data;
using WpfUI.Helpers;

namespace WpfUI.ViewModel
{
    public class WorkedHoursViewModel : ViewModelBase
    {
        private readonly IMalaiDataProvider _malaiDataProvider;
        private WorkedHoursItemViewModel? _selectedWorkedHours;
        private EmployeeItemViewModel? _selectedEmployee;
        private int? _selectedEmployeeId;
        private int? _selectedClientId;
        //private int? _selectedJobId;

        public bool IsWorkedHoursSelected => SelectedWorkedHours is not null;

        public WorkedHoursViewModel(IMalaiDataProvider malaiDataProvider)
        {
            _malaiDataProvider = malaiDataProvider;
            AddCommand = new DelegateCommand(Add);
            DeleteCommand = new DelegateCommand(Delete, CanDelete);
        }

        public MalaiContext? Ctx { get; set; }
        public ObservableCollection<WorkedHoursItemViewModel> WorkedHours { get; set; } = new();
        public ObservableCollection<DtoEmployee> Employees { get; set; } = new();
        public ObservableCollection<DtoClient> Clients
        {
            get
            {
                return GlobalsViewModel.Clients;
            }
            set
            {
                GlobalsViewModel.Clients = value;
            }
        }

        public ObservableCollection<DtoJob> JobsAll
        {
            get => GlobalsViewModel.JobsAll;
            set
            {
                GlobalsViewModel.JobsAll = value;
            }
        }
        public ObservableCollection<DtoJob> JobsClients
        {
            get => GlobalsViewModel.JobsClients;
            set
            {
                GlobalsViewModel.JobsClients = value;
            }
        }
        public ObservableCollection<int> TestObservCol
        {
            get => GlobalsViewModel.TestObservCol;
            set
            {
                GlobalsViewModel.TestObservCol = value;
            }
        }

        public DelegateCommand AddCommand { get; }
        public DelegateCommand DeleteCommand { get; }
        public WorkedHoursItemViewModel? SelectedWorkedHours
        {
            get => _selectedWorkedHours;
            set
            {
                _selectedWorkedHours = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(IsWorkedHoursSelected));
                RaisePropertyChanged(nameof(SelectedEmployeeId));
                RaisePropertyChanged(nameof(SelectedJobId));
                DeleteCommand.RaiseCanExecuteChanged();
            }
        }
        public EmployeeItemViewModel? SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                _selectedEmployee = value;
                RaisePropertyChanged();
            }
        }

        public int SelectedEmployeeId
        {
            get
            {
                if (_selectedWorkedHours != null && _selectedWorkedHours.Employee != null)
                {
                    if (Employees != null)
                    {
                        DtoEmployee employee = Employees.FirstOrDefault(o => o.emp_id == _selectedWorkedHours.Employee.emp_id);
                        return Employees.IndexOf(employee);
                    }
                }
                return -1;
            }
            set
            {
                _selectedEmployeeId = value;
            }
        }

        public int SelectedClientId
        {
            get
            {
                if (_selectedWorkedHours != null && _selectedWorkedHours.Client != null)
                {
                    if (Clients != null)
                    {
                        DtoClient client = Clients.FirstOrDefault(o => o.clt_code == _selectedWorkedHours.Client.clt_code);
                        SelectedWorkedHours.Client = client;
                        SelectedClientId = Clients.IndexOf(client);


                        RaisePropertyChanged(nameof(GlobalsViewModel.JobsClients));
                        RaisePropertyChanged(nameof(SelectedJobId));
                        RaisePropertyChanged(nameof(SelectedJobId));
                        return Clients.IndexOf(client);
                    }
                }
                return -1;
            }
            set
            {
                _selectedClientId = value;
            }
        }


        public int SelectedJobId
        {
            get
            {
                if (_selectedWorkedHours != null && _selectedWorkedHours.Job != null)
                {
                    DtoJob job = GlobalsViewModel.JobsClients.FirstOrDefault(o => o.clt_code == _selectedWorkedHours.Client.clt_code);
                    GlobalsViewModel.SelectedJobId = GlobalsViewModel.JobsClients.IndexOf(job);
                    return GlobalsViewModel.SelectedJobId;
                }
                else
                {
                    GlobalsViewModel.SelectedJobId = -1;
                    return GlobalsViewModel.SelectedJobId;
                }

                return GlobalsViewModel.SelectedJobId;
            }
            set
            {
                GlobalsViewModel.SelectedJobId = value;
            }
        }
        private string statusText;
        public string StatusText
        {
            get => statusText;
            set
            {
                statusText = value;
                RaisePropertyChanged();
            }
        }
        public async override Task LoadAsync()
        {
            try
            {

                Mouse.OverrideCursor = Cursors.Wait;
                StatusText = "Loading.....";

                if (WorkedHours.Any())
                {
                    return;
                }

                Ctx = await _malaiDataProvider.GetDataContextAsync();
                if (Ctx != null)
                {
                    WorkedHours.Clear();
                    foreach (DtoWorkedHours wh in Ctx.lstWorkedHours)
                    {
                        WorkedHours.Add(new WorkedHoursItemViewModel(wh));
                    }
                    Employees.Clear();
                    foreach (DtoEmployee emp in Ctx.lstEmployee)
                    {
                        Employees.Add(emp);
                    }
                    Clients.Clear();
                    foreach (DtoClient clt in Ctx.lstClients)
                    {
                        Clients.Add(clt);
                    }

                    GlobalsViewModel.JobsAll.Clear();
                    foreach (DtoJob job in Ctx.lstJobs)
                    {
                        GlobalsViewModel.JobsAll.Add(job);
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                Mouse.OverrideCursor = null;
                StatusText = "";
            }


        }
        private void Add(object? parameter)
        {
            DateTime currentDate = DateTime.Today;
            // Set the desired time (8:00 AM)
            TimeSpan startTime = new TimeSpan(8, 30, 0);
            // Combine the current date with the desired time
            DateTime startDateTime = currentDate.Add(startTime);
            var wh = new DtoWorkedHours
            {
                emp_id = Globals.Employee_Current!.emp_id,
                emp_code = Globals.Employee_Current.emp_code,
                start_time = startDateTime,
                end_time = DateTime.Now,
                week = Assist.GetIso8601WeekNumber(DateTime.Now),
                month = DateTime.Now.Month,
                year = DateTime.Now.Year

            };
            var viewModel = new WorkedHoursItemViewModel(wh);
            WorkedHours.Add(viewModel);
            SelectedWorkedHours = viewModel;
        }

        private void Delete(object? parameter)
        {
            if (SelectedWorkedHours is not null)
            {
                WorkedHours.Remove(SelectedWorkedHours);
                SelectedWorkedHours = null;
            }
        }

        private bool CanDelete(object? parameter) => SelectedWorkedHours is not null;
    }

}
