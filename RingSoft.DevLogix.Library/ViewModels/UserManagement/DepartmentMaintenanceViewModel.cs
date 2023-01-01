using System;
using System.Linq;
using System.Net;
using RingSoft.App.Interop;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.LookupModel;
using RingSoft.DevLogix.DataAccess.Model;

namespace RingSoft.DevLogix.Library.ViewModels.UserManagement
{
    public interface IDepartmentView : IDbMaintenanceView
    {
        string FtpPassword { get; set; }
    }
    public class DepartmentMaintenanceViewModel : DevLogixDbMaintenanceViewModel<Department>
    {
        public override TableDefinition<Department> TableDefinition => AppGlobals.LookupContext.Departments;

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

        private LookupCommand _userLookupCommand;

        public LookupCommand UserLookupCommand
        {
            get => _userLookupCommand;
            set
            {
                if (_userLookupCommand == value) return;
                _userLookupCommand = value;
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

        public new IDepartmentView View { get; private set; }

        public RelayCommand AddModifyUserLookupCommand { get; set; }

        public DepartmentMaintenanceViewModel()
        {
            AddModifyUserLookupCommand = new RelayCommand(AddViewUser);
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
            
            FixStatusAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.ErrorFixStatusId));
            PassStatusAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.ErrorPassStatusId));
            FailStatusAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.ErrorFailStatusId));
            UserLookupDefinition = AppGlobals.LookupContext.UserLookup.Clone();

            base.Initialize();
        }

        protected override Department PopulatePrimaryKeyControls(Department newEntity, PrimaryKeyValue primaryKeyValue)
        {
            var query = AppGlobals.DataRepository.GetDataContext().GetTable<Department>();
            var result = query.FirstOrDefault(p => p.Id == newEntity.Id);
            if (result != null)
            {
                Id = result.Id;
                KeyAutoFillValue = AppGlobals.LookupContext.OnAutoFillTextRequest(TableDefinition, Id.ToString());
            }

            UserLookupDefinition.FilterDefinition.ClearFixedFilters();
            UserLookupDefinition.FilterDefinition.AddFixedFilter(p => p.DepartmentId, Conditions.Equals, Id);
            UserLookupCommand = GetLookupCommand(LookupCommands.Refresh, primaryKeyValue);

            return result;
        }

        protected override void LoadFromEntity(Department entity)
        {
            if (entity.ErrorFixStatusId != null)
            {
                FixStatusAutoFillValue =
                    AppGlobals.LookupContext.OnAutoFillTextRequest(AppGlobals.LookupContext.ErrorStatuses,
                        entity.ErrorFixStatusId.ToString());
            }

            if (entity.ErrorPassStatusId != null)
            {
                PassStatusAutoFillValue =
                    AppGlobals.LookupContext.OnAutoFillTextRequest(AppGlobals.LookupContext.ErrorStatuses,
                        entity.ErrorPassStatusId.ToString());
            }

            if (entity.ErrorFailStatusId != null)
            {
                FailStatusAutoFillValue =
                    AppGlobals.LookupContext.OnAutoFillTextRequest(AppGlobals.LookupContext.ErrorStatuses,
                        entity.ErrorFailStatusId.ToString());
            }


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
            };

            if (!View.FtpPassword.IsNullOrEmpty())
            {
                result.FtpPassword = View.FtpPassword.Encrypt();
            }

            if (FixStatusAutoFillValue.IsValid())
            {
                result.ErrorFixStatusId = AppGlobals.LookupContext.ErrorStatuses
                    .GetEntityFromPrimaryKeyValue(FixStatusAutoFillValue.PrimaryKeyValue).Id;
            }

            if (PassStatusAutoFillValue.IsValid())
            {
                result.ErrorPassStatusId = AppGlobals.LookupContext.ErrorStatuses
                    .GetEntityFromPrimaryKeyValue(PassStatusAutoFillValue.PrimaryKeyValue).Id;
            }

            if (FailStatusAutoFillValue.IsValid())
            {
                result.ErrorFailStatusId = AppGlobals.LookupContext.ErrorStatuses
                    .GetEntityFromPrimaryKeyValue(FailStatusAutoFillValue.PrimaryKeyValue).Id;
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
            UserLookupCommand = GetLookupCommand(LookupCommands.Clear);
            Notes = null;
        }

        protected override AutoFillValue GetAutoFillValueForNullableForeignKeyField(FieldDefinition fieldDefinition)
        {
            if (fieldDefinition == TableDefinition.GetFieldDefinition(p => p.ErrorFixStatusId))
            {
                return FixStatusAutoFillValue;
            }

            if (fieldDefinition == TableDefinition.GetFieldDefinition(p => p.ErrorPassStatusId))
            {
                return PassStatusAutoFillValue;
            }

            if (fieldDefinition == TableDefinition.GetFieldDefinition(p => p.ErrorFailStatusId))
            {
                return FailStatusAutoFillValue;
            }

            return base.GetAutoFillValueForNullableForeignKeyField(fieldDefinition);
        }

        protected override bool SaveEntity(Department entity)
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            if (context != null)
            {
                return context.SaveEntity(entity, $"Saving Department '{entity.Description}'");
            }
            return false;
        }

        protected override bool DeleteEntity()
        {
            var query = AppGlobals.DataRepository.GetDataContext().GetTable<Department>();
            if (query != null)
            {
                var department = query.FirstOrDefault(p => p.Id == Id);
                var context = AppGlobals.DataRepository.GetDataContext();
                if (context != null && department != null)
                {
                    return context.DeleteEntity(department, $"Deleting Department '{department.Description}'");
                }
            }
            return false;

        }

        private void AddViewUser()
        {
            if (ExecuteAddModifyCommand() == DbMaintenanceResults.Success)
                UserLookupCommand = GetLookupCommand(LookupCommands.AddModify);

        }
    }
}
