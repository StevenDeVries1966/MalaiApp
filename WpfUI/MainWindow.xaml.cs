using System;
using System.Linq;
using System.Windows;
using WpfUI.Data;
using WpfUI.Helpers;
using WpfUI.ViewModel;

namespace WpfUI
{

    public partial class MainWindow : Window
    {
        private readonly MainViewModel _viewModel;
        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainViewModel(new WorkedHoursViewModel(new MalaiDataProvider()), new JobsViewModel());
            DataContext = _viewModel;
            Loaded += MainWindow_Loaded;

            Globals.Employees = new MalaiDataProvider().GetEmployees();
            Globals.Employee_Current =
                Globals.Employees.Where(o => o.login.Equals(Environment.UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            Title = Globals.MainFormTitle;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await _viewModel.LoadAsync();
        }
    }
}
