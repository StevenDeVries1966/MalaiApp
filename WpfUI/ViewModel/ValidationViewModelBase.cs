using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace WpfUI.ViewModel
{
    public class ValidationViewModelBase : ViewModelBase, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> _errorsByPropertyname = new();
        public bool HasErrors => _errorsByPropertyname.Any();

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public IEnumerable GetErrors(string? propertyName)
        {
            return propertyName is not null && _errorsByPropertyname!.ContainsKey(propertyName)
                ? _errorsByPropertyname[propertyName]
                : Enumerable.Empty<string>();
        }

        protected virtual void OnErrorsChanged(DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
        }

        protected void AddError(string error, [CallerMemberName] string? propertyName = null)
        {
            if (propertyName is null) return;
            if (!_errorsByPropertyname.ContainsKey(propertyName))
            {
                _errorsByPropertyname[propertyName] = new List<string>();
            }

            if (!_errorsByPropertyname[propertyName].Contains(error))
            {
                _errorsByPropertyname[propertyName].Add(error);
                OnErrorsChanged(new DataErrorsChangedEventArgs(propertyName));
                RaisePropertyChanged(nameof(HasErrors));
            }
        }
        protected void ClearError([CallerMemberName] string? propertyName = null)
        {
            if (propertyName is null) return;
            if (!_errorsByPropertyname.ContainsKey(propertyName))
            {
                _errorsByPropertyname.Remove(propertyName);
                OnErrorsChanged(new DataErrorsChangedEventArgs(propertyName));
                RaisePropertyChanged(nameof(HasErrors));
            }
        }
    }
}
