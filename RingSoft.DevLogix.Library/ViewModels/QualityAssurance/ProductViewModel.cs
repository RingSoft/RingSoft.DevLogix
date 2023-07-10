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
using RingSoft.App.Library;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using RingSoft.DevLogix.DataAccess.Model.CustomerManagement;

namespace RingSoft.DevLogix.Library.ViewModels.QualityAssurance
{
    public interface IProductView
    {
        bool UpdateVersions(ProductViewModel viewModel);

        string GetInstallerName();

        string GetArchivePath();

        void SetViewToVersions();

        void RefreshView();

        bool SetupRecalcFilter(LookupDefinitionBase lookup);

        string StartRecalcProcedure(LookupDefinitionBase lookup);

        void UpdateRecalcProcedure(int currentProduct, int totalProducts, string currentProductText);
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

        private double? _price;

        public double? Price
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

        private double _totalRevenue;

        public double TotalRevenue
        {
            get => _totalRevenue;
            set
            {
                if (_totalRevenue == value)
                    return;

                _totalRevenue = value;
                OnPropertyChanged(null, false);
            }
        }

        private double _totalCost;

        public double TotalCost
        {
            get => _totalCost;
            set
            {
                if (_totalCost == value)
                    return;

                _totalCost = value;
                OnPropertyChanged(null, false);
            }
        }

        private double _difference;

        public double Difference
        {
            get => _difference;
            set
            {
                if (_difference == value)
                    return;

                _difference = value;
                OnPropertyChanged(null, false);
            }
        }


        public RelayCommand VersionsAddModifyCommand { get; set; }

        public RelayCommand TestOutlinesAddModifyCommand { get; set; }

        public RelayCommand UpdateVersionsCommand { get; set; }

        public RelayCommand RecalcCommand { get; set; }

        public RelayCommand InstallerCommand { get; set; }

        public RelayCommand ArchivePathCommand { get; set; }

        public RelayCommand GenerateGuidCommand { get; set; }

        public new IProductView View { get; set; }

        public ProductViewModel()
        {
            VersionsAddModifyCommand = new RelayCommand(OnVersionsAddModify);
            TestOutlinesAddModifyCommand = new RelayCommand(OnTestOutlineAddModify);
            UpdateVersionsCommand = new RelayCommand(UpdateVersions);
            RecalcCommand = new RelayCommand(Recalc);
            InstallerCommand = new RelayCommand(() => { InstallerFileName = View.GetInstallerName(); });
            ArchivePathCommand = new RelayCommand(() => { ArchivePath = View.GetArchivePath(); });
            GenerateGuidCommand = new RelayCommand(() => { AppGuid = Guid.NewGuid().ToString(); });

            var testOutlineLookup =
                new LookupDefinition<TestingOutlineLookup, TestingOutline>(AppGlobals.LookupContext.TestingOutlines);
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
            lookupDefinition.FilterDefinition.AddFixedFilter(
                p => p.ProductId
                , Conditions.Equals
                , Id);

            if (Id > 0)
            {
                if (DepartmentFilterAutoFillValue.IsValid())
                {
                    var department = DepartmentFilterAutoFillValue.GetEntity<Department>();
                    if (department != null)
                    {
                        var context = AppGlobals.DataRepository.GetDataContext();
                        var table = context.GetTable<Department>();
                        department = table.FirstOrDefault(p => p.Id == department.Id);
                        if (department != null)
                        {
                            lookupDefinition.FilterDefinition
                                .Include(p => p.Department)
                                .AddFixedFilter(p => p.ReleaseLevel
                                    , Conditions.GreaterThanEquals
                                    , department.ReleaseLevel);
                        }
                    }
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
            TotalRevenue = entity.Revenue.GetValueOrDefault();
            TotalCost = entity.Cost.GetValueOrDefault();
            Difference = TotalRevenue - TotalCost;
            View.RefreshView();
        }

        protected override Product GetEntityData()
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<Product>();
            var existProduct = table
                .FirstOrDefault(p => p.Id == Id);
            if (existProduct != null)
            {
                TotalRevenue = existProduct.Revenue.GetValueOrDefault();
                TotalCost += existProduct.Cost.GetValueOrDefault();
            }

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
                Revenue = TotalRevenue,
                Cost = TotalCost,
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
            TotalRevenue = TotalCost = Difference = 0;
            View.RefreshView();
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
                VersionDate = DateTime.Now.ToUniversalTime(),
                DepartmentId = product.CreateDepartmentId,
            };
            if (context.SaveEntity(newVersion, "Creating new version"))
            {
                var versionDepartment = new ProductVersionDepartment
                {
                    VersionId = newVersion.Id,
                    DepartmentId = product.CreateDepartmentId,
                    ReleaseDateTime = newVersion.VersionDate.GetValueOrDefault(),
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

        private void Recalc()
        {
            var lookupFilter = ViewLookupDefinition.Clone();
            if (!View.SetupRecalcFilter(lookupFilter))
                return;
            var result = View.StartRecalcProcedure(lookupFilter);
            if (result.IsNullOrEmpty())
            {
                ControlsGlobals.UserInterface.ShowMessageBox(
                    "Recalculation Complete"
                    , "Recalculation Complete"
                    , RsMessageBoxIcons.Information);
            }
            else
            {
                ControlsGlobals.UserInterface.ShowMessageBox(
                    result
                    , "Product Recalculating"
                    , RsMessageBoxIcons.Error);
            }
        }

        public string StartRecalcProcedure(LookupDefinitionBase lookupToFilter, AppProcedure appProcedure)
        {
            var result = string.Empty;
            var lookupData = TableDefinition.LookupDefinition.GetLookupDataMaui(lookupToFilter, false);
            var context = AppGlobals.DataRepository.GetDataContext();
            DbDataProcessor.DontDisplayExceptions = true;

            var totalProducts = lookupData.GetRecordCount();
            var currentProduct = 1;

            lookupData.PrintOutput += (sender, e) =>
            {
                foreach (var primaryKeyValue in e.Result)
                {
                    var processResult = ProcessCurrentProduct(
                        primaryKeyValue
                        , context
                        , totalProducts
                        , currentProduct
                        , appProcedure);

                    if (!processResult.IsNullOrEmpty())
                    {
                        result = processResult;
                        return;
                    }
                }

            };
            lookupData.DoPrintOutput(10);
            if (result.IsNullOrEmpty())
            {
                if (!context.Commit("Recalculating Finished", true))
                {
                    result = GblMethods.LastError;
                }
            }
            DbDataProcessor.DontDisplayExceptions = false;
            return result;
        }

        private string ProcessCurrentProduct(
            PrimaryKeyValue primaryKeyValue
            , DataAccess.IDbContext context
            , int totalProducts
            , int currentProductIndex
            , AppProcedure procedure)
        {
            var productsTable = context.GetTable<Product>();
            var orderDetailsTable = context.GetTable<OrderDetail>();
            var timeClocksTable = context.GetTable<TimeClock>();

            var currentProduct = TableDefinition.GetEntityFromPrimaryKeyValue(primaryKeyValue);
            if (currentProduct != null)
            {
                currentProduct = productsTable
                    .Include(p => p.OrderDetailProducts)
                   .FirstOrDefault(p => p.Id == currentProduct.Id);

                if (currentProduct != null)
                {
                    var updateResult = UpdateProductValues(
                        totalProducts
                        , currentProductIndex
                        , currentProduct
                        , timeClocksTable
                        , orderDetailsTable
                        , context
                        , procedure);

                    if (!updateResult.IsNullOrEmpty())
                    {
                        return updateResult;
                    }

                    if (currentProduct.Id ==Id)
                    {
                        TotalRevenue = currentProduct.Revenue.GetValueOrDefault();
                        TotalCost = currentProduct.Cost.GetValueOrDefault();
                        Difference = TotalRevenue - TotalCost;
                        View.RefreshView();
                    }
                }
            }
            return string.Empty;
        }

        private string UpdateProductValues(
            int totalProducts
            , int currentCustomerIndex
            , Product currentProduct
            , IQueryable<TimeClock> timeClocksTable
            , IQueryable<OrderDetail> orderDetailsTable
            , DataAccess.IDbContext context
            , AppProcedure procedure)
        {
            View.UpdateRecalcProcedure(currentCustomerIndex, totalProducts, currentProduct.Description);

            var orderDetails = currentProduct.OrderDetailProducts.ToList();

            currentProduct.Revenue = orderDetails.Sum(p => p.ExtendedPrice);
            currentProduct.Cost = 0;
            
            var timeClocks = timeClocksTable
                .Include(p => p.ProjectTask)
                .ThenInclude(p => p.Project)
                .Include(p => p.User)
                .Where(p => p.ProjectTaskId != null
                && p.ProjectTask.Project.ProductId == currentProduct.Id);

            var cost = GetProductCost(timeClocks, procedure, "Project Task");
            currentProduct.Cost += cost;

            timeClocks = timeClocksTable
                .Include(p => p.Error)
                .Include(p => p.User)
                .Where(p => p.ErrorId != null
                            && p.Error.ProductId == currentProduct.Id);

            cost = GetProductCost(timeClocks, procedure, "Product Error");
            currentProduct.Cost += cost;

            timeClocks = timeClocksTable
                .Include(p => p.TestingOutline)
                .Include(p => p.User)
                .Where(p => p.TestingOutlineId != null
                            && p.TestingOutline.ProductId == currentProduct.Id);

            cost = GetProductCost(timeClocks, procedure, "Testing Outline");
            currentProduct.Cost += cost;

            timeClocks = timeClocksTable
                .Include(p => p.SupportTicket)
                .Include(p => p.User)
                .Where(p => p.SupportTicketId != null
                            && p.SupportTicket.ProductId == currentProduct.Id);

            cost = GetProductCost(timeClocks, procedure, "Support Ticket");
            currentProduct.Cost += cost;

            if (!context.SaveNoCommitEntity(currentProduct, "Saving Product", true))
            {
                return GblMethods.LastError;
            }
            return string.Empty;
        }

        private static double GetProductCost(IQueryable<TimeClock> timeClocks
            , AppProcedure procedure
            , string section)
        {
            var formatter = new IntegerEditControlSetup();
            var totalFormat = formatter.FormatValue(timeClocks.Count());

            var index = 1;
            var cost = (double)0;
            foreach (var timeClock in timeClocks)
            {
                var formattedIndex = formatter.FormatValue(index);
                procedure.SplashWindow.SetProgress($"Calculating {section} Time Clocks {formattedIndex}/{totalFormat}");
                var minutesSpent = timeClock.MinutesSpent;
                var hoursSpent = Math.Round((double)minutesSpent / 60, 2);
                var hourlyRate = timeClock.User.HourlyRate;
                cost += Math.Round(hoursSpent * hoursSpent, 2);
                index++;
            }

            return cost;
        }
    }
    }
