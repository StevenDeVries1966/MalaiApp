using System;
using System.Linq;
using System.Windows;
using WpfUI.Data;
using WpfUI.Helpers;
using WpfUI.ViewModel;

namespace WpfUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private MalaiViewModel _viewModel;
        private EmployeeViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new EmployeeViewModel(new MalaiDataProvider());
            DataContext = _viewModel;
            Globals.Employees = new MalaiDataProvider().GetEmployees();
            Globals.Employee_Current =
                Globals.Employees.Where(o => o.login.Equals(Environment.UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            Title = Globals.MainFormTitle;
        }
    }
}
