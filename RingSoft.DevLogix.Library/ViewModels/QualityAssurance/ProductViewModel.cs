using System;
using System.Collections.Generic;
using RingSoft.DbLookup;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DevLogix.DataAccess.Model;
using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DevLogix.DataAccess.LookupModel;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.LookupModel.QualityAssurance;
using RingSoft.DevLogix.DataAccess.Model.QualityAssurance;

namespace RingSoft.DevLogix.Library.ViewModels.QualityAssurance
{
    public interface IProductView
    {
        bool UpdateVersions(ProductViewModel viewModel);

        string GetInstallerName();

        string GetArchivePath();

        void SetViewToVersions();
    }
    public class ProductViewModel : DevLogixDbMaintenanceViewModel<Product>
    {
        public override TableDefinition<Product> TableDefinition => AppGlobals.LookupContext.Products;

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

        private LookupDefinition<ProductVersionLookup, ProductVersion> _productVersionLookupDefinition;

        public LookupDefinition<ProductVersionLookup, ProductVersion> ProductVersionLookupDefinition
        {
            get => _productVersionLookupDefinition;
            set
            {
                if (_productVersionLookupDefinition == value)
                    return;

                _productVersionLookupDefinition = value;
                OnPropertyChanged();
            }
        }

        private LookupCommand _productVersionLookupCommand;

        public LookupCommand ProductVersionLookupCommand
        {
            get => _productVersionLookupCommand;
            set
            {
                if (_productVersionLookupCommand == value)
                    return;

                _productVersionLookupCommand = value;
                OnPropertyChanged(null, false);
            }
        }

        private AutoFillSetup _departmentFilterAutoFillSetup;

        public AutoFillSetup DepartmentFilterAutoFillSetup
        {
            get => _departmentFilterAutoFillSetup;
            set
            {
                if (_departmentFilterAutoFillSetup == value)
                {
                    return;
                }
                _departmentFilterAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _departmentFilterAutoFillValue;

        public AutoFillValue DepartmentFilterAutoFillValue
        {
            get => _departmentFilterAutoFillValue;
            set
            {
                if (_departmentFilterAutoFillValue == value)
                {
                    return;
                }
                _departmentFilterAutoFillValue = value;
                OnPropertyChanged(null, false);
                FilterVersions();
            }
        }

        private string? _installerFileName;

        public string? InstallerFileName
        {
            get => _installerFileName;
            set
            {
                if (_installerFileName == value)
                    return;

                _installerFileName = value;
                OnPropertyChanged();
            }
        }

        private string? _archivePath;

        public string? ArchivePath
        {
            get => _archivePath;
            set
            {
                if (_archivePath == value)
                    return;

                _archivePath = value;
                OnPropertyChanged();
            }
        }

        private string? _appGuid;

        public string? AppGuid
        {
            get => _appGuid;
            set
            {
                if (_appGuid == value) return;
                _appGuid = value;
                OnPropertyChanged();
            }
        }

        private AutoFillSetup _createDepartmentAutoFillSetup;

        public AutoFillSetup CreateDepartmentAutoFillSetup
        {
            get => _createDepartmentAutoFillSetup;
            set
            {
                if (_createDepartmentAutoFillSetup == value)
                {
                    return;
                }
                _createDepartmentAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _createDepartmentAutoFillValue;

        public AutoFillValue CreateDepartmentAutoFillValue
        {
            get => _createDepartmentAutoFillValue;
            set
            {
                if (_createDepartmentAutoFillValue == value)
                {
                    return;
                }
                _createDepartmentAutoFillValue = value;
                OnPropertyChanged(null, _createDepartmentAutoFillSetup.SetDirty);
            }
        }

        private AutoFillSetup _archiveDepartmentAutoFillSetup;

        public AutoFillSetup ArchiveDepartmentAutoFillSetup
        {
            get => _archiveDepartmentAutoFillSetup;
            set
            {
                if (_archiveDepartmentAutoFillSetup == value)
                {
                    return;
                }
                _archiveDepartmentAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _archiveDepartmentAutoFillValue;

        public AutoFillValue ArchiveDepartmentAutoFillValue
        {
            get => _archiveDepartmentAutoFillValue;
            set
            {
                if (_archiveDepartmentAutoFillValue == value)
                {
                    return;
                }
                _archiveDepartmentAutoFillValue = value;
                OnPropertyChanged(null, _archiveDepartmentAutoFillSetup.SetDirty);
            }
        }

        private LookupDefinition<TestingOutlineLookup, TestingOutline> _testingOutlineLookup;

        public LookupDefinition<TestingOutlineLookup, TestingOutline> TestingOutlineLookup
        {
            get => _testingOutlineLookup;
            set
            {
                if (_testingOutlineLookup == value)
                    return;

                _testingOutlineLookup = value;
                OnPropertyChanged();
            }
        }

        private LookupCommand _testingOutlineLookupCommand;

        public LookupCommand TestingOutlineLookupCommand
        {
            get => _testingOutlineLookupCommand;
            set
            {
                if (_testingOutlineLookupCommand == value)
                    return;

                _testingOutlineLookupCommand = value;
                OnPropertyChanged(null, false);
            }
        }

        private decimal? _price;

        public decimal? Price
        {
            get => _price;
            set
            {
                if (_price == value)
                    return;

                _price = value;
                OnPropertyChanged();
            }
        }


        public RelayCommand VersionsAddModifyCommand { get; set; }

        public RelayCommand TestOutlinesAddModifyCommand { get; set; }

        public RelayCommand UpdateVersionsCommand { get; set; }

        public RelayCommand InstallerCommand { get; set; }

        public RelayCommand ArchivePathCommand { get; set; }

        public RelayCommand GenerateGuidCommand { get; set; }

        public new IProductView View { get; set; }

        public ProductViewModel()
        {
            VersionsAddModifyCommand = new RelayCommand(OnVersionsAddModify);
            TestOutlinesAddModifyCommand = new RelayCommand(OnTestOutlineAddModify);
            UpdateVersionsCommand = new RelayCommand(UpdateVersions);
            InstallerCommand = new RelayCommand(() =>
            {
                InstallerFileName = View.GetInstallerName();
            });
            ArchivePathCommand = new RelayCommand(() =>
            {
                ArchivePath = View.GetArchivePath();
            });
            GenerateGuidCommand = new RelayCommand(() =>
            {
                AppGuid = Guid.NewGuid().ToString();
            });

            var testOutlineLookup = new LookupDefinition<TestingOutlineLookup, TestingOutline>(AppGlobals.LookupContext.TestingOutlines);
            testOutlineLookup.AddVisibleColumnDefinition(p => p.Name
                , "Name"
                , p => p.Name, 50);
            testOutlineLookup.Include(p => p.AssignedToUser)
                .AddVisibleColumnDefinition(p => p.AssignedTo
                    , "Assigned To"
                    , p => p.Name, 50);
            TestingOutlineLookup = testOutlineLookup;
        }
        protected override void Initialize()
        {
            if (base.View is IProductView productView)
            {
                View = productView;
            }

            var lookupDefinition = AppGlobals.LookupContext.ProductVersionLookup.Clone();
            //lookupDefinition.InitialSortColumnDefinition = lookupDefinition.VisibleColumns[1];

            ProductVersionLookupDefinition = lookupDefinition;

            CreateDepartmentAutoFillSetup =
                new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.CreateDepartmentId));
            ArchiveDepartmentAutoFillSetup =
                new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.ArchiveDepartmentId));

            DepartmentFilterAutoFillSetup = new AutoFillSetup(AppGlobals.LookupContext.DepartmentLookup);
            DepartmentFilterAutoFillSetup.AllowLookupAdd = false;
            base.Initialize();
        }

        private void FilterVersions()
        {
            var lookupDefinition = ProductVersionLookupDefinition;
            lookupDefinition.FilterDefinition.ClearFixedFilters();
            if (Id > 0)
            {
                lookupDefinition.FilterDefinition.AddFixedFilter("ProductId", Conditions.Equals,
                    Id.ToString(), "ProductId");

                if (DepartmentFilterAutoFillValue.IsValid())
                {
                    var department =
                        AppGlobals.LookupContext.Departments.GetEntityFromPrimaryKeyValue(DepartmentFilterAutoFillValue
                            .PrimaryKeyValue);

                    var tableDefinition = AppGlobals.LookupContext.ProductVersionDepartments;
                    var field = tableDefinition.GetFieldDefinition(p => p.VersionId).FieldName;
                    var query = new SelectQuery(tableDefinition.TableName);
                    query.AddSelectColumn(field);
                    query.AddWhereItem(tableDefinition.GetFieldDefinition(p => p.DepartmentId).FieldName,
                        Conditions.Equals, department.Id);

                    var versionsTableDefinition = AppGlobals.LookupContext.ProductVersions;
                    field = versionsTableDefinition.GetFieldDefinition(p => p.Id).FieldName;
                    field = AppGlobals.LookupContext.DataProcessor.SqlGenerator.FormatSqlObject(field);
                    field =
                        $"{AppGlobals.LookupContext.DataProcessor.SqlGenerator.FormatSqlObject(versionsTableDefinition.TableName)}.{field}";
                    var sql = AppGlobals.LookupContext.DataProcessor.SqlGenerator.GenerateSelectStatement(query);
                    sql = $"{field} IN ({sql})";
                    lookupDefinition.FilterDefinition.AddFixedFilter("Department", null, "", sql);
                }
            }

            ProductVersionLookupCommand = GetLookupCommand(LookupCommands.Refresh);
        }


        protected override Product PopulatePrimaryKeyControls(Product newEntity, PrimaryKeyValue primaryKeyValue)
        {
            ProductVersionLookupDefinition.FilterDefinition.ClearFixedFilters();
            var query = AppGlobals.DataRepository.GetDataContext().GetTable<Product>();
            var result = query.FirstOrDefault(p => p.Id == newEntity.Id);
            if (result != null)
            {
                Id = result.Id;
                KeyAutoFillValue = AppGlobals.LookupContext.OnAutoFillTextRequest(TableDefinition, Id.ToString());
                FilterVersions();
                UpdateVersionsCommand.IsEnabled = true;
            }

            TestingOutlineLookup.FilterDefinition.ClearFixedFilters();
            TestingOutlineLookup.FilterDefinition.AddFixedFilter(p => p.ProductId, Conditions.Equals, Id);
            TestingOutlineLookupCommand = GetLookupCommand(LookupCommands.Refresh, primaryKeyValue);

            return result;
        }

        protected override void LoadFromEntity(Product entity)
        {
            Notes = entity.Notes;
            InstallerFileName = entity.InstallerFileName;
            ArchivePath = entity.ArchivePath;
            AppGuid = entity.AppGuid;
            CreateDepartmentAutoFillValue =
                CreateDepartmentAutoFillSetup.GetAutoFillValueForIdValue(entity.CreateDepartmentId);
            ArchiveDepartmentAutoFillValue =
                ArchiveDepartmentAutoFillSetup.GetAutoFillValueForIdValue(entity.ArchiveDepartmentId);
            Price = entity.Price;
        }

        protected override Product GetEntityData()
        {
            var result = new Product()
            {
                Id = Id,
                Description = KeyAutoFillValue.Text,
                Notes = Notes,
                InstallerFileName = InstallerFileName,
                ArchivePath = ArchivePath,
                AppGuid = AppGuid,
                CreateDepartmentId = CreateDepartmentAutoFillValue.GetEntity(AppGlobals.LookupContext.Departments).Id,
                ArchiveDepartmentId = ArchiveDepartmentAutoFillValue.GetEntity(AppGlobals.LookupContext.Departments).Id,
                Price = Price,
            };

            if (result.ArchiveDepartmentId == 0)
            {
                result.ArchiveDepartmentId = null;
            }

            return result;
        }

        protected override void ClearData()
        {
            Id = 0;
            Notes = null;
            ProductVersionLookupCommand = GetLookupCommand(LookupCommands.Clear);
            UpdateVersionsCommand.IsEnabled = false;
            InstallerFileName = ArchivePath = AppGuid = null;
            CreateDepartmentAutoFillValue = ArchiveDepartmentAutoFillValue = null;
            TestingOutlineLookupCommand = GetLookupCommand(LookupCommands.Clear);
            Price = null;
        }

        protected override bool SaveEntity(Product entity)
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            if (context != null)
            {
                var result = context.SaveEntity(entity, $"Saving Product '{entity.Description}'");
                if (result && MaintenanceMode == DbMaintenanceModes.AddMode)
                {
                    return CreateNewVersion(context, entity);
                }
                return result;
            }
            return false;

        }

        private bool CreateNewVersion(DataAccess.IDbContext context, Product product)
        {
            var newVersion = new ProductVersion
            {
                Description = "00.85.1",
                ProductId = product.Id,
            };
            if (context.SaveEntity(newVersion, "Creating new version"))
            {
                var versionDepartment = new ProductVersionDepartment
                {
                    VersionId = newVersion.Id,
                    DepartmentId = product.CreateDepartmentId,
                    ReleaseDateTime = DateTime.Now.ToUniversalTime(),
                };
                var versionDepartments = new List<ProductVersionDepartment>();
                versionDepartments.Add(versionDepartment);
                context.AddRange(versionDepartments);
                if (context.Commit("Adding new Version Department"))
                {
                    View.SetViewToVersions();
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        protected override bool DeleteEntity()
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            var query = context.GetTable<Product>();
            if (query != null)
            {
                var entity = query.FirstOrDefault(p => p.Id == Id);

                if (entity != null)
                {
                    return context.DeleteEntity(entity, $"Deleting Product Version '{entity.Description}'");
                }
            }
            return false;

        }

        private void OnVersionsAddModify()
        {
            if (ExecuteAddModifyCommand() == DbMaintenanceResults.Success)
                ProductVersionLookupCommand = GetLookupCommand(LookupCommands.AddModify);

        }

        private void OnTestOutlineAddModify()
        {
            if (ExecuteAddModifyCommand() == DbMaintenanceResults.Success)
                TestingOutlineLookupCommand = GetLookupCommand(LookupCommands.AddModify);
        }

        private void UpdateVersions()
        {
            if (View.UpdateVersions(this))
            {
                FilterVersions();
            }
        }

        protected override void OnRecordDirtyChanged(bool newValue)
        {
            if (newValue)
            {

            }
            base.OnRecordDirtyChanged(newValue);
        }
    }
}
