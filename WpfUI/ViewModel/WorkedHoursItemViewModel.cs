﻿using DataLayer.Classes;
using System;

namespace WpfUI.ViewModel
{
    public class WorkedHoursItemViewModel : ViewModelBase
    {
        public DtoWorkedHours _model;

        public WorkedHoursItemViewModel(DtoWorkedHours model)
        {
            _model = model;
        }

        public int entry_id => _model.entry_id;

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
        public string clt_code
        {
            get => _model.clt_code;
            set
            {
                _model.clt_code = value;
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
        public DateTime start_time
        {
            get => _model.start_time;
            set
            {
                _model.start_time = value;
                RaisePropertyChanged();
            }
        }
        public DateTime end_time
        {
            get => _model.end_time;
            set
            {
                _model.end_time = value;
                RaisePropertyChanged();
            }
        }
        public int week
        {
            get => _model.week;
            set
            {
                _model.week = value;
                RaisePropertyChanged();
            }
        }
        public int month
        {
            get => _model.month;
            set
            {
                _model.month = value;
                RaisePropertyChanged();
            }
        }
        public int year
        {
            get => _model.year;
            set
            {
                _model.year = value;
                RaisePropertyChanged();
            }
        }

    }
}
