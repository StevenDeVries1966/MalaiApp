using DataLayer.Classes;

namespace WpfUI.ViewModel
{
    public class EmployeeItemViewModel : ViewModelBase
    {
        public DtoEmployee _model;

        public EmployeeItemViewModel(DtoEmployee model)
        {
            _model = model;
        }
        public int emp_id
        {
            get => _model.emp_id;
            set
            {
                _model.emp_id = value;
                RaisePropertyChanged();
            }
        }
        public string emp_code
        {
            get => _model.emp_code;
            set
            {
                _model.emp_code = value;
                RaisePropertyChanged();
            }
        }
        public string first_name
        {
            get => _model.first_name;
            set
            {
                _model.first_name = value;
                RaisePropertyChanged();
            }
        }
        public string last_name
        {
            get => _model.last_name;
            set
            {
                _model.last_name = value;
                RaisePropertyChanged();
            }
        }
        public string login
        {
            get => _model.login;
            set
            {
                _model.login = value;
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

        public string display_name()
        {
            return $"{emp_code} {first_name} {last_name}";
        }
    }
}
