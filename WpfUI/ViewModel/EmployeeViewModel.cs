using DataLayer.Classes;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WpfUI.Data;

namespace WpfUI.ViewModel
{
    public class EmployeeViewModel : ViewModelBase
    {
        private readonly IMalaiDataProvider _malaiDataProvider;
        private EmployeeItemViewModel? _selectedEmployee;

        public bool IsEmployeeSelected => SelectedEmployee is not null;

        public EmployeeViewModel(IMalaiDataProvider malaiDataProvider)
        {
            _malaiDataProvider = malaiDataProvider;
        }
        public ObservableCollection<EmployeeItemViewModel> Employees { get; } = new();
        public EmployeeItemViewModel? SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                _selectedEmployee = value;
                RaisePropertyChanged();
            }
        }

        public async override Task LoadAsync()
        {
            if (Employees.Any())
            {
                return;
            }
            List<DtoEmployee>? lst = await _malaiDataProvider.GetEmployeesAsync();
            if (lst is not null)
            {
                foreach (DtoEmployee emp in lst)
                {
                    Employees.Add(new EmployeeItemViewModel(emp));
                }
            }
        }
    }
}
