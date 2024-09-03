using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DevLogix.DataAccess.Model;

namespace RingSoft.DevLogix.Library.ViewModels.UserManagement
{
    public interface IUserClockReasonView
    {
        void CloseWindow();
        void EnableOther(bool enable);
        void SetFocusToOther();
    }
    public class UserClockReasonViewModel : INotifyPropertyChanged
    {
        #region Properties

        private ClockOutReasons _clockOutReason;

        public ClockOutReasons ClockOutReason
        {
            get => _clockOutReason;
            set
            {
                if (_clockOutReason == value)
                {
                    return;
                }
                _clockOutReason = value;
                OnPropertyChanged();
                View.EnableOther(_clockOutReason == ClockOutReasons.Other);
            }
        }

        private string _otherReason;

        public string OtherReason
        {
            get => _otherReason;
            set
            {
                if (_otherReason == value)
                {
                    return;
                }
                _otherReason = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public bool DialogResult { get; private set; }

        public IUserClockReasonView View { get; private set; }

        public RelayCommand OnOkCommand { get; private set; }

        public RelayCommand OnCancelCommand { get; private set; }

        public UserClockReasonViewModel()
        {
            ClockOutReason = ClockOutReasons.GoneHome;
            OnOkCommand = new RelayCommand(() =>
            {
                if (ClockOutReason == ClockOutReasons.Other)
                {
                    if (OtherReason.IsNullOrEmpty())
                    {
                        var message = "You must specify a reason for Other";
                        var caption = "Validation Error";
                        View.SetFocusToOther();
                        ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                        return;
                    }
                }
                DialogResult = true;
                View.CloseWindow();
            });
            OnCancelCommand = new RelayCommand(() =>
            {
                View.CloseWindow();
            });
        }

        public void Initialize(IUserClockReasonView view, User user)
        {
            View = view;
            var clockOutReason = (ClockOutReasons)user.ClockOutReason;
            switch (clockOutReason)
            {
                case ClockOutReasons.ClockedIn:
                    ClockOutReason = ClockOutReasons.GoneHome;
                    View.EnableOther(false);
                    break;
                case ClockOutReasons.Other:
                    ClockOutReason = ClockOutReasons.Other;
                    if (!user.OtherClockOutReason.IsNullOrEmpty())
                    {
                        OtherReason = user.OtherClockOutReason;
                    }
                    break;
                default:
                    ClockOutReason = (ClockOutReasons)user.ClockOutReason;
                    View.EnableOther(false);
                    break;
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
