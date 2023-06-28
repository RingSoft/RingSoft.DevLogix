using RingSoft.DataEntryControls.Engine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RingSoft.DevLogix.Library.ViewModels
{
    public enum SpeedControlTypes
    {
        [Description("Megahertz (MHz)")]
        Mhz = 0,
        [Description("Gigahertz (GHz)")]
        Ghz = 1,
        [Description("Terahertz (THz)")]
        Thz = 2,
        [Description("Petahertz (PHz)")]
        Phz = 3,
    }

    public interface ISpeedControlPopupView
    {
        void CloseWindow();
    }

    public class SpeedControlPopupViewModel : INotifyPropertyChanged
    {
        private double _speedPart;

        public double SpeedPart
        {
            get => _speedPart;
            set
            {
                if (_speedPart == value)
                {
                    return;
                }
                _speedPart = value;
                OnPropertyChanged();
            }
        }

        private TextComboBoxControlSetup _speedTypeComboBoxControlSetup;

        public TextComboBoxControlSetup SpeedTypeComboBoxControlSetup
        {
            get => _speedTypeComboBoxControlSetup;
            set
            {
                if (_speedTypeComboBoxControlSetup == value)
                    return;

                _speedTypeComboBoxControlSetup = value;
                OnPropertyChanged();
            }
        }

        private TextComboBoxItem _speedTypeComboBoxItem;

        public TextComboBoxItem SpeedTypeComboBoxItem
        {
            get => _speedTypeComboBoxItem;
            set
            {
                if (_speedTypeComboBoxItem == value)
                    return;

                _speedTypeComboBoxItem = value;
                OnPropertyChanged();
            }
        }

        public SpeedControlTypes Type
        {
            get => (SpeedControlTypes)SpeedTypeComboBoxItem.NumericValue;
            set => SpeedTypeComboBoxItem = SpeedTypeComboBoxControlSetup.GetItem((int)value);
        }

        public ISpeedControlPopupView View { get; private set; }

        public bool DialogResult { get; private set; }

        public double Speed { get; private set; }

        public RelayCommand OkCommand { get; private set; }

        public RelayCommand CancelCommand { get; private set; }

        public SpeedControlPopupViewModel()
        {
            SpeedTypeComboBoxControlSetup = new TextComboBoxControlSetup();
            SpeedTypeComboBoxControlSetup.LoadFromEnum<SpeedControlTypes>();
            Type = SpeedControlTypes.Mhz;
            OkCommand = new RelayCommand(OnOk);
            CancelCommand = new RelayCommand((() =>
            {
                View.CloseWindow();
            }));
        }

        public void Initialize(ISpeedControlPopupView view, double speed)
        {
            View = view;
            Speed = speed;
            SetSpeed(speed);
        }

        private void SetSpeed(double speed)
        {
            if (speed > 1000)
            {
                var gHz = speed / 1000;
                if (gHz > 1000)
                {
                    var tHz = gHz / 1000;
                    Type = SpeedControlTypes.Thz;
                    SpeedPart = tHz;

                    if (tHz > 1000)
                    {
                        var pHz = tHz / 1000;
                        Type = SpeedControlTypes.Phz;
                        SpeedPart = pHz;
                    }
                }
                else
                {
                    Type = SpeedControlTypes.Ghz;
                    SpeedPart = gHz;
                }
            }
            else
            {
                Type = SpeedControlTypes.Mhz;
                SpeedPart = speed;
            }
        }

        private void OnOk()
        {
            var speed = (double)0;
            switch (Type)
            {
                case SpeedControlTypes.Mhz:
                    speed = SpeedPart;
                    break;
                case SpeedControlTypes.Ghz:
                    speed = SpeedPart * 1000;
                    break;
                case SpeedControlTypes.Thz:
                    var gHz = SpeedPart * 1000;
                    speed = gHz * 1000;
                    break;
                case SpeedControlTypes.Phz:
                    var tHz = SpeedPart * 1000;
                    var gHz1 = tHz * 1000;
                    speed = gHz1 * 1000;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
            Speed = speed;
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
