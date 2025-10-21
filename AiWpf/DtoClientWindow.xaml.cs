using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DataLayer.Classes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AiWpf
{
    public partial class DtoClientWindow : Window, INotifyPropertyChanged
    {
        private readonly ILogger<DtoClientWindow>? _logger;
        private ObservableCollection<DtoClient> _clients = new();
        private DtoClient? _originalClient;
        private DtoClient? _selectedClient;
        private bool _isEditing;
        private bool _isLoading;
        private string _statusMessage = "Ready";

        public ObservableCollection<DtoClient> Clients
        {
            get => _clients;
            set => SetProperty(ref _clients, value);
        }

        public DtoClient? SelectedClient
        {
            get => _selectedClient;
            set
            {
                if (SetProperty(ref _selectedClient, value))
                {
                    OnPropertyChanged(nameof(CanEdit));
                }
            }
        }

        public bool IsEditing
        {
            get => _isEditing;
            set
            {
                if (SetProperty(ref _isEditing, value))
                {
                    OnPropertyChanged(nameof(CanEdit));
                    OnPropertyChanged(nameof(CanAdd));
                    OnPropertyChanged(nameof(CanRefresh));
                    OnPropertyChanged(nameof(CanSave));
                    OnPropertyChanged(nameof(CanCancel));
                    OnPropertyChanged(nameof(IsReadOnly));
                }
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (SetProperty(ref _isLoading, value))
                {
                    OnPropertyChanged(nameof(CanEdit));
                    OnPropertyChanged(nameof(CanAdd));
                    OnPropertyChanged(nameof(CanRefresh));
                }
            }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        // Command properties
        public bool CanEdit => SelectedClient != null && !IsEditing && !IsLoading;
        public bool CanSave => IsEditing && !IsLoading;
        public bool CanCancel => IsEditing && !IsLoading;
        public bool CanAdd => !IsEditing && !IsLoading;
        public bool CanRefresh => !IsEditing && !IsLoading;
        public bool IsReadOnly => !IsEditing;

        public ICommand EditCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand RefreshCommand { get; }

        public DtoClientWindow()
        {
            InitializeComponent();
            DataContext = this;

            // Initialize commands
            EditCommand = new RelayCommand(_ => EditClient(), _ => CanEdit);
            SaveCommand = new RelayCommand(async _ => await SaveClientAsync(), _ => CanSave);
            CancelCommand = new RelayCommand(_ => CancelEdit(), _ => CanCancel);
            AddCommand = new RelayCommand(_ => AddClient(), _ => CanAdd);
            RefreshCommand = new RelayCommand(async _ => await LoadClientDataAsync(), _ => CanRefresh);

            // Set initial data source
            DtoClientGrid.ItemsSource = Clients;
            
            Loaded += async (s, e) => await LoadClientDataAsync();
        }

        private async Task LoadClientDataAsync()
        {
            try
            {
                IsLoading = true;
                StatusMessage = "Loading client data...";
                
                var connectionString = await GetConnectionStringAsync();
                if (string.IsNullOrEmpty(connectionString))
                {
                    StatusMessage = "Error: Connection string not found in configuration";
                    ShowError("Connection string not found in appsettings.json", "Configuration Error");
                    return;
                }

                using var context = new MalaiContext(connectionString);
                var message = context.GetAllClients();
                
                if (message == "OK" && context.lstClients != null)
                {
                    Clients.Clear();
                    foreach (var client in context.lstClients)
                    {
                        Clients.Add(client);
                    }
                    
                    Title = $"DtoClient Viewer - {Clients.Count} records loaded";
                    StatusMessage = $"Successfully loaded {Clients.Count} client records";
                    _logger?.LogInformation("Successfully loaded {Count} client records", Clients.Count);
                }
                else
                {
                    StatusMessage = $"Error loading data: {message}";
                    ShowError($"Error loading client data: {message}", "Database Error");
                    _logger?.LogError("Error loading client data: {Message}", message);
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: {ex.Message}";
                ShowError($"Error loading client data: {ex.Message}", "Application Error");
                _logger?.LogError(ex, "Error loading client data");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void EditClient()
        {
            if (SelectedClient == null) return;

            // Deep copy for rollback functionality
            _originalClient = CreateClientCopy(SelectedClient);
            IsEditing = true;
            StatusMessage = "Edit mode enabled. Make your changes and click Save or Cancel.";
            
            // Make the client code column read-only during edit
            var codeColumn = DtoClientGrid.Columns.FirstOrDefault(c => 
                c.Header?.ToString()?.Contains("Code", StringComparison.OrdinalIgnoreCase) == true);
            if (codeColumn != null)
            {
                codeColumn.IsReadOnly = true;
            }
        }

        private async Task SaveClientAsync()
        {
            if (SelectedClient == null) return;

            try
            {
                StatusMessage = "Saving changes...";
                    
                var connectionString = await GetConnectionStringAsync();
                using var context = new MalaiContext(connectionString);
                        
                var success = context.UpdateClient(SelectedClient, out var message);
                
                if (success)
                {
                    StatusMessage = "Changes saved successfully.";
                    ShowInfo("Client updated successfully!", "Success");
                    ExitEditMode();
                    _logger?.LogInformation("Client {ClientCode} updated successfully", SelectedClient.clt_code);
                }
                else
                {
                    StatusMessage = $"Error saving changes: {message}";
                    ShowError($"Error updating client: {message}", "Database Error");
                    _logger?.LogError("Error updating client {ClientCode}: {Message}", SelectedClient.clt_code, message);
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error saving changes: {ex.Message}";
                ShowError($"Error saving changes: {ex.Message}", "Application Error");
                _logger?.LogError(ex, "Error saving client changes");
            }
        }

        private void CancelEdit()
        {
            if (SelectedClient != null && _originalClient != null)
            {
                // Restore original values using reflection or manual copy
                CopyClientProperties(_originalClient, SelectedClient);
                DtoClientGrid.Items.Refresh();
                StatusMessage = "Changes cancelled.";
            }
            
            ExitEditMode();
        }

        private void AddClient()
        {
            var addWindow = new AddClientWindow
            {
                Owner = this
            };
            
            if (addWindow.ShowDialog() == true)
            {
                _ = LoadClientDataAsync(); // Fire and forget for UI responsiveness
            }
        }

        private void ExitEditMode()
        {
            IsEditing = false;
            _originalClient = null;
            StatusMessage = $"Successfully loaded {Clients.Count} client records";
        }

        private void DtoClientGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedClient = DtoClientGrid.SelectedItem as DtoClient;
        }

        private static DtoClient CreateClientCopy(DtoClient original)
        {
            return new DtoClient
            {
                clt_code = original.clt_code,
                clt_name = original.clt_name,
                address = original.address,
                postalcode = original.postalcode,
                city = original.city,
                country = original.country,
                email = original.email,
                phone = original.phone,
                rate_ES001 = original.rate_ES001,
                rate_AS001 = original.rate_AS001,
                retainer_ES001 = original.retainer_ES001,
                retainer_AS001 = original.retainer_AS001,
                report_type = original.report_type
            };
        }

        private static void CopyClientProperties(DtoClient source, DtoClient target)
        {
            target.clt_code = source.clt_code;
            target.clt_name = source.clt_name;
            target.address = source.address;
            target.postalcode = source.postalcode;
            target.city = source.city;
            target.country = source.country;
            target.email = source.email;
            target.phone = source.phone;
            target.rate_ES001 = source.rate_ES001;
            target.rate_AS001 = source.rate_AS001;
            target.retainer_ES001 = source.retainer_ES001;
            target.retainer_AS001 = source.retainer_AS001;
            target.report_type = source.report_type;
        }

        private static async Task<string> GetConnectionStringAsync()
        {
            return await Task.Run(() =>
            {
                try
                {
                    var config = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .Build();
                    return config["connectionstring"] ?? string.Empty;
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Failed to read configuration: {ex.Message}", ex);
                }
            });
        }

        private static void ShowError(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private static void ShowInfo(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T backingField, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingField, value))
                return false;

            backingField = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        #endregion
    }

    // Simple RelayCommand implementation
    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Predicate<object?>? _canExecute;

        public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object? parameter) => _canExecute?.Invoke(parameter) ?? true;

        public void Execute(object? parameter) => _execute(parameter);
    }
}