using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DevLogix.DataAccess.Model;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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

        protected override void Initialize()
        {
            ProductAutoFillSetup =
                new AutoFillSetup(AppGlobals.LookupContext.ProductVersions.GetFieldDefinition(p => p.ProductId));
            DepartmentsManager = new ProductVersionDepartmentsManager(this);
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
        }

        protected override ProductVersion GetEntityData()
        {
            var result = new ProductVersion()
            {
                Id = Id,
                Description = KeyAutoFillValue.Text,
                Notes = Notes
            };

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
    }
}
