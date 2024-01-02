using DataLayer.Classes;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WpfUI.Data;

namespace WpfUI.ViewModel
{
    public class EmployeeViewModel
    {
        private readonly IMalaiDataProvider _malaiDataProvider;

        public EmployeeViewModel(IMalaiDataProvider malaiDataProvider)
        {
            _malaiDataProvider = malaiDataProvider;
        }
        public ObservableCollection<DtoEmployee> Employees { get; } = new();
        public DtoEmployee? SelectedEmployee { get; set; }
        public async Task LoadEmployeesAsync()
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
                    Employees.Add(emp);
                }
            }
        }
    }
}
