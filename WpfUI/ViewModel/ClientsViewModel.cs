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
        private ClientsItemViewModel? _selectedClient;

        public ClientsViewModel(IMalaiDataProvider malaiDataProvider)
        {
            _malaiDataProvider = malaiDataProvider;
        }
        public ObservableCollection<ClientsItemViewModel> Clients { get; } = new();
        public ClientsItemViewModel? SelectedClient
        {
            get => _selectedClient;
            set
            {
                _selectedClient = value;
                RaisePropertyChanged();
            }
        }
        public async override Task LoadAsync()
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
                    Clients.Add(new ClientsItemViewModel(clt));
                }
            }
        }
    }

}
