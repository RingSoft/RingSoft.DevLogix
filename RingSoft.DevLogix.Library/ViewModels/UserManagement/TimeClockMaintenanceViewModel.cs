using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model;
using System;
using System.ComponentModel;
using System.Linq;
using System.Timers;
using RingSoft.DataEntryControls.Engine;

namespace RingSoft.DevLogix.Library.ViewModels.UserManagement
{
    public enum TimeClockModes
    {
        Error = 1,
    }
    public interface ITimeClockView : IDbMaintenanceView
    {
        Error GetError();

        void SetTimeClockMode(TimeClockModes timeClockMode);

        void SetElapsedTime();

        void FocusNotes();
    }

    public class TimeClockMaintenanceViewModel : DevLogixDbMaintenanceViewModel<TimeClock>
    {
        public override TableDefinition<TimeClock> TableDefinition => AppGlobals.LookupContext.TimeClocks;

        private int _id;

        public int Id
        {
            get => _id;
            set
            {
                if (_id == value)
                {
                    return;
                }

                _id = value;
                OnPropertyChanged();
            }
        }

        private AutoFillSetup _userAutoFillSetup;

        public AutoFillSetup UserAutoFillSetup
        {
            get => _userAutoFillSetup;
            set
            {
                if (_userAutoFillSetup == value)
                {
                    return;
                }

                _userAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _userAutoFillValue;

        public AutoFillValue UserAutoFillValue
        {
            get => _userAutoFillValue;
            set
            {
                if (_userAutoFillValue == value)
                {
                    return;
                }

                _userAutoFillValue = value;
                OnPropertyChanged();
            }
        }

        private string _keyText;

        public string KeyText
        {
            get => _keyText;
            set
            {
                if (_keyText == value)
                {
                    return;
                }

                _keyText = value;
                OnPropertyChanged();
            }
        }

        private AutoFillSetup _errorAutoFillSetup;

        public AutoFillSetup ErrorAutoFillSetup
        {
            get => _errorAutoFillSetup;
            set
            {
                if (_errorAutoFillSetup == value)
                {
                    return;
                }

                _errorAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _errorAutoFillValue;

        public AutoFillValue ErrorAutoFillValue
        {
            get => _errorAutoFillValue;
            set
            {
                if (_errorAutoFillValue == value)
                {
                    return;
                }

                _errorAutoFillValue = value;
                OnPropertyChanged();
            }
        }

        private DateTime _punchInDate;

        public DateTime PunchInDate
        {
            get => _punchInDate;
            set
            {
                if (_punchInDate == value)
                {
                    return;
                }

                _punchInDate = value;
                ChangePunchDates(true);
                OnPropertyChanged();
            }
        }

        private DateTime? _punchOutDate;

        public DateTime? PunchOutDate
        {
            get => _punchOutDate;
            set
            {
                if (_punchOutDate == value)
                {
                    return;
                }

                _punchOutDate = value;
                ChangePunchDates(false);
                OnPropertyChanged();
            }
        }

        private string _notes;

        public string? Notes
        {
            get => _notes;
            set
            {
                if (_notes == value)
                {
                    return;
                }

                _notes = value;
                OnPropertyChanged();
            }
        }

        private bool _isEdited;

        public bool IsEdited
        {
            get => _isEdited;
            set
            {
                if (_isEdited == value)
                    return;
                _isEdited = value;
                OnPropertyChanged();
            }
        }

        private string _elapsedTime;

        public string ElapsedTime
        {
            get => _elapsedTime;
            set
            {
                if (_elapsedTime == value)
                {
                    return;
                }
                _elapsedTime = value;
                //OnPropertyChanged(null, false);
            }
        }

        public new ITimeClockView View { get; private set; }

        public decimal MinutesSpent { get; private set; }

        public RelayCommand PunchOutCommand { get; private set; }

        private DateTime _endDate;
        private Timer _timer = new Timer();
        private bool _loading;
        private bool _timerActive;

        public TimeClockMaintenanceViewModel()
        {
            PunchOutCommand = new RelayCommand(PunchOut);
        }

        private void SetError(Error error)
        {
            ErrorAutoFillValue = ErrorAutoFillSetup.GetAutoFillValueForIdValue(error.Id);
            KeyText = "Error";
        }

        protected override void Initialize()
        {
            _loading = true;
            NewButtonEnabled = false;
            var punchIn = false;
            _timer.Elapsed += (sender, args) =>
            {
                if (_timerActive)
                {
                    _endDate = DateTime.Now;
                    SetElapsedTime(GetElapsedTime());
                }
            };

            ErrorAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.ErrorId));
            if (base.View is ITimeClockView timeClockView)
            {
                View = timeClockView;
                var error = View.GetError();
                if (error != null)
                {
                    SetError(error);
                    punchIn = true;
                }

            }

            UserAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.UserId));
            UserAutoFillValue = UserAutoFillSetup.GetAutoFillValueForIdValue(AppGlobals.LoggedInUser.Id);
            PunchInDate = DateTime.Now;

            if (punchIn)
            {
                PunchIn(true);
            }

            base.Initialize();
            if (punchIn)
            {
                if (TableDefinition.HasRight(RightTypes.AllowDelete))
                {
                    DeleteButtonEnabled = true;
                }
            }

            _loading = false;
        }

        protected override TimeClock PopulatePrimaryKeyControls(TimeClock newEntity, PrimaryKeyValue primaryKeyValue)
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            var timeClock = context.GetTable<TimeClock>().FirstOrDefault(p => p.Id == newEntity.Id);
            Id = newEntity.Id;
            PunchOutCommand.IsEnabled =
                !timeClock.PunchOutDate.HasValue && timeClock.UserId == AppGlobals.LoggedInUser.Id;
            return timeClock;
        }

        protected override void LoadFromEntity(TimeClock entity)
        {
            _loading = true;
            UserAutoFillValue = UserAutoFillSetup.GetAutoFillValueForIdValue(entity.UserId);
            ErrorAutoFillValue = ErrorAutoFillSetup.GetAutoFillValueForIdValue(entity.ErrorId);
            PunchInDate = entity.PunchInDate.ToLocalTime();
            PunchOutDate = entity.PunchOutDate;
            if (PunchOutDate.HasValue)
            {
                PunchOutDate = PunchOutDate.Value.ToLocalTime();
                _endDate = PunchOutDate.Value;
            }
            else
            {
                PunchIn(false);
            }

            Notes = entity.Notes;
            IsEdited = entity.AreDatesEdited;

            if (ErrorAutoFillValue.IsValid())
            {
                KeyText = "Error";
            }

            _loading = false;
            if (PunchOutDate.HasValue)
            {
                ChangePunchDates(false);
                IsEdited = entity.AreDatesEdited;
            }

            View.FocusNotes();
        }

        protected override TimeClock GetEntityData()
        {
            if (PunchOutDate != null)
            {
                var duration = PunchOutDate.Value.Subtract(PunchInDate);
                MinutesSpent = (decimal)duration.TotalMinutes;
            }

            var timeClock = new TimeClock
            {
                Id = Id,
                UserId = UserAutoFillValue.GetEntity(AppGlobals.LookupContext.Users).Id,
                ErrorId = ErrorAutoFillValue.GetEntity(AppGlobals.LookupContext.Errors).Id,
                PunchInDate = PunchInDate.ToUniversalTime(),
                PunchOutDate = PunchOutDate,
                MinutesSpent = MinutesSpent,
                Notes = Notes,
                AreDatesEdited = IsEdited
            };
            if (timeClock.PunchOutDate.HasValue)
            {
                timeClock.PunchOutDate = timeClock.PunchOutDate.Value.ToUniversalTime();
            }

            if (timeClock.ErrorId == 0)
            {
                timeClock.ErrorId = null;
            }

            return timeClock;
        }

        protected override void ClearData()
        {

        }

        protected override bool SaveEntity(TimeClock entity)
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            return context.SaveEntity(entity, "Saving Time Clock");
        }

        protected override bool DeleteEntity()
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            var timeClock = context.GetTable<TimeClock>().FirstOrDefault(p => p.Id == Id);
            var result = context.DeleteEntity(timeClock, "Deleting Time Clock");

            if (result)
            {
                if (context.GetTable<TimeClock>().Any())
                {
                    OnGotoNextButton();
                }
                else
                {
                    Processor.CloseWindow();
                }
            }

            return result;
        }

        public void PunchIn(bool save)
        {
            if (!_timer.Enabled)
            {
                _timer.Start();
            }
            
            _timerActive = true;
            if (save)
            {
                var timeClock = GetEntityData();
                if (SaveEntity(timeClock))
                {
                    Id = timeClock.Id;
                }

                MaintenanceMode = DbMaintenanceModes.EditMode;
            }
        }

        private string GetElapsedTime()
        {
            var result = string.Empty;

            var duration = _endDate.Subtract(PunchInDate);
            result = $"{duration.Days.ToString("00")} {duration.ToString("hh\\:mm\\:ss")}";

            return result;
        }

        public override void OnWindowClosing(CancelEventArgs e)
        {
            StopTimer();
            base.OnWindowClosing(e);
        }

        private void PunchOut()
        {
            _loading = true;
            PunchOutSave();
            View.FocusNotes();
            _loading = false;
        }

        private void PunchOutSave(bool save = true)
        {
            if (!PunchOutDate.HasValue)
            {
                PunchOutDate = DateTime.Now;
            }

            _endDate = PunchOutDate.Value;
            StopTimer();
            if (save)
            {
                DoSave();
            }
        }
        private void StopTimer()
        {
            _timer.Stop();
            _timer.Enabled = false;
            _timerActive = false;
        }

        private void ChangePunchDates(bool fromPunchIn)
        {
            if (_loading)
            {
                return;
            }

            var elapsedTime = string.Empty;
            if (PunchOutDate.HasValue)
            {
                PunchOutCommand.IsEnabled = false;
                if (!fromPunchIn)
                {
                    PunchOutSave(false);
                }
            }
            else if (!fromPunchIn)
            {
                _endDate = DateTime.Now;
                PunchIn(false);
            }

            IsEdited = true;
            if (PunchOutDate.HasValue)
            {
                _endDate = PunchOutDate.Value;
                StopTimer();
                elapsedTime = GetElapsedTime();
                SetElapsedTime(elapsedTime);
            }
            else
            {
                if (_timer.Enabled)
                {
                    elapsedTime = GetElapsedTime();
                    SetElapsedTime(elapsedTime);
                }
            }
        }

        private void SetElapsedTime(string elapsedTime)
        {
            ElapsedTime = elapsedTime;
            View.SetElapsedTime();
        }
}
}
