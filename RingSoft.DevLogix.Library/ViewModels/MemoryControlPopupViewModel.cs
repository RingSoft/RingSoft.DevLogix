using RingSoft.DataEntryControls.Engine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RingSoft.DevLogix.Library.ViewModels
{
    public enum MemoryControlTypes
    {
        [Description("Megabytes (MB)")]
        Mbs = 0,
        [Description("Gigaabytes (GB)")]
        Gbs = 1,
        [Description("Terrabytes (TB)")]
        Tbs = 2,
        [Description("Petabytes (PB)")]
        Pbs = 3,
    }

    public interface IMemoryControlPopupView
    {
        void CloseWindow();
    }

    public class MemoryControlPopupViewModel : INotifyPropertyChanged
    {
        private double _memoryPart;

        public double MemoryPart
        {
            get => _memoryPart;
            set
            {
                if (_memoryPart == value)
                {
                    return;
                }
                _memoryPart = value;
                OnPropertyChanged();
            }
        }

        private TextComboBoxControlSetup _memoryTypeComboBoxControlSetup;

        public TextComboBoxControlSetup MemoryTypeComboBoxControlSetup
        {
            get => _memoryTypeComboBoxControlSetup;
            set
            {
                if (_memoryTypeComboBoxControlSetup == value)
                    return;

                _memoryTypeComboBoxControlSetup = value;
                OnPropertyChanged();
            }
        }

        private TextComboBoxItem _memoryTypeComboBoxItem;

        public TextComboBoxItem MemoryTypeComboBoxItem
        {
            get => _memoryTypeComboBoxItem;
            set
            {
                if (_memoryTypeComboBoxItem == value)
                    return;

                _memoryTypeComboBoxItem = value;
                OnPropertyChanged();
            }
        }

        public MemoryControlTypes Type
        {
            get => (MemoryControlTypes)MemoryTypeComboBoxItem.NumericValue;
            set => MemoryTypeComboBoxItem = MemoryTypeComboBoxControlSetup.GetItem((int)value);
        }

        public IMemoryControlPopupView View { get; private set; }

        public bool DialogResult { get; private set; }

        public double Memory { get; private set; }

        public RelayCommand OkCommand { get; private set; }

        public RelayCommand CancelCommand { get; private set; }

        public MemoryControlPopupViewModel()
        {
            MemoryTypeComboBoxControlSetup = new TextComboBoxControlSetup();
            MemoryTypeComboBoxControlSetup.LoadFromEnum<MemoryControlTypes>();
            Type = MemoryControlTypes.Mbs;
            OkCommand = new RelayCommand(OnOk);
            CancelCommand = new RelayCommand((() =>
            {
                View.CloseWindow();
            }));
        }

        public void Initialize(IMemoryControlPopupView view, double memory)
        {
            View = view;
            Memory = memory;
            SetMemory(memory);
        }

        private void SetMemory(double memory)
        {
            if (memory > 1000)
            {
                var gbs = (double)(memory / 1000);
                if (gbs > 1000)
                {
                    var tbs = gbs / 1000;
                    Type = MemoryControlTypes.Tbs;
                    MemoryPart = tbs;

                    if (tbs > 1000)
                    {
                        var pbs = tbs / 1000;
                        Type = MemoryControlTypes.Pbs;
                        MemoryPart = pbs;
                    }
                }
                else
                {
                    Type = MemoryControlTypes.Gbs;
                    MemoryPart = gbs;
                }
            }
            else
            {
                Type = MemoryControlTypes.Mbs;
                MemoryPart = memory;
            }
        }

        private void OnOk()
        {
            var memory = (double)0;
            switch (Type)
            {
                case MemoryControlTypes.Mbs:
                    memory = MemoryPart;
                    break;
                case MemoryControlTypes.Gbs:
                    memory = MemoryPart * 1000;
                    break;
                case MemoryControlTypes.Tbs:
                    var gHz = MemoryPart * 1000;
                    memory = gHz * 1000;
                    break;
                case MemoryControlTypes.Pbs:
                    var tHz = MemoryPart * 1000;
                    var gHz1 = tHz * 1000;
                    memory = gHz1 * 1000;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
            Memory = memory;
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
