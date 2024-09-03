using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using RingSoft.DataEntryControls.Engine;

namespace RingSoft.DevLogix.Library.ViewModels.UserManagement
{
    public interface ITimeClockManualPunchOutView
    {
        void CloseWindow();
    }
    public class TimeClockManualPunchOutViewModel : INotifyPropertyChanged
    {

        #region Properties

        private DateTime _punchInDateTime;

        public DateTime PunchInDate
        {
            get => _punchInDateTime;
            set
            {
                if (_punchInDateTime == value)
                {
                    return;
                }
                _punchInDateTime = value;
                OnPropertyChanged();
            }
        }

        private double _minutesToAdd;

        public double MinutesTodd
        {
            get => _minutesToAdd;
            set
            {
                if (_minutesToAdd == value)
                    return;

                _minutesToAdd = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public DateTime PunchOutDate { get; private set; }
        public RelayCommand OkCommand { get; }

        public RelayCommand CancelCommand { get; }

        public bool DialogResult { get; private set; }

        public ITimeClockManualPunchOutView View { get; private set; }

        public TimeClockManualPunchOutViewModel()
        {
            var today = DateTime.Today;
            PunchInDate = new DateTime(today.Year, today.Month, today.Day, 0, 0, 0);
            OkCommand = new RelayCommand((() =>
            {
                var punchOutDate = PunchInDate.AddMinutes(MinutesTodd);
                PunchOutDate = punchOutDate;
                DialogResult = true;
                View.CloseWindow();
            }));

            CancelCommand = new RelayCommand((() =>
            {
                View.CloseWindow();
            }));
        }

        public void Initialize(ITimeClockManualPunchOutView view)
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
