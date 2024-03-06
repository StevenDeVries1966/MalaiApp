using DataLayer.Classes;
using System.Collections.ObjectModel;

namespace WpfUI.ViewModel
{
    public class GlobalsViewModel
    {
        public static ObservableCollection<DtoJob> JobsAll { get; set; } = new();
        public static ObservableCollection<DtoJob> JobsClients { get; set; } = new();
        public static int SelectedJobId { get; set; }
    }
}
