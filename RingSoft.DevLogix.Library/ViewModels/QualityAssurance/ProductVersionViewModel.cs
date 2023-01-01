using System;
using System.IO;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DevLogix.DataAccess.Model;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DevLogix.Library.ViewModels.QualityAssurance
{
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
                OnPropertyChanged();
            }
        }



        public RelayCommand ArchiveCommand { get; private set; }

        public RelayCommand GetVersionCommand { get; private set; }

        public RelayCommand DeployCommand { get; private set; }

        public ProductVersionViewModel()
        {
            ArchiveCommand = new RelayCommand(ArchiveVersion);

            GetVersionCommand = new RelayCommand(GetVersion);

            DeployCommand = new RelayCommand(DeployToDepartment);
        }

        protected override void Initialize()
        {
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

        private void GetVersion()
        {

        }

        private void DeployToDepartment()
        {

        }
    }
}
