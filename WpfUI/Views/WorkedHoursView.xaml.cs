﻿using System.Windows;
using System.Windows.Controls;
using WpfUI.Data;
using WpfUI.ViewModel;

namespace WpfUI.Views
{
    /// <summary>
    /// Interaction logic for WorkedHoursView.xaml
    /// </summary>
    public partial class WorkedHoursView : UserControl
    {
        private WorkedHoursViewModel _viewModel;

        public WorkedHoursView()
        {
            InitializeComponent();
            _viewModel = new WorkedHoursViewModel(new MalaiDataProvider());
            //_viewModel.SelectedMonth = myComboBoxMonthYear.monthComboBox.SelectionBoxItem.ToString();
            //_viewModel.SelectedYear = (int)myComboBoxMonthYear.yearComboBox.SelectionBoxItem;
            DataContext = _viewModel;
            Loaded += WorkedHoursView_Loaded;
        }

        private async void WorkedHoursView_Loaded(object sender, RoutedEventArgs e)
        {
            await _viewModel.LoadAsync();
        }

    }
}
