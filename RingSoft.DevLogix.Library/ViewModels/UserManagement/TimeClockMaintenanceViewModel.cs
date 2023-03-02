using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model;
using System;
using System.ComponentModel;
using System.Linq;
using System.Timers;
using Microsoft.EntityFrameworkCore;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;
using IDbContext = RingSoft.DevLogix.DataAccess.IDbContext;

namespace RingSoft.DevLogix.Library.ViewModels.UserManagement
{
    public enum TimeClockModes
    {
        Error = 1,
        Project = 2,
    }
    public interface ITimeClockView : IDbMaintenanceView
    {
        Error GetError();

        Project GetProject();

        void SetTimeClockMode(TimeClockModes timeClockMode);

        void SetElapsedTime();

        void FocusNotes();

        void SetDialogMode();
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
                OnPropertyChanged(null, false);
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
                OnPropertyChanged(null, false);
            }
        }

        private AutoFillSetup _projectAutoFillSetup;

        public AutoFillSetup ProjectAutoFillSetup
        {
            get => _projectAutoFillSetup;
            set
            {
                if (_projectAutoFillSetup == value)
                {
                    return;
                }

                _projectAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _projectAutoFillValue;

        public AutoFillValue ProjectAutoFillValue
        {
            get => _projectAutoFillValue;
            set
            {
                if (_projectAutoFillValue == value)
                {
                    return;
                }

                _projectAutoFillValue = value;
                OnPropertyChanged(null, false);
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

        public decimal OriginalMinutesSpent { get; private set; }

        public DialogInput DialogInput { get; private set; }

        public RelayCommand PunchOutCommand { get; private set; }

        private DateTime _endDate;
        private Timer _timer = new Timer();
        private bool _loading;
        private bool _timerActive;
        private bool _setDirty = true;

        public TimeClockMaintenanceViewModel()
        {
            PunchOutCommand = new RelayCommand((() =>
            {
                _setDirty = false;
                PunchOut();
                _setDirty = true;
            }));
        }

        private void SetError(Error error)
        {
            ErrorAutoFillValue = ErrorAutoFillSetup.GetAutoFillValueForIdValue(error.Id);
        }

        private void SetProject(Project project)
        {
            ProjectAutoFillValue = ProjectAutoFillSetup.GetAutoFillValueForIdValue(project.Id);
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
            ProjectAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.ProjectId));

            if (base.View is ITimeClockView timeClockView)
            {
                View = timeClockView;

                var timeClockMode = TimeClockModes.Error;
                var error = View.GetError();
                if (error != null)
                {
                    SetError(error);
                    punchIn = true;
                }
                var project = View.GetProject();
                if (project != null)
                {
                    SetProject(project);
                    punchIn = true;
                    timeClockMode = TimeClockModes.Project;
                }

                View.SetTimeClockMode(timeClockMode);

                if (InputParameter is DialogInput dialogInput)
                {
                    DialogInput = dialogInput;
                    NextCommand.IsEnabled = false;
                    PreviousCommand.IsEnabled = false;
                    View.SetDialogMode();
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
            var timeClock = context.GetTable<TimeClock>().Include(p => p.User)
                .FirstOrDefault(p => p.Id == newEntity.Id);
            Id = newEntity.Id;
            if (timeClock.MinutesSpent != null) 
                OriginalMinutesSpent = timeClock.MinutesSpent.Value;
            if (!timeClock.PunchOutDate.HasValue)
            {
                if (timeClock.UserId == AppGlobals.LoggedInUser.Id)
                {
                    EnablePunchOutDate();
                }
                else if (timeClock.User.IsSupervisor())
                {
                    EnablePunchOutDate();
                }
                else
                {
                    PunchOutCommand.IsEnabled = false;
                }
            }
            return timeClock;
        }

        private void EnablePunchOutDate()
        {
            ChangePunchDates(false);
        }

        protected override void LoadFromEntity(TimeClock entity)
        {
            _loading = true;
            UserAutoFillValue = UserAutoFillSetup.GetAutoFillValueForIdValue(entity.UserId);
            ErrorAutoFillValue = ErrorAutoFillSetup.GetAutoFillValueForIdValue(entity.ErrorId);
            ProjectAutoFillValue = ProjectAutoFillSetup.GetAutoFillValueForIdValue(entity.ProjectId);

            var timeClockMode = TimeClockModes.Error;

            if (ErrorAutoFillValue.IsValid())
            {
                timeClockMode = TimeClockModes.Error;
            }
            else if (ProjectAutoFillValue.IsValid())
            {
                timeClockMode = TimeClockModes.Project;
            }
            View.SetTimeClockMode(timeClockMode);
            

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
                ProjectId =ProjectAutoFillValue.GetEntity(AppGlobals.LookupContext.Projects).Id,
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

            if (timeClock.ProjectId == 0)
            {
                timeClock.ProjectId = null;
            }

            return timeClock;
        }

        protected override void ClearData()
        {
        }

        protected override bool SaveEntity(TimeClock entity)
        {
            var saveChildren = entity.Id != 0;
            var context = AppGlobals.DataRepository.GetDataContext();
            var user = context.GetTable<User>().FirstOrDefault(p => p.Id == entity.UserId);
            Project project = null;
            var result = context.SaveNoCommitEntity(entity, "Saving Time Clock");
            if (result && saveChildren)
            {
                if (entity.ProjectId.HasValue)
                {
                    project = context.GetTable<Project>()
                        .Include(p => p.ProjectUsers)
                        .FirstOrDefault(p => p.Id == entity.ProjectId.Value);
                    if (project != null)
                    {
                        result = UpdateProject(entity, project, context, user);
                        if (result && project.IsBillable)
                        {
                            user.BillableProjectsMinutesSpent += GetNewMinutesSpent();
                        }
                        else
                        {
                            user.NonBillableProjectsMinutesSpent += GetNewMinutesSpent();
                        }
                    }
                }
                else if (entity.ErrorId.HasValue)
                {
                    var error = context.GetTable<Error>()
                        .Include(p => p.Users)
                        .ThenInclude(p => p.User)
                        .FirstOrDefault(p => p.Id == entity.ErrorId.Value);
                    if (error != null)
                    {
                        result = UpdateError(entity, error, context, user);
                    }
                }

                if (result)
                {
                    result = context.SaveNoCommitEntity(user, "Saving User");
                }
            }

            if (result)
            {
                result = context.Commit("Saving Timeclock");
                if (result && saveChildren)
                {
                    var viewModels = AppGlobals.MainViewModel.UserViewModels.Where(p => p.Id == user.Id);

                    if (viewModels.Any())
                    {
                        foreach (var model in viewModels)
                        {
                            model.RefreshBillability(user);
                        }
                    }

                    if (project != null)
                    {
                        var projectViewModels = AppGlobals.MainViewModel.ProjectViewModels
                            .Where(p => p.Id == project.Id);

                        if (projectViewModels.Any())
                        {
                            foreach (var model in projectViewModels)
                            {
                                model.RefreshCostGrid(project);
                            }
                        }
                    }
                }

            }
            return result;
        }

        private bool UpdateError(TimeClock entity, Error error, IDbContext context, User user)
        {
            var result = true;
            user.ErrorsMinutesSpent += entity.MinutesSpent.Value;
            error.MinutesSpent += entity.MinutesSpent.Value;
            var errorUser = error.Users.FirstOrDefault(p => p.UserId == user.Id);
            if (errorUser != null)
            {
                errorUser.MinutesSpent += GetNewMinutesSpent();
                errorUser.Cost = Math.Round((errorUser.MinutesSpent / 60) * user.HourlyRate, 2);
                result = context.SaveNoCommitEntity(errorUser, "Saving Error User");
            }

            if (result)
            {
                AppGlobals.CalculateError(error, error.Users.ToList());
                result = context.SaveNoCommitEntity(error, "Saving Error");
            }

            var errorViewModels = AppGlobals.MainViewModel.ErrorViewModels.Where(p => p.Id == error.Id);
            foreach (var errorViewModel in errorViewModels)
            {
                errorViewModel.RefreshCost(errorUser);
            }
            return result;

        }

        private bool UpdateProject(TimeClock entity, Project project, IDbContext context, User user)
        {
            var result = true;
            project.MinutesSpent += entity.MinutesSpent.Value;
            var projectUser = project.ProjectUsers.FirstOrDefault(p => p.UserId == user.Id);
            if (projectUser != null)
            {
                projectUser.MinutesSpent += GetNewMinutesSpent();
                projectUser.Cost = Math.Round((projectUser.MinutesSpent / 60) * user.HourlyRate, 2);
                result = context.SaveNoCommitEntity(projectUser, "Saving Project User");
            }

            if (result)
            {
                AppGlobals.CalculateProject(project, project.ProjectUsers.ToList());
                result = context.SaveNoCommitEntity(project, "Saving Project");
            }

            return result;
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

                var context = AppGlobals.DataRepository.GetDataContext();
                var usersQuery = context.GetTable<User>();
                if (usersQuery != null)
                {
                    var user = usersQuery.FirstOrDefault(p => p.Id == timeClock.UserId);
                    if (user != null && user.ClockDate == null)
                    {
                        user.ClockDate = timeClock.PunchInDate;
                    }

                    context.SaveEntity(user, "Updating Clock Date");
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
            if (DialogInput != null && PunchOutDate != null)
            {
                DialogInput.DialogResult = true;
            }
            base.OnWindowClosing(e);
        }

        private void PunchOut()
        {
            //_loading = true;
            PunchOutSave();
            View.FocusNotes();
            //_loading = false;
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

            if (_setDirty)
            {
                IsEdited = true;
            }

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

        public decimal GetNewMinutesSpent()
        {
            var result = MinutesSpent - OriginalMinutesSpent;
            return result;
        }
    }
}
