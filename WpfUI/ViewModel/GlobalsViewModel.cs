using DataLayer.Classes;
using System;
using System.Collections.ObjectModel;

namespace WpfUI.ViewModel
{
    public class GlobalsViewModel
    {
        public static ObservableCollection<DtoClient> Clients { get; set; } = new();
        public static ObservableCollection<int> TestObservCol { get; set; } = new() { 1, 2, 3 };
        public static ObservableCollection<DtoJob> JobsAll { get; set; } = new();
        public static ObservableCollection<DtoJob> JobsClients { get; set; } = new();
        public static int SelectedJobId { get; set; }

        public static string? SelectedMonth
        {
            get
            {
                if (string.IsNullOrEmpty(SelectedMonth))
                {
                    return "December";
                }
                else
                {
                    return SelectedMonth;
                }
            }
            set
            {
                SelectedMonth = value;

            }
        }
        public static int SelectedYear
        {
            get
            {
                if (SelectedYear == 0)
                {
                    return DateTime.Now.Year;
                }
                return SelectedYear;
            }
            set
            {
                SelectedYear = value;

            }
        }

        public static string TestTime
        {
            get => "02:33";
            set { TestTime = value; }

        }
    }
}
