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

        void SetElapsedTime(string elapsedTime);
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

        public new ITimeClockView View { get; private set; }

        public decimal MinutesSpent { get; private set; }

        public RelayCommand PunchOutCommand { get; private set; }

        private DateTime _endDate;
        private Timer _timer = new Timer();

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
            NewButtonEnabled = false;
            var punchIn = false;
            _timer.Elapsed += (sender, args) =>
            {
                _endDate = DateTime.Now;
                View.SetElapsedTime(GetElapsedTime());
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
        }

        protected override TimeClock PopulatePrimaryKeyControls(TimeClock newEntity, PrimaryKeyValue primaryKeyValue)
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            var timeClock = context.GetTable<TimeClock>().FirstOrDefault(p => p.Id == newEntity.Id);
            Id = newEntity.Id;
            PunchOutCommand.IsEnabled = false;
            return timeClock;
        }

        protected override void LoadFromEntity(TimeClock entity)
        {
            UserAutoFillValue = UserAutoFillSetup.GetAutoFillValueForIdValue(entity.UserId);
            ErrorAutoFillValue = ErrorAutoFillSetup.GetAutoFillValueForIdValue(entity.ErrorId);
            PunchInDate = entity.PunchInDate.ToLocalTime();
            PunchOutDate = entity.PunchOutDate;
            if (PunchOutDate.HasValue)
            {
                PunchOutDate = PunchOutDate.Value.ToLocalTime();
                _endDate = PunchOutDate.Value;
                View.SetElapsedTime(GetElapsedTime());
            }
            Notes = entity.Notes;

            if (PunchOutDate == null && entity.UserId == AppGlobals.LoggedInUser.Id)
            {
                PunchIn(false);
            }

            if (ErrorAutoFillValue.IsValid())
            {
                KeyText = "Error";
            }
        }

        protected override TimeClock GetEntityData()
        {
            if (PunchOutDate != null && MinutesSpent.Equals(0))
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
                Notes = Notes
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
                if (!context.GetTable<TimeClock>().Any())
                {
                    Processor.CloseWindow();
                }
            }
            return result;
        }

        private void PunchIn(bool save)
        {
            _timer.Start();
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
            result = duration.ToString("hh\\:mm\\:ss");

            return result;
        }

        public override void OnWindowClosing(CancelEventArgs e)
        {
            _timer.Stop();
            _timer.Enabled = false;
            base.OnWindowClosing(e);
        }

        private void PunchOut()
        {
            PunchOutDate = DateTime.Now;
            _endDate = PunchOutDate.Value;
            _timer.Stop();
            _timer.Enabled = false;
            DoSave();
        }
    }
}
