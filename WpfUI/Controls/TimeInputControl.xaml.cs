using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace WpfUI.Controls
{
    /// <summary>
    /// Interaction logic for TimeInputControl.xaml
    /// </summary>
    public partial class TimeInputControl : UserControl
    {
        public static readonly DependencyProperty SelectedTimeProperty =
            DependencyProperty.Register("SelectedTime", typeof(string), typeof(TimeInputControl),
                new FrameworkPropertyMetadata("00:00", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string SelectedTime
        {
            get { return (string)GetValue(SelectedTimeProperty); }
            set { SetValue(SelectedTimeProperty, value); }
        }

        public TimeInputControl()
        {
            InitializeComponent();
        }

        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            // Validate the input to allow only numeric characters and the ':' symbol
            if (!(char.IsDigit(e.Text, 0) || e.Text == ":"))
            {
                e.Handled = true;
            }
        }

        private void txtTime_LostFocus(object sender, RoutedEventArgs e)
        {
            // Ensure that the entered time is in the valid format (HH:mm)
            try
            {
                TimeSpan time = TimeSpan.ParseExact(SelectedTime, "hh\\:mm", CultureInfo.InvariantCulture);
                SelectedTime = time.ToString("hh\\:mm");
            }
            catch
            {
                // Handle invalid input or set default value
                SelectedTime = "00:00";
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Attach event handlers
            txtTime.LostFocus += txtTime_LostFocus;
        }
    }
}
