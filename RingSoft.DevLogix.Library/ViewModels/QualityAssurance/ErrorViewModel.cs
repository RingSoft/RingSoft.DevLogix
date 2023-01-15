using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RingSoft.App.Library;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model;

namespace RingSoft.DevLogix.Library.ViewModels.QualityAssurance
{
    public class ErrorViewModel :AppDbMaintenanceViewModel<Error>
    {
        public override TableDefinition<Error> TableDefinition => AppGlobals.LookupContext.Errors;

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

        private DateTime _errorDate;

        public DateTime ErrorDate
        {
            get => _errorDate;
            set
            {
                if (_errorDate == value)
                {
                    return;
                }
                _errorDate = value;
                OnPropertyChanged();
            }
        }

        private AutoFillSetup _statusAutoFillSetup;

        public AutoFillSetup StatusAutoFillSetup
        {
            get => _statusAutoFillSetup;
            set
            {
                if (_statusAutoFillSetup == value)
                {
                    return;
                }
                _statusAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _statusAutoFillValue;

        public AutoFillValue StatusAutoFillValue
        {
            get => _statusAutoFillValue;
            set
            {
                if (_statusAutoFillValue == value)
                {
                    return;
                }
                _statusAutoFillValue = value;
                OnPropertyChanged();
            }
        }

        private AutoFillSetup _productAutoFillSetup;

        public AutoFillSetup ProductAutoFillSetup
        {
            get => _productAutoFillSetup;
            set
            {
                if (_productAutoFillSetup == value)
                {
                    return;
                }
                _productAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _productAutoFillValue;

        public AutoFillValue ProductAutoFillValue
        {
            get => _productAutoFillValue;
            set
            {
                if (_productAutoFillValue == value)
                {
                    return;
                }
                _productAutoFillValue = value;
                SetProductVersionFilter();
                OnPropertyChanged();
            }
        }

        private AutoFillSetup _priorityAutoFillSetup;

        public AutoFillSetup PriorityAutoFillSetup
        {
            get => _priorityAutoFillSetup;
            set
            {
                if (_priorityAutoFillSetup == value)
                {
                    return;
                }
                _priorityAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _priorityAutoFillValue;

        public AutoFillValue PriorityAutoFillValue
        {
            get => _priorityAutoFillValue;
            set
            {
                if (_priorityAutoFillValue == value)
                {
                    return;
                }
                _priorityAutoFillValue = value;
                OnPropertyChanged();
            }
        }

        private AutoFillSetup _foundUserAutoFillSetup;

        public AutoFillSetup FoundUserAutoFillSetup
        {
            get => _foundUserAutoFillSetup;
            set
            {
                if (_foundUserAutoFillSetup == value)
                {
                    return;
                }
                _foundUserAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _foundUserAutoFillValue;

        public AutoFillValue FoundUserAutoFillValue
        {
            get => _foundUserAutoFillValue;
            set
            {
                if (_foundUserAutoFillValue == value)
                {
                    return;
                }
                _foundUserAutoFillValue = value;
                OnPropertyChanged();
            }
        }


        private AutoFillSetup _foundVersionAutoFillSetup;

        public AutoFillSetup FoundVersionAutoFillSetup
        {
            get => _foundVersionAutoFillSetup;
            set
            {
                if (_foundVersionAutoFillSetup == value)
                {
                    return;
                }
                _foundVersionAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillSetup _fixedUserAutoFillSetup;

        public AutoFillSetup FixedUserAutoFillSetup
        {
            get => _fixedUserAutoFillSetup;
            set
            {
                if (_fixedUserAutoFillSetup == value)
                {
                    return;
                }
                _fixedUserAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _foundVersionAutoFillValue;

        public AutoFillValue FoundVersionAutoFillValue
        {
            get => _foundVersionAutoFillValue;
            set
            {
                if (_foundVersionAutoFillValue == value)
                {
                    return;
                }
                _foundVersionAutoFillValue = value;
                OnPropertyChanged();
            }
        }

        private AutoFillSetup _fixedVersionAutoFillSetup;

        public AutoFillSetup FixedVersionAutoFillSetup
        {
            get => _fixedVersionAutoFillSetup;
            set
            {
                if (_fixedVersionAutoFillSetup == value)
                {
                    return;
                }
                _fixedVersionAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _fixedVersionAutoFillValue;

        public AutoFillValue FixedVersionAutoFillValue
        {
            get => _fixedVersionAutoFillValue;
            set
            {
                if (_fixedVersionAutoFillValue == value)
                {
                    return;
                }
                _fixedVersionAutoFillValue = value;
                OnPropertyChanged();
            }
        }

        private AutoFillSetup _assignedDeveloperAutoFillSetup;

        public AutoFillSetup AssignedDeveloperAutoFillSetup
        {
            get => _assignedDeveloperAutoFillSetup;
            set
            {
                if (_assignedDeveloperAutoFillSetup == value)
                {
                    return;
                }
                _assignedDeveloperAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _assignedDeveloperAutoFillValue;

        public AutoFillValue AssignedDeveloperAutoFillValue
        {
            get => _assignedDeveloperAutoFillValue;
            set
            {
                if (_assignedDeveloperAutoFillValue == value)
                {
                    return;
                }
                _assignedDeveloperAutoFillValue = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _fixedDate;

        public DateTime? FixedDate
        {
            get => _fixedDate;
            set
            {
                if (_fixedDate == value)
                {
                    return;
                }
                _fixedDate = value;
                OnPropertyChanged();
            }
        }

        private AutoFillSetup _assignedQualityAssuranceAutoFillSetup;

        public AutoFillSetup AssignedQualityAssuranceAutoFillSetup
        {
            get => _assignedQualityAssuranceAutoFillSetup;
            set
            {
                if (_assignedQualityAssuranceAutoFillSetup == value)
                {
                    return;
                }
                _assignedQualityAssuranceAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _assignedQualityAssuranceAutoFillValue;

        public AutoFillValue AssignedQualityAssuranceAutoFillValue
        {
            get => _assignedQualityAssuranceAutoFillValue;
            set
            {
                if (_assignedQualityAssuranceAutoFillValue == value)
                {
                    return;
                }
                _assignedQualityAssuranceAutoFillValue = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _passedDate;

        public DateTime? PassedDate
        {
            get => _passedDate;
            set
            {
                if (_passedDate == value)
                {
                    return;
                }
                _passedDate = value;
                OnPropertyChanged();
            }
        }

        private string _description;

        public string Description
        {
            get => _description;
            set
            {
                if (_description == value)
                    return;

                _description = value;
                OnPropertyChanged();
            }
        }

        private string _resolution;

        public string Resolution
        {
            get => _resolution;
            set
            {
                if (_resolution == value)
                    return;

                _resolution = value;
                OnPropertyChanged();
            }
        }

        private ErrorDeveloperManager _developerManager;

        public ErrorDeveloperManager DeveloperManager
        {
            get => _developerManager;
            set
            {
                if (_developerManager == value)
                {
                    return;
                }
                _developerManager = value;
                OnPropertyChanged();
            }
        }


        private IDbContext _makeErrorIdContext;
        private bool _makeErrorId;

        protected override void Initialize()
        {
            StatusAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.ErrorStatusId));
            ProductAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.ProductId));
            PriorityAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.ErrorPriorityId));
            FoundVersionAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.FoundVersionId));
            FixedVersionAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.FixedVersionId));
            FoundUserAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.FoundByUserId));

            AssignedDeveloperAutoFillSetup =
                new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.AssignedDeveloperId));
            AssignedQualityAssuranceAutoFillSetup =
                new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.AssignedTesterId));

            DeveloperManager = new ErrorDeveloperManager(this);
            TablesToDelete.Add(AppGlobals.LookupContext.ErrorDevelopers);

            base.Initialize();
        }

        private void SetProductVersionFilter()
        {
            FoundVersionAutoFillSetup.LookupDefinition.FilterDefinition.ClearFixedFilters();
            FixedVersionAutoFillSetup.LookupDefinition.FilterDefinition.ClearFixedFilters();

            if (ProductAutoFillValue.IsValid())
            {
                var product =
                    AppGlobals.LookupContext.Products.GetEntityFromPrimaryKeyValue(ProductAutoFillValue
                        .PrimaryKeyValue);

                FoundVersionAutoFillSetup.LookupDefinition.FilterDefinition.AddFixedFilter(
                    AppGlobals.LookupContext.ProductVersions.GetFieldDefinition(p => p.ProductId), Conditions.Equals,
                    product.Id);

                FixedVersionAutoFillSetup.LookupDefinition.FilterDefinition.AddFixedFilter(
                    AppGlobals.LookupContext.ProductVersions.GetFieldDefinition(p => p.ProductId), Conditions.Equals,
                    product.Id);

                if (MaintenanceMode == DbMaintenanceModes.AddMode)
                {
                    FoundVersionAutoFillValue = GetVersionForUser();
                }
            }
        }

        private AutoFillValue GetVersionForUser()
        {
            AutoFillValue result = null;
            if (ProductAutoFillValue.IsValid())
            {
                var product =
                    AppGlobals.LookupContext.Products.GetEntityFromPrimaryKeyValue(ProductAutoFillValue
                        .PrimaryKeyValue);

                var context = AppGlobals.DataRepository.GetDataContext();
                var productTable = context.GetTable<ProductVersionDepartment>();
                var productVersions = productTable.Include(p => p.ProductVersion)
                    .OrderByDescending(p => p.ReleaseDateTime)
                    .Where(p => p.ProductVersion.ProductId == product.Id);

                if (productVersions != null)
                {
                    var productVersion = productVersions.FirstOrDefault();

                    if (productVersion != null)
                    {
                        if (AppGlobals.LoggedInUser != null)
                        {
                            var departmentId = AppGlobals.LoggedInUser.DepartmentId;
                            productVersion = productVersions.FirstOrDefault(p => p.DepartmentId == departmentId);
                        }

                        if (productVersion == null)
                        {
                            productVersion = productVersions.FirstOrDefault();
                        }

                        result = FoundVersionAutoFillSetup.GetAutoFillValueForIdValue(productVersion.VersionId);
                    }
                }
            }

            return result;
        }

        protected override Error PopulatePrimaryKeyControls(Error newEntity, PrimaryKeyValue primaryKeyValue)
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            var errorTable = context.GetTable<Error>();
            var result = errorTable.Include(p => p.Developers)
                .FirstOrDefault(p => p.Id == newEntity.Id);
            if (result != null)
            {
                Id = result.Id;
                if (_makeErrorIdContext != null && _makeErrorId)
                {
                    var errorId = $"E-{result.Id}";
                    result.ErrorId = errorId;
                    _makeErrorIdContext.SaveEntity(result, "Updating ErrorId");
                }
                _makeErrorIdContext = null;
                _makeErrorId = false;
                KeyAutoFillValue = KeyAutoFillSetup.GetAutoFillValueForIdValue(Id);
            }

            return result;
        }

        protected override void LoadFromEntity(Error entity)
        {
            ErrorDate = entity.ErrorDate;
            StatusAutoFillValue = StatusAutoFillSetup.GetAutoFillValueForIdValue(entity.ErrorStatusId);
            ProductAutoFillValue = ProductAutoFillSetup.GetAutoFillValueForIdValue(entity.ProductId);
            PriorityAutoFillValue = PriorityAutoFillSetup.GetAutoFillValueForIdValue(entity.ErrorPriorityId);
            FoundVersionAutoFillValue = FoundVersionAutoFillSetup.GetAutoFillValueForIdValue(entity.FoundVersionId);
            FixedVersionAutoFillValue = FixedVersionAutoFillSetup.GetAutoFillValueForIdValue(entity.FixedVersionId);
            FoundUserAutoFillValue = FoundUserAutoFillSetup.GetAutoFillValueForIdValue(entity.FoundByUserId);
            AssignedDeveloperAutoFillValue =
                AssignedDeveloperAutoFillSetup.GetAutoFillValueForIdValue(entity.AssignedDeveloperId);
            FixedDate = entity.FixedDate;
            AssignedQualityAssuranceAutoFillValue =
                AssignedQualityAssuranceAutoFillSetup.GetAutoFillValueForIdValue(entity.AssignedTesterId);
            PassedDate = entity.PassedDate;
            Description = entity.Description;
            Resolution = entity.Resolution;
            DeveloperManager.LoadGrid(entity.Developers);
        }

        protected override Error GetEntityData()
        {
            var result = new Error
            {
                Id = Id,
                ErrorDate = ErrorDate,
                ErrorStatusId = StatusAutoFillValue.GetEntity(AppGlobals.LookupContext.ErrorStatuses).Id,
                ProductId = ProductAutoFillValue.GetEntity(AppGlobals.LookupContext.Products).Id,
                ErrorPriorityId = PriorityAutoFillValue.GetEntity(AppGlobals.LookupContext.ErrorPriorities).Id,
                FoundByUserId = FoundUserAutoFillValue.GetEntity(AppGlobals.LookupContext.Users).Id,
                FoundVersionId = FoundVersionAutoFillValue.GetEntity(AppGlobals.LookupContext.ProductVersions).Id,
                FixedVersionId = FixedVersionAutoFillValue.GetEntity(AppGlobals.LookupContext.ProductVersions).Id,
                AssignedDeveloperId = AssignedDeveloperAutoFillValue.GetEntity(AppGlobals.LookupContext.Users).Id,
                FixedDate = FixedDate,
                AssignedTesterId = AssignedQualityAssuranceAutoFillValue.GetEntity(AppGlobals.LookupContext.Users).Id,
                PassedDate = PassedDate,
                Description = Description,
                Resolution = Resolution,
            };

            if (MaintenanceMode == DbMaintenanceModes.AddMode && KeyAutoFillValue.Text.IsNullOrEmpty())
            {
                result.ErrorId = Guid.NewGuid().ToString();
                KeyAutoFillValue = new AutoFillValue(new PrimaryKeyValue(TableDefinition), result.ErrorId);
                _makeErrorId = true;
            }
            else
            {
                result.ErrorId = KeyAutoFillValue.Text;
            }

            if (result.FoundByUserId == 0)
            {
                result.FoundByUserId = null;
                if (result.FoundByUser == null && AppGlobals.LoggedInUser != null)
                {
                    var message = "Found by user cannot be empty. Do you wish to set the found by user to yourself?";
                    var caption = "Validation Check";
                    if (ControlsGlobals.UserInterface.ShowYesNoMessageBox(message, caption) ==
                        MessageBoxButtonsResult.Yes)
                    {
                        FoundUserAutoFillValue = FoundUserAutoFillSetup.GetAutoFillValueForIdValue(AppGlobals.LoggedInUser.Id);
                        result.FoundByUserId = AppGlobals.LoggedInUser.Id;
                    }

                }
            }

            if (result.FixedVersionId == 0)
            {
                result.FixedVersionId = null;
            }

            if (result.AssignedDeveloperId == 0)
            {
                result.AssignedDeveloperId = null;
            }

            if (result.AssignedTesterId == 0)
            {
                result.AssignedTesterId = null;
            }

            return result;
        }

        protected override void ClearData()
        {
            Id = 0;
            ErrorDate = DateTime.Now;
            StatusAutoFillValue = ProductAutoFillValue = PriorityAutoFillValue = FoundVersionAutoFillValue =
                FixedVersionAutoFillValue = AssignedDeveloperAutoFillValue = AssignedQualityAssuranceAutoFillValue =  null;
            if (AppGlobals.LoggedInUser != null)
            {
                FoundUserAutoFillValue = FoundUserAutoFillSetup.GetAutoFillValueForIdValue(AppGlobals.LoggedInUser.Id);
            }

            FixedDate = PassedDate = null;
            Description = Resolution = string.Empty;
            DeveloperManager.SetupForNewRecord();

        }

        protected override bool SaveEntity(Error entity)
        {
            var result = false;
            var context = AppGlobals.DataRepository.GetDataContext();
            if (context != null)
            {
                result = context.SaveEntity(entity, "Saving Error");

                if (result)
                {
                    var developerQuery = AppGlobals.DataRepository.GetDataContext().GetTable<ErrorDeveloper>();
                    var developers = developerQuery.Where(p => p.ErrorId == Id).ToList();
                    context.RemoveRange(developers);
                    developers = DeveloperManager.GetEntityList();
                    if (developers != null)
                    {
                        foreach (var developer in developers)
                        {
                            developer.ErrorId = entity.Id;
                        }

                        context.AddRange(developers);
                    }

                    result = context.Commit("Saving ErrorDevelopers");
                }

            }

            if (MaintenanceMode == DbMaintenanceModes.AddMode)
            {
                _makeErrorIdContext = context as IDbContext;
            }

            return result;
        }

        protected override bool DeleteEntity()
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            if (context != null)
            {
                var developerQuery = AppGlobals.DataRepository.GetDataContext().GetTable<ErrorDeveloper>();
                var developers = developerQuery.Where(p => p.ErrorId == Id).ToList();
                context.RemoveRange(developers);

                var entity = context.GetTable<Error>().FirstOrDefault(p => p.Id == Id);
                return context.DeleteEntity(entity, "Deleting Error");
            }

            return false;

        }
    }
}
