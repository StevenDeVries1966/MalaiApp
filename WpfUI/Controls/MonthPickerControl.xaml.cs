using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfUI.Controls
{
    /// <summary>
    /// Interaction logic for MonthPickerControl.xaml
    /// </summary>
    public partial class MonthPickerControl : UserControl
    {
        public static readonly DependencyProperty SelectedMonthProperty =
            DependencyProperty.Register("SelectedMonth", typeof(int), typeof(MonthPickerControl));

        public static readonly DependencyProperty SelectedYearProperty =
            DependencyProperty.Register("SelectedYear", typeof(int), typeof(MonthPickerControl));

        public int SelectedMonth
        {
            get { return (int)GetValue(SelectedMonthProperty); }
            set { SetValue(SelectedMonthProperty, value); }
        }

        public int SelectedYear
        {
            get { return (int)GetValue(SelectedYearProperty); }
            set { SetValue(SelectedYearProperty, value); }
        }

        public MonthPickerControl()
        {
            InitializeComponent();
            Loaded += MonthPickerControl_Loaded;
            monthComboBox.SelectedIndex = DateTime.Now.Month - 1;
        }

        private void MonthPickerControl_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = DateTime.Now.Year; i >= DateTime.Now.Year - 2; i--)
            {
                yearComboBox.Items.Add(i);
            }
            yearComboBox.SelectedIndex = 0;
            yearComboBox.SelectionChanged += ComboBoxYear_SelectionChanged;
        }

        private void ComboBoxYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            monthComboBox.SelectedIndex = 0;
            SelectedMonth = monthComboBox.SelectedIndex + 1; // Months are 1-based
            SelectedYear = (int)yearComboBox.SelectedItem;
        }
    }
}
