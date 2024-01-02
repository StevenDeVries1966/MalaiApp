using System.Windows;
using System.Windows.Controls;
using WpfUI.Data;
using WpfUI.ViewModel;

namespace WpfUI.Views
{
    /// <summary>
    /// Interaction logic for JobsView.xaml
    /// </summary>
    public partial class JobsView : UserControl
    {
        private JobsViewModel _viewModel;

        public JobsView()
        {
            InitializeComponent();
            _viewModel = new JobsViewModel(new MalaiDataProvider());
            DataContext = _viewModel;
            Loaded += JobsView_Loaded;
        }

        private async void JobsView_Loaded(object sender, RoutedEventArgs e)
        {
            await _viewModel.LoadAsync();
        }
    }
}
