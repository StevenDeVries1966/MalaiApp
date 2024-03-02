using DataLayer.Classes;
using System;

namespace WpfUI.ViewModel
{
    public class WorkedHoursItemViewModel : ValidationViewModelBase
    {
        public DtoWorkedHours _model;

        public WorkedHoursItemViewModel(DtoWorkedHours model)
        {
            _model = model;
        }

        public string emp_code
        {
            get => _model.emp_code;
            set
            {
                _model.emp_code = value;
                RaisePropertyChanged();
                if (string.IsNullOrEmpty(_model.emp_code))
                {
                    AddError("required");
                }
                else
                {
                    ClearError();
                }
                RaisePropertyChanged(nameof(IsValid));
            }
        }
        public int emp_id
        {
            get => _model.emp_id;
            set
            {
                _model.emp_id = value;
                RaisePropertyChanged();
                if (_model.emp_id <= 0)
                {
                    AddError("required");
                }
                else
                {
                    ClearError();
                }
                RaisePropertyChanged(nameof(IsValid));
            }
        }
        public string? clt_code
        {
            get => _model.clt_code;
            set
            {
                _model.clt_code = value;
                RaisePropertyChanged();
                if (string.IsNullOrEmpty(_model.clt_code))
                {
                    AddError("Client code is required");
                }
                else
                {
                    ClearError();
                }
                RaisePropertyChanged(nameof(IsValid));
            }
        }
        public string? job_name
        {
            get => _model.job_name;
            set
            {
                _model.job_name = value;
                RaisePropertyChanged();
                if (string.IsNullOrEmpty(_model.job_name))
                {
                    AddError("Job name is required");
                }
                else
                {
                    ClearError();
                }
                RaisePropertyChanged(nameof(IsValid));
            }
        }
        public int job_id
        {
            get => _model.job_id;
            set
            {
                _model.job_id = value;
                RaisePropertyChanged();
                if (_model.job_id <= 0)
                {
                    AddError("job id is required");
                }
                else
                {
                    ClearError();
                }
                RaisePropertyChanged(nameof(IsValid));
            }
        }
        public DateTime start_time
        {
            get => _model.start_time;
            set
            {
                _model.start_time = value;
                RaisePropertyChanged();
                if (_model.start_time == null)
                {
                    AddError("required");
                }
                else
                {
                    ClearError();
                }
                RaisePropertyChanged(nameof(IsValid));
            }
        }
        public DateTime end_time
        {
            get => _model.end_time;
            set
            {
                _model.end_time = value;
                RaisePropertyChanged();
                if (_model.start_time == null)
                {
                    AddError("required");
                }
                else
                {
                    ClearError();
                }
                RaisePropertyChanged(nameof(IsValid));
            }
        }
        public int week
        {
            get => _model.week;
            set
            {
                _model.week = value;
                RaisePropertyChanged();
                if (_model.week <= 0)
                {
                    AddError("required");
                }
                else
                {
                    ClearError();
                }
                RaisePropertyChanged(nameof(IsValid));
            }
        }
        public int month
        {
            get => _model.month;
            set
            {
                _model.month = value;
                RaisePropertyChanged();
                if (_model.month <= 0)
                {
                    AddError("required");
                }
                else
                {
                    ClearError();
                }
                RaisePropertyChanged(nameof(IsValid));
            }
        }
        public int year
        {
            get => _model.year;
            set
            {
                _model.year = value;
                RaisePropertyChanged();
                if (_model.year <= 0)
                {
                    AddError("required");
                }
                else
                {
                    ClearError();
                }
            }
        }
        public string hours_worked_display
        {
            get => _model.hours_worked_display;
            set
            {
                _model.hours_worked_display = value;
                RaisePropertyChanged();
            }
        }
        public string notes
        {
            get => _model.notes;
            set
            {
                _model.notes = value;
                RaisePropertyChanged();
            }
        }

        public virtual DtoClient? Functie
        {
            get => _model.Client;
            set
            {
                _model.Client = value;

                RaisePropertyChanged();
                if (_model.Client == null)
                {
                    AddError("required");
                }
                else
                {
                    ClearError();
                }
                RaisePropertyChanged(nameof(job_id));
                RaisePropertyChanged(nameof(IsValid));
            }
        }

        public string date_worked_total_displaystring
        {
            get => $"{start_time.ToString("yyyy-MM-dd")}";
        }
        public string hours_worked_total_displaystring
        {
            get => $"from {start_time.ToString("HH:mm")} to {end_time.ToString("HH:mm")} ({hours_worked_display})";
        }
        public bool IsValid
        {
            get
            {
                //Validate();
                string test = (!HasErrors).ToString();
                //MessageBox.Show(test);
                return !HasErrors;
            }
            set
            {
                IsValid = value;
                RaisePropertyChanged();
            }
        }
    }
}
