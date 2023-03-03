using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using RingSoft.DataEntryControls.Engine;

namespace RingSoft.DevLogix.Library.ViewModels
{
    public enum TimeControlPeriods
    {
        [Description("Minute(s)")]
        Minutes = 0,
        [Description("Hour(s)")]
        Hours = 1,
        [Description("Day(s)")]
        Days = 2,
    }

    public interface ITimeControlPopupView
    {
        void CloseWindow();
    }
    public class TimeControlPopupViewModel : INotifyPropertyChanged
    {
        private decimal _timePart;

        public decimal TimePart
        {
            get => _timePart;
            set
            {
                if (_timePart == value)
                {
                    return;
                }
                _timePart = value;
                OnPropertyChanged();
            }
        }

        private TextComboBoxControlSetup _timePeriodComboBoxControlSetup ;

        public TextComboBoxControlSetup TimePeriodComboBoxControlSetup  
        {
            get => _timePeriodComboBoxControlSetup;
            set
            {
                if (_timePeriodComboBoxControlSetup == value)
                    return;

                _timePeriodComboBoxControlSetup = value;
                OnPropertyChanged();
            }
        }

        private TextComboBoxItem _timePeriodComboBoxItem;

        public TextComboBoxItem TimePeriodComboBoxItem
        {
            get => _timePeriodComboBoxItem;
            set
            {
                if (_timePeriodComboBoxItem == value)
                    return;

                _timePeriodComboBoxItem = value;
                OnPropertyChanged();
            }
        }

        public TimeControlPeriods Period
        {
            get => (TimeControlPeriods)TimePeriodComboBoxItem.NumericValue;
            set => TimePeriodComboBoxItem = TimePeriodComboBoxControlSetup.GetItem((int)value);
        }

        public ITimeControlPopupView View { get; private set; }

        public bool DialogResult { get; private set; }

        public decimal Minutes { get; private set; }

        public RelayCommand OkCommand { get; private set; }

        public RelayCommand CancelCommand { get; private set; }

        public TimeControlPopupViewModel()
        {
            TimePeriodComboBoxControlSetup = new TextComboBoxControlSetup();
            TimePeriodComboBoxControlSetup.LoadFromEnum<TimeControlPeriods>();
            Period = TimeControlPeriods.Minutes;
            OkCommand = new RelayCommand(OnOk);
            CancelCommand = new RelayCommand((() =>
            {
                View.CloseWindow();
            }));
        }

        public void Initialize(ITimeControlPopupView view, decimal minutes)
        {
            View = view;
            Minutes = minutes;
            SetMinutes(minutes);
        }

        private void SetMinutes(decimal minutes)
        {
            if (minutes > 60)
            {
                var hours = minutes / 60;
                if (hours > 24)
                {
                    var days = hours / 24;
                    Period = TimeControlPeriods.Days;
                    TimePart = days;
                }
                else
                {
                    Period = TimeControlPeriods.Hours;
                    TimePart = hours;
                }
            }
            else
            {
                Period = TimeControlPeriods.Minutes;
                TimePart = minutes;
            }
        }

        private void OnOk()
        {
            var minutes = (decimal)0;
            switch (Period)
            {
                case TimeControlPeriods.Minutes:
                    minutes = TimePart;
                    break;
                case TimeControlPeriods.Hours:
                    minutes = TimePart * 60;
                    break;
                case TimeControlPeriods.Days:
                    var hours = TimePart * 24;
                    minutes = hours * 60;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            Minutes = minutes;
            DialogResult = true;
            View.CloseWindow();
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
