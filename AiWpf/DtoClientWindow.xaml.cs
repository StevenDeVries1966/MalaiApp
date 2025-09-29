using System.Windows;
using DataLayer.Classes;
using Microsoft.Extensions.Configuration;

namespace AiWpf
{
    public partial class DtoClientWindow : Window
    {
        public DtoClientWindow()
        {
            InitializeComponent();
            LoadClientData();
        }

        private async void LoadClientData()
        {
            try
            {
                StatusBarText.Content = "Loading client data...";
                
                // Get connection string from configuration
                string connectionString = GetConnectionString();
                
                if (string.IsNullOrEmpty(connectionString))
                {
                    StatusBarText.Content = "Error: Connection string not found in configuration";
                    MessageBox.Show("Connection string not found in appsettings.json", "Configuration Error", 
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Create MalaiContext and load client data
                using (var context = new MalaiContext(connectionString))
                {
                    string message = context.GetAllClients();
                    
                    if (message == "OK" && context.lstClients != null)
                    {
                        // Set the DataGrid's ItemsSource to the client list
                        DtoClientGrid.ItemsSource = context.lstClients;
                        
                        // Update window title and status bar with record count
                        int count = context.lstClients.Count;
                        Title = $"DtoClient Viewer - {count} records loaded";
                        StatusBarText.Content = $"Successfully loaded {count} client records";
                    }
                    else
                    {
                        StatusBarText.Content = $"Error loading data: {message}";
                        MessageBox.Show($"Error loading client data: {message}", "Database Error", 
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                StatusBarText.Content = $"Error: {ex.Message}";
                MessageBox.Show($"Error loading client data: {ex.Message}", "Application Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
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