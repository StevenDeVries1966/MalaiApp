using System.Windows;

namespace Malai.UI.Views
{
    /// <summary>
    /// Interaction logic for DtoWorkedHoursWindow.xaml
    /// </summary>
    public partial class DtoWorkedHoursWindow : Window
    {
        public DtoWorkedHoursWindow()
        {
            InitializeComponent();
        }
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            // _dtoWorkedHours has been updated with the data from the text boxes

            // TODO: Save _dtoWorkedHours to database or use it in some other way
        }
    }
}
