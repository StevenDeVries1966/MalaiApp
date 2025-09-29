using System.Windows.Controls;

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
            //GlobalsViewModel.JobsClients.CollectionChanged += Data_CollectionChanged;
            //txtStartTime.LostFocus += txtTimeLostFocus;
            //txtEndTime.LostFocus += txtTimeLostFocus;
        }

        //private void txtTimeLostFocus(object sender, RoutedEventArgs e)
        //{
        //    TextBox txtBox = (TextBox)sender;
        //    if (!Assist.IsValidTime(txtBox.Text))
        //    {
        //        txtBox.Foreground = Brushes.Red;
        //    }
        //    else
        //    {
        //        txtBox.Foreground = Brushes.Black;
        //        CalcWorkedHours();
        //    }
        //}

        //private void CalcWorkedHours()
        //{
        //    if (Assist.IsValidTime(txtStartTime.Text) && Assist.IsValidTime(txtEndTime.Text))
        //    {
        //        DateTime start = Assist.SetDateTime(DateTime.Now, txtStartTime.Text);
        //        DateTime end = Assist.SetDateTime(DateTime.Now, txtEndTime.Text);
        //        txtWorkedHours.Text = Assist.CalcHoursWorked(start, end);
        //    }
        //}

        //private void Data_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        //{
        //    ComboBoxJobs.SelectedIndex = GlobalsViewModel.SelectedJobId;
        //}

    }
}
