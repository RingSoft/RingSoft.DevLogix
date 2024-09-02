using RingSoft.App.Interop;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.LookupModel;
using RingSoft.DevLogix.DataAccess.Model;
using System;

namespace RingSoft.DevLogix.Library.ViewModels.UserManagement
{
    public interface IDepartmentView : IDbMaintenanceView
    {
        string FtpPassword { get; set; }
    }
    public class DepartmentMaintenanceViewModel : DbMaintenanceViewModel<Department>
    {
        #region Properties

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

        private AutoFillSetup _fixStatusAutoFillSetup;

        public AutoFillSetup FixStatusAutoFillSetup
        {
            get => _fixStatusAutoFillSetup;
            set
            {
                if (_fixStatusAutoFillSetup == value)
                    return;

                _fixStatusAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _fixStatusAutoFillValue;

        public AutoFillValue FixStatusAutoFillValue
        {
            get => _fixStatusAutoFillValue;
            set
            {
                if (_fixStatusAutoFillValue == value)
                    return;

                _fixStatusAutoFillValue = value;
                OnPropertyChanged();
            }
        }

        private AutoFillSetup _passStatusAutoFillSetup;

        public AutoFillSetup PassStatusAutoFillSetup
        {
            get => _passStatusAutoFillSetup;
            set
            {
                if (_passStatusAutoFillSetup == value)
                    return;

                _passStatusAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _passStatusAutoFillValue;

        public AutoFillValue PassStatusAutoFillValue
        {
            get => _passStatusAutoFillValue;
            set
            {
                if (_passStatusAutoFillValue == value)
                    return;

                _passStatusAutoFillValue = value;
                OnPropertyChanged();
            }
        }

        private AutoFillSetup _failStatusAutoFillSetup;

        public AutoFillSetup FailStatusAutoFillSetup
        {
            get => _failStatusAutoFillSetup;
            set
            {
                if (_failStatusAutoFillSetup == value)
                    return;

                _failStatusAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _failStatusAutoFillValue;

        public AutoFillValue FailStatusAutoFillValue
        {
            get => _failStatusAutoFillValue;
            set
            {
                if (_failStatusAutoFillValue == value)
                    return;

                _failStatusAutoFillValue = value;
                OnPropertyChanged();
            }
        }

        private string _fixStatusText;

        public string FixStatusText
        {
            get => _fixStatusText;
            set
            {
                if (_fixStatusText == value) return;
                _fixStatusText = value;
                OnPropertyChanged();
            }
        }

        private string _passStatusText;

        public string PassStatusText
        {
            get => _passStatusText;
            set
            {
                if (_passStatusText == value) return;
                _passStatusText = value;
                OnPropertyChanged();
            }
        }

        private string _failStatusText;

        public string FailStatusText
        {
            get => _failStatusText;
            set
            {
                if (_failStatusText == value) return;
                _failStatusText = value;
                OnPropertyChanged();
            }
        }

        private LookupDefinition<UserLookup, User> _userLookupDefinition;

        public LookupDefinition<UserLookup, User> UserLookupDefinition
        {
            get => _userLookupDefinition;
            set
            {
                if (_userLookupDefinition == value) return;

                _userLookupDefinition = value;
                OnPropertyChanged();
            }
        }

        private string? _notes;

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

        private string? _ftpAddress;

        public string? FtpAddress
        {
            get => _ftpAddress;
            set
            {
                if (_ftpAddress == value) return;
                _ftpAddress = value;
                OnPropertyChanged();
            }
        }

        private string? _ftpUserName;

        public string? FtpUserName
        {
            get => _ftpUserName;
            set
            {
                if (_ftpUserName == value) return;
                _ftpUserName = value;
                OnPropertyChanged();
            }
        }

        private int _releaseLevel;

        public int ReleaseLevel
        {
            get => _releaseLevel;
            set
            {
                if (_releaseLevel == value) return;

                _releaseLevel = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public new IDepartmentView View { get; private set; }

        public RelayCommand AddModifyUserLookupCommand { get; set; }

        public DepartmentMaintenanceViewModel()
        {
            AddModifyUserLookupCommand = new RelayCommand(AddViewUser);
            FixStatusAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.ErrorFixStatusId));
            PassStatusAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.ErrorPassStatusId));
            FailStatusAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.ErrorFailStatusId));
            UserLookupDefinition = AppGlobals.LookupContext.UserLookup.Clone();
            RegisterLookup(UserLookupDefinition);
        }

        protected override void Initialize()
        {
            if (base.View is IDepartmentView view)
            {
                View = view;
            }
            else
            {
                throw new Exception("Invalid View");
            }
            
            base.Initialize();
        }

        protected override void PopulatePrimaryKeyControls(Department newEntity, PrimaryKeyValue primaryKeyValue)
        {
            Id = newEntity.Id;
        }

        protected override void LoadFromEntity(Department entity)
        {
            FixStatusAutoFillValue = entity.ErrorFixStatus.GetAutoFillValue();
            PassStatusAutoFillValue = entity.ErrorPassStatus.GetAutoFillValue();
            FailStatusAutoFillValue = entity.ErrorFailStatus.GetAutoFillValue();
            FixStatusText = entity.FixText;
            PassStatusText = entity.PassText;
            FailStatusText = entity.FailText;
            Notes = entity.Notes;
            FtpAddress = entity.FtpAddress;
            FtpUserName = entity.FtpUsername;
            if (!entity.FtpPassword.IsNullOrEmpty())
            {
                View.FtpPassword = entity.FtpPassword.Decrypt();
            }
            ReleaseLevel = entity.ReleaseLevel;
        }

        protected override Department GetEntityData()
        {
            var result = new Department
            {
                Id = Id,
                Description = KeyAutoFillValue.Text,
                FixText = FixStatusText,
                PassText = PassStatusText,
                FailText = FailStatusText,
                Notes = Notes,
                FtpAddress = FtpAddress,
                FtpUsername = FtpUserName,
                ReleaseLevel = ReleaseLevel,
                ErrorFixStatusId = FixStatusAutoFillValue.GetEntity<ErrorStatus>().Id,
                ErrorPassStatusId = PassStatusAutoFillValue.GetEntity<ErrorStatus>().Id,
                ErrorFailStatusId = FailStatusAutoFillValue.GetEntity<ErrorStatus>().Id
            };

            if (!View.FtpPassword.IsNullOrEmpty())
            {
                result.FtpPassword = View.FtpPassword.Encrypt();
            }

            if (result.ErrorFixStatusId == 0)
            {
                result.ErrorFixStatusId = null;
            }

            if (result.ErrorPassStatusId == 0)
            {
                result.ErrorPassStatusId = null;
            }

            if (result.ErrorFailStatusId == 0)
            {
                result.ErrorFailStatusId = null;
            }

            return result;
        }

        protected override void ClearData()
        {
            Id = 0;
            FixStatusAutoFillValue = null;
            PassStatusAutoFillValue = null;
            FailStatusAutoFillValue = null;
            FixStatusText = PassStatusText = FailStatusText = string.Empty;
            FtpAddress = FtpUserName = null;
            View.FtpPassword = string.Empty;
            Notes = null;
            ReleaseLevel = 0;
        }

        private void AddViewUser()
        {
            if (ExecuteAddModifyCommand() == DbMaintenanceResults.Success)
                UserLookupDefinition.SetCommand(GetLookupCommand(LookupCommands.AddModify));

        }
    }
}
