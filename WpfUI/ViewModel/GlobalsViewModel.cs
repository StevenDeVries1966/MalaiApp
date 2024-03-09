using DataLayer.Classes;
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

        public static string TestTime
        {
            get => "02:33";
            set { TestTime = value; }

        }
    }
}
