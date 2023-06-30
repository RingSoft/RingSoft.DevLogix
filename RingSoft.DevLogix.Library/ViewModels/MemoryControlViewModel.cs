using RingSoft.DataEntryControls.Engine;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RingSoft.DevLogix.Library.ViewModels
{
    public interface IMemoryControlView
    {
        bool LaunchPopup();
    }

    public class MemoryControlViewModel : INotifyPropertyChanged
    {
        private string _memory;

        public string Memory
        {
            get => _memory;
            set
            {
                if (_memory == value)
                {
                    return;
                }
                _memory = value;
                OnPropertyChanged();
            }
        }

        public IMemoryControlView View { get; private set; }

        public RelayCommand ShowPopupCommand { get; private set; }

        private double _localMemory;
        public double LocalMemory
        {
            get => _localMemory;
            set
            {
                _localMemory = value;
                Memory = AppGlobals.MakeSpace(_localMemory);
            }
        }

        public MemoryControlViewModel()
        {
            ShowPopupCommand = new RelayCommand(() =>
            {
                View.LaunchPopup();
            });
        }

        public void Initialize(IMemoryControlView view)
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
