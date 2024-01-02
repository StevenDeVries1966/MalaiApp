using DataLayer.Classes;

namespace WpfUI.ViewModel
{
    public class JobsItemViewModel : ViewModelBase
    {
        public DtoJob _model;

        public JobsItemViewModel(DtoJob model)
        {
            _model = model;
        }
        public int job_id
        {
            get => _model.job_id;
            set
            {
                _model.job_id = value;
                RaisePropertyChanged();
            }
        }

        public string job_name
        {
            get => _model.job_name;
            set
            {
                _model.job_name = value;
                RaisePropertyChanged();
            }
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
    }
}
