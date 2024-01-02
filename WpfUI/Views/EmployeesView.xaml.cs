using System.Windows;
using System.Windows.Controls;
using WpfUI.Data;
using WpfUI.ViewModel;

namespace WpfUI.Views
{
    /// <summary>
    /// Interaction logic for EmployeesView.xaml
    /// </summary>
    public partial class EmployeesView : UserControl
    {
        private EmployeeViewModel _viewModel;

        public EmployeesView()
        {
            InitializeComponent();
            _viewModel = new EmployeeViewModel(new MalaiDataProvider());
            DataContext = _viewModel;
            Loaded += EmployeesView_Loaded;
        }

        private async void EmployeesView_Loaded(object sender, RoutedEventArgs e)
        {
            await _viewModel.LoadAsync();
        }
    }
}
