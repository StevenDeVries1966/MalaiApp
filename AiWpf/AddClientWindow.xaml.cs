using System.Windows;
using DataLayer.Classes;
using Microsoft.Extensions.Configuration;

namespace AiWpf
{
    public partial class AddClientWindow : Window
    {
        public AddClientWindow()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate required fields
            if (string.IsNullOrWhiteSpace(ClientCodeTextBox.Text))
            {
                MessageBox.Show("Client Code is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                ClientCodeTextBox.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(ClientNameTextBox.Text))
            {
                MessageBox.Show("Client Name is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                ClientNameTextBox.Focus();
                return;
            }

            try
            {
                // Parse numeric values
                double rateES001 = 0, rateAS001 = 0, retainerES001 = 0, retainerAS001 = 0;
                
                if (!string.IsNullOrWhiteSpace(RateES001TextBox.Text) && !double.TryParse(RateES001TextBox.Text, out rateES001))
                {
                    MessageBox.Show("Invalid Rate ES001 value.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                
                if (!string.IsNullOrWhiteSpace(RateAS001TextBox.Text) && !double.TryParse(RateAS001TextBox.Text, out rateAS001))
                {
                    MessageBox.Show("Invalid Rate AS001 value.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                
                if (!string.IsNullOrWhiteSpace(RetainerES001TextBox.Text) && !double.TryParse(RetainerES001TextBox.Text, out retainerES001))
                {
                    MessageBox.Show("Invalid Retainer ES001 value.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                
                if (!string.IsNullOrWhiteSpace(RetainerAS001TextBox.Text) && !double.TryParse(RetainerAS001TextBox.Text, out retainerAS001))
                {
                    MessageBox.Show("Invalid Retainer AS001 value.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Create new client
                var newClient = new DtoClient
                {
                    clt_code = ClientCodeTextBox.Text.Trim(),
                    clt_name = ClientNameTextBox.Text.Trim(),
                    address = AddressTextBox.Text.Trim(),
                    postalcode = PostalCodeTextBox.Text.Trim(),
                    city = CityTextBox.Text.Trim(),
                    country = CountryTextBox.Text.Trim(),
                    email = EmailTextBox.Text.Trim(),
                    phone = PhoneTextBox.Text.Trim(),
                    rate_ES001 = rateES001,
                    rate_AS001 = rateAS001,
                    retainer_ES001 = retainerES001,
                    retainer_AS001 = retainerAS001,
                    report_type = ReportTypeTextBox.Text.Trim()
                };

                // Save to database
                string connectionString = GetConnectionString();
                using (var context = new MalaiContext(connectionString))
                {
                    string message;
                    bool success = context.AddClient(newClient, out message);
                    
                    if (success)
                    {
                        MessageBox.Show("Client added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        DialogResult = true;
                        Close();
                    }
                    else
                    {
                        MessageBox.Show($"Error adding client: {message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding client: {ex.Message}", "Application Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private static string GetConnectionString()
        {
            try
            {
                IConfiguration config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", false, true)
                    .Build();
                return config["connectionstring"] ?? "";
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to read configuration: {ex.Message}", ex);
            }
        }
    }
}