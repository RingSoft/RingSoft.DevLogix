using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DevLogix.DataAccess.Model;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.QueryBuilder;
using System.Net;
using Newtonsoft.Json;
using RingSoft.App.Interop;
using RingSoft.DbMaintenance;

namespace RingSoft.DevLogix.Library.ViewModels.QualityAssurance
{
    public interface IProductVersionView : IDbMaintenanceView
    {
        bool UploadFile(FileInfo file, Department department, Product product);
    }

    public class ProcedureStatusArgs
    {
        public string Status { get; set; }
    }
    public class ProductVersionViewModel : DevLogixDbMaintenanceViewModel<ProductVersion>
    {
        public override TableDefinition<ProductVersion> TableDefinition => AppGlobals.LookupContext.ProductVersions;

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



        public new IProductVersionView View { get; private set; }

        public RelayCommand ArchiveCommand { get; private set; }

        public RelayCommand GetVersionCommand { get; private set; }

        public RelayCommand DeployCommand { get; private set; }

        public event EventHandler<ProcedureStatusArgs> UpdateStatusEvent;

        public event EventHandler<ProcedureStatusArgs> DeployErrorEvent;

        public ProductVersionViewModel()
        {
            ArchiveCommand = new RelayCommand(ArchiveVersion);

            GetVersionCommand = new RelayCommand(GetVersion);

            DeployCommand = new RelayCommand(DeployToDepartment);
        }

        protected override void Initialize()
        {
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
            DepartmentsManager = new ProductVersionDepartmentsManager(this);

            var departmentLookup = AppGlobals.LookupContext.DepartmentLookup.Clone();
            departmentLookup.FilterDefinition.AddFixedFilter(p => p.FtpAddress, Conditions.NotEqualsNull, "");

            DepartmentAutoFillSetup = new AutoFillSetup(departmentLookup);
            base.Initialize();
        }

        protected override ProductVersion PopulatePrimaryKeyControls(ProductVersion newEntity, PrimaryKeyValue primaryKeyValue)
        {
            var query = AppGlobals.DataRepository.GetDataContext().GetTable<ProductVersion>();
            var result = query
                .Include(p => p.ProductVersionDepartments)
                .FirstOrDefault(p => p.Id == newEntity.Id);
            if (result != null)
            {
                Id = result.Id;
                KeyAutoFillValue = AppGlobals.LookupContext.OnAutoFillTextRequest(TableDefinition, Id.ToString());
            }

            return result;
        }

        protected override void LoadFromEntity(ProductVersion entity)
        {
            ProductAutoFillValue =
                AppGlobals.LookupContext.OnAutoFillTextRequest(AppGlobals.LookupContext.Products,
                    entity.ProductId.ToString());

            Notes = entity.Notes;
            DepartmentsManager.LoadGrid(entity.ProductVersionDepartments);
            if (entity.ArchiveDateTime != null)
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
            };

            if (ArchiveDateTime.HasValue)
            {
                result.ArchiveDateTime = ArchiveDateTime.Value.ToUniversalTime();
            }

            if (ProductAutoFillValue.IsValid())
            {
                result.ProductId = AppGlobals.LookupContext.Products
                    .GetEntityFromPrimaryKeyValue(ProductAutoFillValue.PrimaryKeyValue).Id;
            }
            return result;
        }

        protected override void ClearData()
        {
            Id = 0;
            ProductAutoFillValue = null;
            if (LookupAddViewArgs != null && LookupAddViewArgs.ParentWindowPrimaryKeyValue != null)
            {
                if (LookupAddViewArgs.ParentWindowPrimaryKeyValue.TableDefinition ==
                    AppGlobals.LookupContext.Products)
                {
                    var product =
                        AppGlobals.LookupContext.Products.GetEntityFromPrimaryKeyValue(LookupAddViewArgs
                            .ParentWindowPrimaryKeyValue);
                    ProductAutoFillValue =
                        AppGlobals.LookupContext.OnAutoFillTextRequest(AppGlobals.LookupContext.Products,
                            product.Id.ToString());
                }
            }

            Notes = null;
            DepartmentsManager.SetupForNewRecord();
            DepartmentAutoFillValue = null;
            ArchiveDateTime = null;
            CheckArchiveButtonState();
        }

        protected override bool SaveEntity(ProductVersion entity)
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            if (context != null)
            {
                if (context.SaveEntity(entity, $"Saving Product Version '{entity.Description}'"))
                {
                    var departmentsToAdd = DepartmentsManager.GetEntityList();
                    var departmentsToRemove = context.GetTable<ProductVersionDepartment>()
                        .Where(p => p.VersionId == entity.Id);

                    foreach (var productVersionDepartment in departmentsToAdd)
                    {
                        productVersionDepartment.VersionId = entity.Id;
                    }
                    context.RemoveRange(departmentsToRemove);
                    context.AddRange(departmentsToAdd);

                    return context.Commit("Saving Product Version Details");
                }
            }
            return false;

        }

        protected override bool DeleteEntity()
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            var query = context.GetTable<ProductVersion>();
            if (query != null)
            {
                var departmentsToRemove = context.GetTable<ProductVersionDepartment>()
                    .Where(p => p.VersionId == Id);

                context.RemoveRange(departmentsToRemove);
                var entity = query.FirstOrDefault(p => p.Id == Id);
                
                if (entity != null)
                {
                    return context.DeleteEntity(entity, $"Deleting Product Version '{entity.Description}'");
                }
            }
            return false;

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
                            if (product != null && !product.ArchivePath.IsNullOrEmpty() && !product.InstallerFileName.IsNullOrEmpty())
                            {
                                var file = new FileInfo(product.InstallerFileName);
                                if (file != null)
                                {
                                    var archiveFile = GetArchiveFileName(product, file);
                                    try
                                    {
                                        file.CopyTo(archiveFile);
                                        ArchiveDateTime = DateTime.Now;
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
                                if (archiveFile != null)
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
    }
}
