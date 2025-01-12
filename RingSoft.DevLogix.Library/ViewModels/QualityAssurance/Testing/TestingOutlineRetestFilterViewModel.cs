using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;

namespace RingSoft.DevLogix.Library.ViewModels.QualityAssurance.Testing
{
    public enum RetestFilterTypes
    {
        Outline = 0,
        Product = 1,
    }

    public class TestingOutlineRetestFilterViewModel : INotifyPropertyChanged
    {
        #region Properties

        private RetestFilterTypes _retestFilterType;

        public RetestFilterTypes RetestFilterType
        {
            get { return _retestFilterType; }
            set
            {
                if (_retestFilterType == value)
                {
                    return;
                }
                _retestFilterType = value;
                OnPropertyChanged();
                SetVisuals();
            }
        }

        private AutoFillSetup _startOutlineAutoFillSetup;

        public AutoFillSetup StartOutlineAutoFillSetup
        {
            get { return _startOutlineAutoFillSetup; }
            set
            {
                if (_startOutlineAutoFillSetup == value)
                    return;

                _startOutlineAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _startOutlineAutoFillValue;

        public AutoFillValue StartOutlineAutoFillValue
        {
            get { return _startOutlineAutoFillValue; }
            set
            {
                if (_startOutlineAutoFillValue == value)
                    return;

                _startOutlineAutoFillValue = value;
                OnPropertyChanged();

                if (!EndOutlineAutoFillValue.IsValid())
                {
                    EndOutlineAutoFillValue = StartOutlineAutoFillValue;
                }
            }
        }

        private AutoFillSetup _endOutlineAutoFillSetup;

        public AutoFillSetup EndOutlineAutoFillSetup
        {
            get { return _endOutlineAutoFillSetup; }
            set
            {
                if (_endOutlineAutoFillSetup == value)
                    return;

                _endOutlineAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _endOutlineAutoFillValue;

        public AutoFillValue EndOutlineAutoFillValue
        {
            get { return _endOutlineAutoFillValue; }
            set
            {
                if (_endOutlineAutoFillValue == value)
                    return;

                _endOutlineAutoFillValue = value;
                OnPropertyChanged();
            }
        }

        private AutoFillSetup _startProductAutoFillSetup;

        public AutoFillSetup StartProductAutoFillSetup
        {
            get { return _startProductAutoFillSetup; }
            set
            {
                if (_startProductAutoFillSetup == value)
                    return;

                _startProductAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _startProductAutoFillValue;

        public AutoFillValue StartProductAutoFillValue
        {
            get { return _startProductAutoFillValue; }
            set
            {
                if (_startProductAutoFillValue == value)
                    return;

                _startProductAutoFillValue = value;
                OnPropertyChanged();

                if (!EndProductAutoFillValue.IsValid())
                {
                    EndProductAutoFillValue = StartProductAutoFillValue;
                }
            }
        }

        private AutoFillSetup _endProductAutoFillSetup;

        public AutoFillSetup EndProductAutoFillSetup
        {
            get { return _endProductAutoFillSetup; }
            set
            {
                if (_endProductAutoFillSetup == value)
                    return;

                _endProductAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _endProductAutoFillValue;

        public AutoFillValue EndProductAutoFillValue
        {
            get { return _endProductAutoFillValue; }
            set
            {
                if (_endProductAutoFillValue == value)
                    return;

                _endProductAutoFillValue = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public UiCommand StartOutlineUiCommand { get; }

        public UiCommand EndOutlineUiCommand { get; }

        public UiCommand StartProductUiCommand { get; }

        public UiCommand EndProductUiCommand { get; }

        public TestingOutlineRetestFilterViewModel()
        {
            StartOutlineUiCommand = new UiCommand();
            EndOutlineUiCommand = new UiCommand();
            StartProductUiCommand = new UiCommand();
            EndProductUiCommand = new UiCommand();

            StartOutlineAutoFillSetup = new AutoFillSetup(AppGlobals.LookupContext.TestingOutlineLookup);
            StartOutlineAutoFillSetup.AllowLookupAdd = StartOutlineAutoFillSetup.AllowLookupView = false;

            EndOutlineAutoFillSetup = new AutoFillSetup(AppGlobals.LookupContext.TestingOutlineLookup);
            EndOutlineAutoFillSetup.AllowLookupAdd = EndOutlineAutoFillSetup.AllowLookupView = false;

            StartProductAutoFillSetup = new AutoFillSetup(AppGlobals.LookupContext.ProductLookup);
            StartProductAutoFillSetup.AllowLookupAdd = StartProductAutoFillSetup.AllowLookupView = false;

            EndProductAutoFillSetup = new AutoFillSetup(AppGlobals.LookupContext.ProductLookup);
            EndProductAutoFillSetup.AllowLookupAdd = EndProductAutoFillSetup.AllowLookupView = false;
        }

        public void Initialize()
        {
            SetVisuals();
        }

        private void SetVisuals()
        {
            StartOutlineUiCommand.IsEnabled = RetestFilterType == RetestFilterTypes.Outline;
            EndOutlineUiCommand.IsEnabled = RetestFilterType == RetestFilterTypes.Outline;
            StartProductUiCommand.IsEnabled = RetestFilterType == RetestFilterTypes.Product;
            EndProductUiCommand.IsEnabled = RetestFilterType == RetestFilterTypes.Product;
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
