using DataLayer.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WpfUI.Command;
using WpfUI.Data;
using WpfUI.Helpers;

namespace WpfUI.ViewModel
{
    public class WorkedHoursViewModel : ViewModelBase
    {
        private readonly IMalaiDataProvider _malaiDataProvider;
        private WorkedHoursItemViewModel? _selectedWorkedHours;
        public ObservableCollection<WorkedHoursItemViewModel> WorkedHours { get; } = new();
        public DelegateCommand AddCommand { get; }
        public DelegateCommand DeleteCommand { get; }
        public bool IsWorkedHoursSelected => SelectedWorkedHours is not null;

        public WorkedHoursViewModel(IMalaiDataProvider malaiDataProvider)
        {
            _malaiDataProvider = malaiDataProvider;
            AddCommand = new DelegateCommand(Add);
            DeleteCommand = new DelegateCommand(Delete, CanDelete);
        }
        public WorkedHoursItemViewModel? SelectedWorkedHours
        {
            get => _selectedWorkedHours;
            set
            {
                _selectedWorkedHours = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(IsWorkedHoursSelected));
                DeleteCommand.RaiseCanExecuteChanged();
            }
        }

        public async override Task LoadAsync()
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
                    WorkedHours.Add(new WorkedHoursItemViewModel(emp));
                }
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
                emp_id = Globals.Employee_Current.emp_id,
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
