using RingSoft.DataEntryControls.Engine;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RingSoft.DevLogix.Library.ViewModels
{
    public interface ISpeedControlView
    {
        bool LaunchPopup();
    }

    public class SpeedControlVewModel : INotifyPropertyChanged
    {
        private string _speed;

        public string Speed
        {
            get => _speed;
            set
            {
                if (_speed == value)
                {
                    return;
                }
                _speed = value;
                OnPropertyChanged();
            }
        }

        public ISpeedControlView View { get; private set; }

        public RelayCommand ShowPopupCommand { get; private set; }

        private decimal _localSpeed;
        public decimal LocalSpeed
        {
            get => _localSpeed;
            set
            {
                _localSpeed = value;
                Speed = AppGlobals.MakeSpeed(_localSpeed);
            }
        }

        public SpeedControlVewModel()
        {
            ShowPopupCommand = new RelayCommand(() =>
            {
                View.LaunchPopup();
            });
        }

        public void Initialize(ISpeedControlView view)
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
