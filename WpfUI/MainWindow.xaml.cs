using System.Windows;
using System.Windows.Controls;

namespace WpfUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonAddWorkedHours_Click(object sender, RoutedEventArgs e)
        {
            BtnAddWorkedHours.Content = "Add worked hours!";

            //var column = (int)BtnAddWorkedHours.GetValue(Grid.ColumnProperty);
            //var newColumn = column == 0 ? 2 : 0;
            //BtnAddWorkedHours.SetValue(Grid.ColumnProperty, newColumn);

            var column = Grid.GetColumn(BtnAddWorkedHours);
            var newColumn = column == 0 ? 2 : 0;
            Grid.SetColumn(BtnAddWorkedHours, newColumn);
        }
    }
}
