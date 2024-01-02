using DataLayer.Classes;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WpfUI.Data;

namespace WpfUI.ViewModel
{
    public class ClientsViewModel : ViewModelBase
    {
        private readonly IMalaiDataProvider _malaiDataProvider;

        public ClientsViewModel(IMalaiDataProvider malaiDataProvider)
        {
            _malaiDataProvider = malaiDataProvider;
        }
        public ObservableCollection<DtoClient> Clients { get; } = new();
        public DtoEmployee? SelectedEmployee { get; set; }
        public async Task LoadClientsAsync()
        {
            if (Clients.Any())
            {
                return;
            }
            List<DtoClient>? lst = await _malaiDataProvider.GetClientsAsync();
            if (lst is not null)
            {
                foreach (DtoClient clt in lst)
                {
                    Clients.Add(clt);
                }
            }
        }

    }

}
