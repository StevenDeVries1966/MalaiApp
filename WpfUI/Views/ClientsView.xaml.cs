﻿using System.Windows;
using System.Windows.Controls;
using WpfUI.Data;
using WpfUI.ViewModel;

namespace WpfUI.Views
{
    /// <summary>
    /// Interaction logic for ClientsView.xaml
    /// </summary>
    public partial class ClientsView : UserControl
    {
        private MalaiViewModel _viewModel;

        public ClientsView()
        {
            InitializeComponent();
            _viewModel = new MalaiViewModel(new MalaiDataProvider());
            DataContext = _viewModel;
            Loaded += ClientsView_Loaded;
        }

        private async void ClientsView_Loaded(object sender, RoutedEventArgs e)
        {
            await _viewModel.LoadDataContextAsync();
        }
    }
}