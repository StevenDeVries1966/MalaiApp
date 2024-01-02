using DataLayer.Classes;

namespace WpfUI.ViewModel
{
    public class ClientsItemViewModel : ViewModelBase
    {
        public DtoClient _model;

        public ClientsItemViewModel(DtoClient model)
        {
            _model = model;
        }

        public string clt_code
        {
            get => _model.clt_code;
            set
            {
                _model.clt_code = value;
                RaisePropertyChanged();
            }
        }
        public string clt_name
        {
            get => _model.clt_name;
            set
            {
                _model.clt_name = value;
                RaisePropertyChanged();
            }
        }
        public string address
        {
            get => _model.address;
            set
            {
                _model.address = value;
                RaisePropertyChanged();
            }
        }
        public string postalcode
        {
            get => _model.postalcode;
            set
            {
                _model.postalcode = value;
                RaisePropertyChanged();
            }
        }
        public string city
        {
            get => _model.city;
            set
            {
                _model.city = value;
                RaisePropertyChanged();
            }
        }
        public string country
        {
            get => _model.country;
            set
            {
                _model.country = value;
                RaisePropertyChanged();
            }
        }
        public string email
        {
            get => _model.email;
            set
            {
                _model.email = value;
                RaisePropertyChanged();
            }
        }
        public string phone
        {
            get => _model.phone;
            set
            {
                _model.phone = value;
                RaisePropertyChanged();
            }
        }
    }
}
