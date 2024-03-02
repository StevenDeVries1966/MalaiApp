﻿using DataLayer.Classes;
using System;
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
        private EmployeeItemViewModel? _selectedEmployee;
        private int? _selectedEmployeeId;

        public bool IsWorkedHoursSelected => SelectedWorkedHours is not null;

        public WorkedHoursViewModel(IMalaiDataProvider malaiDataProvider)
        {
            _malaiDataProvider = malaiDataProvider;
            AddCommand = new DelegateCommand(Add);
            DeleteCommand = new DelegateCommand(Delete, CanDelete);
        }

        public MalaiContext? Ctx { get; set; }
        public ObservableCollection<WorkedHoursItemViewModel> WorkedHours { get; } = new();
        public ObservableCollection<EmployeeItemViewModel> Employees { get; } = new();
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
                RaisePropertyChanged(nameof(SelectedEmployee));
                RaisePropertyChanged(nameof(SelectedEmployeeId));
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
        public int? SelectedEmployeeId
        {
            get
            {

                if (_selectedWorkedHours != null)
                {
                    return _selectedWorkedHours.emp_id; ;
                }
                return -1;
            }
            set
            {
                _selectedEmployeeId = ((int)value)!;
                RaisePropertyChanged();
            }
        }

        public async override Task LoadAsync()
        {
            if (WorkedHours.Any())
            {
                return;
            }

            Ctx = await _malaiDataProvider.GetDataContextAsync();
            if (Ctx != null)
            {
                foreach (DtoWorkedHours wh in Ctx.lstWorkedHours)
                {
                    WorkedHours.Add(new WorkedHoursItemViewModel(wh));
                }
                foreach (DtoEmployee emp in Ctx.lstEmployee)
                {
                    Employees.Add(new EmployeeItemViewModel(emp));
                }
                //SelectedEmployee = Employees.FirstOrDefault(o => o.first_name.Equals("Steven"));
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
