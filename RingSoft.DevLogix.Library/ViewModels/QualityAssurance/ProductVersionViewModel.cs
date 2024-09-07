using Newtonsoft.Json;
using RingSoft.App.Interop;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using RingSoft.DbLookup.TableProcessing;
using IDbContext = RingSoft.DevLogix.DataAccess.IDbContext;

namespace RingSoft.DevLogix.Library.ViewModels.QualityAssurance
{
    public interface IProductVersionView : IDbMaintenanceView
    {
        bool UploadFile(FileInfo file, Department department, Product product);

        void SetFocusToGrid();

        void SetFocusToDeploy();
    }

    public class ProcedureStatusArgs
    {
        public string Status { get; set; }
    }
    public class ProductVersionViewModel : DbMaintenanceViewModel<ProductVersion>
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
                CheckArchiveButtonState();
            }
        }

        private AutoFillSetup  _productAutoFillSetup;

        public AutoFillSetup ProductAutoFillSetup
        {
            get => _productAutoFillSetup;
            set
            {
                if (_productAutoFillSetup == value)
                    return;

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
                    return;

                _productAutoFillValue = value;
                OnPropertyChanged();
            }
        }

        private ProductVersionDepartmentsManager _departmentsManager;

        public ProductVersionDepartmentsManager DepartmentsManager
        {
            get => _departmentsManager;
            set
            {
                if (_departmentsManager == value)
                {
                    return;
                }
                _departmentsManager = value;
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

        private DateTime? _archiveDateTime;

        public DateTime? ArchiveDateTime
        {
            get => _archiveDateTime;
            set
            {
                if (_archiveDateTime == value)
                {
                    return;
                }
                _archiveDateTime = value;
                OnPropertyChanged();
                CheckArchiveButtonState();
            }
        }

        private AutoFillSetup _departmentAutoFillSetup;

        public AutoFillSetup DepartmentAutoFillSetup
        {
            get => _departmentAutoFillSetup;
            set
            {
                if (_departmentAutoFillSetup == value)
                    return;

                _departmentAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _departmentAutoFillValue;

        public AutoFillValue DepartmentAutoFillValue
        {
            get => _departmentAutoFillValue;
            set
            {
                if (_departmentAutoFillValue == value)
                {
                    return;
                }
                _departmentAutoFillValue = value;
                OnPropertyChanged(null, false);
                DeployCommand.IsEnabled = ArchiveDateTime != null && value.IsValid();
            }
        }

        private bool _productEnabled = true;

        public bool ProductEnabled
        {
            get => _productEnabled;
            set
            {
                if (_productEnabled == value)
                    return;
                _productEnabled = value;
                OnPropertyChanged();
            }
        }

        private bool _deployProductToDepartmentEnabled;

        public bool DeployProductToDepartmentEnabled
        {
            get => _deployProductToDepartmentEnabled;
            set
            {
                if (_deployProductToDepartmentEnabled == value)
                    return;

                _deployProductToDepartmentEnabled = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public new IProductVersionView View { get; private set; }

        public AutoFillValue DefaultProductAutoFillValue { get; private set; }

        public RelayCommand ArchiveCommand { get; private set; }

        public RelayCommand GetVersionCommand { get; private set; }

        public RelayCommand CreateVersionCommand { get; private set; }

        public RelayCommand DeployCommand { get; private set; }

        public event EventHandler<ProcedureStatusArgs> UpdateStatusEvent;

        public event EventHandler<ProcedureStatusArgs> DeployErrorEvent;

        private int _originalProductId;

        public ProductVersionViewModel()
        {
            ArchiveCommand = new RelayCommand(ArchiveVersion);

            GetVersionCommand = new RelayCommand(GetVersion);

            DeployCommand = new RelayCommand(DeployToDepartment);

            CreateVersionCommand = new RelayCommand(CreateVersion);

            DepartmentsManager = new ProductVersionDepartmentsManager(this);
            RegisterGrid(DepartmentsManager);
        }

        protected override void Initialize()
        {
            TablesToDelete.Add(AppGlobals.LookupContext.ProductVersionDepartments);
            if (base.View is IProductVersionView productVersionView)
            {
                View = productVersionView;
            }
            else
            {
                throw new InvalidOperationException("Invalid View");
            }
            ProductAutoFillSetup =
                new AutoFillSetup(AppGlobals.LookupContext.ProductVersions.GetFieldDefinition(p => p.ProductId));

            var departmentLookup = AppGlobals.LookupContext.DepartmentLookup.Clone();
            departmentLookup.FilterDefinition.AddFixedFilter(p => p.FtpAddress, Conditions.NotEqualsNull, "");

            DepartmentAutoFillSetup = new AutoFillSetup(departmentLookup);

            if (LookupAddViewArgs != null && LookupAddViewArgs.ParentWindowPrimaryKeyValue != null)
            {
                if (LookupAddViewArgs.ParentWindowPrimaryKeyValue.TableDefinition ==
                    AppGlobals.LookupContext.Products)
                {
                    var product =
                        AppGlobals.LookupContext.Products.GetEntityFromPrimaryKeyValue(LookupAddViewArgs
                            .ParentWindowPrimaryKeyValue);
                    product = product.FillOutProperties(false);
                    DefaultProductAutoFillValue = product.GetAutoFillValue();
                }
            }

            if (DefaultProductAutoFillValue.IsValid())
            {
                var defaultLookup = AppGlobals.LookupContext.ProductVersionLookup.Clone();
                var taskColumn = defaultLookup.GetColumnDefinition(p => p.Product);
                defaultLookup.DeleteVisibleColumn(taskColumn);
                foreach (var fixedFilter in ViewLookupDefinition.FilterDefinition.FixedFilters)
                {
                    if (fixedFilter is FieldFilterDefinition fieldFilter)
                    {
                        defaultLookup.FilterDefinition.AddFixedFieldFilter(
                            fieldFilter.FieldDefinition
                            , fieldFilter.Condition
                            , fieldFilter.Value);
                    }
                }
                FindButtonLookupDefinition = defaultLookup;
                KeyAutoFillSetup.LookupDefinition = defaultLookup;
            }

            FindButtonLookupDefinition.InitialOrderByType = OrderByTypes.Descending;
            base.Initialize();
        }

        protected override void SetupViewLookupDefinition(LookupDefinitionBase lookupDefinition)
        {
            if (LookupAddViewArgs != null && LookupAddViewArgs.ParentWindowPrimaryKeyValue != null)
            {
                if (LookupAddViewArgs.ParentWindowPrimaryKeyValue.TableDefinition ==
                    AppGlobals.LookupContext.Products)
                {
                    var product =
                        AppGlobals.LookupContext.Products.GetEntityFromPrimaryKeyValue(LookupAddViewArgs
                            .ParentWindowPrimaryKeyValue);

                    _originalProductId = product.Id;

                    var field = AppGlobals.LookupContext.ProductVersions.GetFieldDefinition(p => p.ProductId);

                    ViewLookupDefinition.FilterDefinition.AddFixedFilter(field,
                        Conditions.Equals, product.Id);
                }
            }

            lookupDefinition.InitialOrderByType = OrderByTypes.Ascending;


            base.SetupViewLookupDefinition(lookupDefinition);
        }

        public override void OnWindowClosing(CancelEventArgs e)
        {
            DbDataProcessor.ShowSqlStatementWindow(false);
            base.OnWindowClosing(e);
        }

        protected override void PopulatePrimaryKeyControls(ProductVersion newEntity, PrimaryKeyValue primaryKeyValue)
        {
            Id = newEntity.Id;
        }

        protected override void LoadFromEntity(ProductVersion entity)
        {
            ProductAutoFillValue =
                AppGlobals.LookupContext.OnAutoFillTextRequest(AppGlobals.LookupContext.Products,
                    entity.ProductId.ToString());

            Notes = entity.Notes;
            if (entity.ArchiveDateTime == null)
            {
                ArchiveDateTime = null;
            }
            else
            {
                ArchiveDateTime = entity.ArchiveDateTime.Value.ToLocalTime();
            }
            CheckArchiveButtonState();
        }

        protected override ProductVersion GetEntityData()
        {
            var result = new ProductVersion()
            {
                Id = Id,
                Description = KeyAutoFillValue.Text,
                Notes = Notes,
                ProductId = ProductAutoFillValue.GetEntity<Product>().Id,
            };

            var list = DepartmentsManager.GetEntityList()
                .OrderByDescending(p => p.ReleaseDateTime);
            if (list.Any())
            {
                var versionDepartment = list.FirstOrDefault();
                if (versionDepartment != null)
                {
                    result.VersionDate = versionDepartment.ReleaseDateTime;
                    result.DepartmentId = versionDepartment.DepartmentId;
                }
            }
            if (ArchiveDateTime.HasValue)
            {
                result.ArchiveDateTime = ArchiveDateTime.Value.ToUniversalTime();
            }

            return result;
        }

        protected override void ClearData()
        {
            Id = 0;
            ProductAutoFillValue = DefaultProductAutoFillValue;
            Notes = null;
            DepartmentAutoFillValue = null;
            ArchiveDateTime = null;
            CheckArchiveButtonState();
        }

        protected override bool ValidateEntity(ProductVersion entity)
        {
            var result = DepartmentsManager.ValidateGrid();
            if (!result)
            {
                return result;
            }
            
            if (entity.ProductId != _originalProductId)
            {
                if (!ValidateOnlyVersion())
                    return false;
            }

            return base.ValidateEntity(entity);
        }

        protected override bool DeleteEntity()
        {
            var context = SystemGlobals.DataRepository.GetDataContext();
            var query = context.GetTable<ProductVersion>();
            if (query != null)
            {
                if (!ValidateOnlyVersion()) return false;
                var entity = query.FirstOrDefault(p => p.Id == Id);

                if (entity != null)
                {
                    DepartmentsManager.DeleteNoCommitData(entity, context);
                    return context.DeleteEntity(entity, $"Deleting Product Version '{entity.Description}'");
                }
            }
            return false;

        }

        private bool ValidateOnlyVersion()
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<ProductVersion>();
            var query = table.Where(p => p.ProductId == _originalProductId);

            if (query.Count() < 2)
            {
                var message = "You cannot delete the version or change the product for only version of this product.";
                var caption = "Delete Validation Fail";
                ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                return false;
            }


            return true;
        }

        private void ArchiveVersion()
        {
            if (ArchiveDateTime == null)
            {
                if (ProductAutoFillValue.IsValid())
                {
                    var product =
                        AppGlobals.LookupContext.Products.GetEntityFromPrimaryKeyValue(ProductAutoFillValue
                            .PrimaryKeyValue);

                    if (product != null)
                    {
                        var context = AppGlobals.DataRepository.GetDataContext();
                        var productTable = context.GetTable<Product>();
                        if (productTable != null)
                        {
                            product = productTable.FirstOrDefault(p => p.Id == product.Id);
                            if (product != null && product.ArchiveDepartmentId.HasValue)
                            {
                                AddNewDepartment(product.ArchiveDepartmentId.Value);
                            }
                            if (product != null && !product.ArchivePath.IsNullOrEmpty() && !product.InstallerFileName.IsNullOrEmpty())
                            {
                                var file = new FileInfo(product.InstallerFileName);
                                if (file != null)
                                {
                                    var archiveFile = GetArchiveFileName(product, file);
                                    try
                                    {
                                        file.CopyTo(archiveFile);
                                        ArchiveDateTime = GblMethods.NowDate();
                                        DoSave();
                                        View.SetFocusToDeploy();
                                    }
                                    catch (Exception e)
                                    {
                                        ControlsGlobals.UserInterface.ShowMessageBox("Error Copying File", "Error",
                                            RsMessageBoxIcons.Exclamation);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private string GetArchiveFileName(Product product, FileInfo file)
        {
            var archivePath = product.ArchivePath;
            if (!archivePath.EndsWith("\\"))
            {
                archivePath += "\\";
            }

            var fileName = file.Name;
            var typePos = fileName.LastIndexOf(".");
            var extension = string.Empty;
            if (typePos != -1)
            {
                extension = fileName.RightStr(fileName.Length - typePos);
                fileName = fileName.LeftStr(typePos);
            }

            var archiveFile = $"{archivePath}{fileName}_{Id}{extension}";
            return archiveFile;
        }

        private void GetVersion()
        {
            if (ProductAutoFillValue.IsValid())
            {
                var product =
                    AppGlobals.LookupContext.Products.GetEntityFromPrimaryKeyValue(ProductAutoFillValue
                        .PrimaryKeyValue);

                if (product != null)
                {
                    var context = AppGlobals.DataRepository.GetDataContext();
                    var productTable = context.GetTable<Product>();
                    if (productTable != null)
                    {
                        product = productTable.FirstOrDefault(p => p.Id == product.Id);
                        if (product != null && !product.ArchivePath.IsNullOrEmpty() &&
                            !product.InstallerFileName.IsNullOrEmpty())
                        {
                            var file = new FileInfo(product.InstallerFileName);
                            if (file != null)
                            {
                                var archiveFile = GetArchiveFileName(product, file);
                                if (!archiveFile.IsNullOrEmpty())
                                {
                                    try
                                    {
                                        Process.Start(archiveFile);
                                    }
                                    catch (Exception e)
                                    {
                                        var caption = "Error Getting Version";
                                        ControlsGlobals.UserInterface.ShowMessageBox(e.Message, caption,
                                            RsMessageBoxIcons.Error);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void DeployToDepartment()
        {
            if (ProductAutoFillValue.IsValid() && DepartmentAutoFillValue.IsValid() && ArchiveDateTime.HasValue)
            {
                var context = AppGlobals.DataRepository.GetDataContext();
                var department =
                    AppGlobals.LookupContext.Departments.GetEntityFromPrimaryKeyValue(DepartmentAutoFillValue
                        .PrimaryKeyValue);

                if (department == null)
                {
                    return;
                }
                else
                {
                    department = context.GetTable<Department>().FirstOrDefault(p => p.Id == department.Id);
                }

                var product =
                    AppGlobals.LookupContext.Products.GetEntityFromPrimaryKeyValue(ProductAutoFillValue
                        .PrimaryKeyValue);

                if (product == null)
                {
                    return;
                }
                else
                {
                    product = context.GetTable<Product>().FirstOrDefault(p => p.Id == product.Id);
                }

                var file = new FileInfo(product.InstallerFileName);
                if (file != null)
                {
                    View.UploadFile(file, department, product);
                }
            }
        }
        public bool UploadFile(FileInfo file, Department department, Product product)
        {
            UpdateStatusEvent?.Invoke(this, new ProcedureStatusArgs{Status = "Copying File"});
            var archiveFile = GetArchiveFileName(product, file);
            var archiveFileInfo = new FileInfo(archiveFile);
            var fileText = $"{archiveFileInfo.DirectoryName}\\{file.Name}";
            FileInfo fileToUpload = null;
            try
            {
                var fileTestInfo = new FileInfo(fileText);
                if (fileTestInfo.Exists)
                {
                    fileTestInfo.Delete();
                }
                fileToUpload = archiveFileInfo.CopyTo(fileText);
            }
            catch (Exception e)
            {
                DeployErrorEvent?.Invoke(this, new ProcedureStatusArgs { Status = e.Message });
                return false;
            }

            var client = new WebClient();
            var ftpAddress = department.FtpAddress;
            if (!ftpAddress.EndsWith("/"))
            {
                ftpAddress += "/";
            }

            try
            {
                UpdateStatusEvent?.Invoke(this, new ProcedureStatusArgs { Status = "Deploying File" });
                var password = department.FtpPassword.Decrypt();
                client.Credentials = new NetworkCredential(department.FtpUsername, password);

                client.UploadFile($"{ftpAddress}{fileToUpload.Name}", WebRequestMethods.Ftp.UploadFile, fileToUpload.FullName);
                File.Delete(fileToUpload.FullName);
            }
            catch (Exception e)
            {
                DeployErrorEvent?.Invoke(this, new ProcedureStatusArgs{Status = e.Message});
                return false;
            }

            UpdateStatusEvent?.Invoke(this, new ProcedureStatusArgs { Status = "Deploying Json File" });
            var ringSoftAppsFileName = "RingSoftApps.json";
            var ringSoftApps = new List<RingSoftApps>();
            var ringSoftApp = new RingSoftApps();
            try
            {
                var appsText = client.DownloadString($"{ftpAddress}{ringSoftAppsFileName}");
                if (!appsText.IsNullOrEmpty())
                {
                    ringSoftApps = JsonConvert.DeserializeObject<List<RingSoftApps>>(appsText);
                    ringSoftApp = ringSoftApps.FirstOrDefault(p => p.AppGuid == product.AppGuid);
                    if (ringSoftApp == null)
                    {
                        ringSoftApp = new RingSoftApps();
                        ringSoftApps.Add(ringSoftApp);
                    }
                }
            }
            catch (Exception e)
            {
                ringSoftApps.Add(ringSoftApp);
            }

            ringSoftApp.AppGuid = product.AppGuid;
            ringSoftApp.InstallerFileName = fileToUpload.Name;
            ringSoftApp.VersionId = Id;
            ringSoftApp.VersionDate = DateTime.Now.ToUniversalTime();
            ringSoftApp.VersionName = KeyAutoFillValue.Text;
            var jsonText = JsonConvert.SerializeObject(ringSoftApps);
            try
            {
                var jsonFile =  $"{archiveFileInfo.DirectoryName}\\{ringSoftAppsFileName}";
                File.WriteAllText(jsonFile, jsonText);
                client.UploadFile($"{ftpAddress}{ringSoftAppsFileName}", WebRequestMethods.Ftp.UploadFile, jsonFile);
                File.Delete(jsonFile);
            }
            catch (Exception e)
            {
                DeployErrorEvent?.Invoke(this, new ProcedureStatusArgs { Status = e.Message });
                return false;
            }

            return true;
        }

        private void CheckArchiveButtonState()
        {
            DeployCommand.IsEnabled = ArchiveDateTime != null && DepartmentAutoFillValue.IsValid();
            DeployProductToDepartmentEnabled = ArchiveDateTime != null;
            ProductEnabled = ArchiveDateTime == null;
            ArchiveCommand.IsEnabled = ArchiveDateTime == null && Id > 0;
            GetVersionCommand.IsEnabled = ArchiveDateTime != null && Id > 0;
        }

        private void CreateVersion()
        {
            if (RecordDirty)
            {
                if (DoSave() != DbMaintenanceResults.Success)
                {
                    return;
                }
            }
            OnNewButton();
            var context = AppGlobals.DataRepository.GetDataContext();
            var productId = ProductAutoFillValue.GetEntity(AppGlobals.LookupContext.Products).Id;
            if (productId == 0)
            {
                SetNewVersion(context, "00.85.1");
                return;
            }
            var productVersionTable = context.GetTable<ProductVersion>();
            var versionsExist = productVersionTable.Any(p =>
                p.ProductId == productId);

            if (versionsExist)
            {
                var lastProduct = productVersionTable.OrderBy(p => p.Id)
                    .LastOrDefault(p => p.ProductId == productId);
                var newVersion = CreateNewVersion(lastProduct.Description);
                var existingProduct =
                    productVersionTable.FirstOrDefault(p => p.ProductId == productId && p.Description == newVersion);
                while (existingProduct != null)
                {
                    newVersion = CreateNewVersion(newVersion);
                    existingProduct =
                        productVersionTable.FirstOrDefault(p => p.ProductId == productId && p.Description == newVersion);
                }
                SetNewVersion(context, newVersion);
            }
            else
            {
                SetNewVersion(context, "00.85.1");
            }
        }

        private void SetNewVersion(DataAccess.IDbContext context, string version)
        {
            KeyAutoFillValue = new AutoFillValue(new PrimaryKeyValue(TableDefinition), version);
            var product = GetProduct(context);
            if (product != null)
            {
                AddNewDepartment(product.CreateDepartmentId);
            }

            DoSave();
        }

        private Product GetProduct(IDbContext context)
        {
            if (ProductAutoFillValue.IsValid())
            {
                var productQuery = context.GetTable<Product>();
                return productQuery.FirstOrDefault(p =>
                    p.Id == ProductAutoFillValue.GetEntity(AppGlobals.LookupContext.Products).Id)!;
            }

            return null;
        }


        private void AddNewDepartment(int departmentId)
        {
            DepartmentsManager.AddNewDepartment(departmentId);
            RecordDirty = true;
        }

        private string CreateNewVersion(string existingVersion)
        {
            var result = string.Empty;
            var newVersion = 1;
            var lastDecimal = existingVersion.LastIndexOf('.');
            if (lastDecimal > -1)
            {
                var text = existingVersion.RightStr(existingVersion.Length - (lastDecimal + 1));
                if (!text.IsNullOrEmpty())
                {
                    if (IsNumeric(text))
                    {
                        newVersion = text.ToInt() + 1;
                        var leftSide = existingVersion.LeftStr(lastDecimal);
                        if (!leftSide.IsNullOrEmpty())
                        {
                            return $"{leftSide}.{newVersion}";
                        }
                    }
                }
            }
            return $"{existingVersion}.{newVersion}";
        }

        private bool IsNumeric(string version)
        {
            var charList = version.ToCharArray().ToList();
            foreach (var versionChar in charList)
            {
                switch (versionChar)
                {
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        break;
                    default:
                        return false;
                }
            }
            return true;
        }

        protected override void OnPropertyChanged(string propertyName = null, bool raiseDirtyFlag = true)
        {
            if (raiseDirtyFlag)
            {
                
            }
            base.OnPropertyChanged(propertyName, raiseDirtyFlag);
        }
    }
}
