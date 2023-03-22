using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public class ProjectScheduleViewModel : INotifyPropertyChanged
    {
        private DateTime _startDate;

        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                if (_startDate == value)
                {
                    return;
                }
                _startDate = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
