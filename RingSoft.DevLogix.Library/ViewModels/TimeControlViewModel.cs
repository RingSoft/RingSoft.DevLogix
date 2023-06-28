using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;

namespace RingSoft.DevLogix.Library.ViewModels
{
    public interface ITimeControlView
    {
        bool LaunchPopup();
    }
    public class TimeControlViewModel : INotifyPropertyChanged
    {
        private string _timeSpent;

        public string TimeSpent
        {
            get => _timeSpent;
            set
            {
                if (_timeSpent == value)
                {
                    return;
                }
                _timeSpent = value;
                OnPropertyChanged();
            }
        }

        public ITimeControlView View { get; private set; }

        public RelayCommand ShowPopupCommand { get; private set; }

        private double _minutes;
        public double Minutes
        {
            get => _minutes;
            set
            {
                _minutes = value;
                TimeSpent = AppGlobals.MakeTimeSpent(_minutes);
            }
        }

        public TimeControlViewModel()
        {
            ShowPopupCommand = new RelayCommand(() =>
            {
                View.LaunchPopup();
            });
        }

        public void Initialize(ITimeControlView view)
        {
            View = view;
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
