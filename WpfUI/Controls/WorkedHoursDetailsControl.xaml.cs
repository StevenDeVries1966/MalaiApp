using System.Collections.Specialized;
using System.Windows.Controls;
using WpfUI.ViewModel;

namespace WpfUI.Controls
{
    /// <summary>
    /// Interaction logic for WorkedHoursDetailsControl.xaml
    /// </summary>
    public partial class WorkedHoursDetailsControl : UserControl
    {
        public WorkedHoursDetailsControl()
        {
            InitializeComponent();
            GlobalsViewModel.Clients.CollectionChanged += Data_CollectionChanged;
        }
        private void Data_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ComboBoxJobs.SelectedIndex = 0;
        }

    }
}
