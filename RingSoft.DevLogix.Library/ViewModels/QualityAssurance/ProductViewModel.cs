using RingSoft.DbLookup;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DevLogix.DataAccess.Model;
using System.Linq;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DevLogix.DataAccess.LookupModel;

namespace RingSoft.DevLogix.Library.ViewModels.QualityAssurance
{
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
                OnPropertyChanged();
            }
        }

        protected override void Initialize()
        {
            ProductVersionLookupDefinition = MakeProductLookupDefinition();
            base.Initialize();
        }

        private LookupDefinition<ProductVersionLookup, ProductVersion> MakeProductLookupDefinition()
        {
            var result = AppGlobals.LookupContext.ProductVersionLookup.Clone();

            var tableDefinition = AppGlobals.LookupContext.ProductVersions;
            var query = new SelectQuery(tableDefinition.TableName);
            query.AddSelectColumn(tableDefinition.GetFieldDefinition(p => p.Id).FieldName);
            query.AddSelectColumn(tableDefinition.GetFieldDefinition(p => p.ProductId).FieldName);
            query.AddSelectColumn(tableDefinition.GetFieldDefinition(p => p.Description).FieldName);
            query.AddSelectFormulaColumn("VersionDate", MakeVersionDateFormula());
            query.AddSelectFormulaColumn("MaxDepartment", MakeMaxDepartmentFormula());

            var sql = AppGlobals.LookupContext.DataProcessor.SqlGenerator.GenerateSelectStatement(query);
            result.HasFromFormula(sql);

            var column = result.AddVisibleColumnDefinition(p => p.VersionDate
                , "VersionDate", "");
            column.HasDateType(DbDateTypes.DateTime)
                .HasDateFormatString(string.Empty)
                .HasConvertToLocalTime();

            result.InitialSortColumnDefinition = column;
            result.InitialOrderByType = OrderByTypes.Descending;

            column = result.AddVisibleColumnDefinition(p => p.MaxDepartment
                , "MaxDepartment", "");

            return result;
        }

        private string MakeVersionDateFormula()
        {
            var result = string.Empty;

            var tableDefinition = AppGlobals.LookupContext.ProductVersionDepartments;
            var field = tableDefinition.GetFieldDefinition(p => p.ReleaseDateTime).FieldName;
            field = AppGlobals.LookupContext.DataProcessor.SqlGenerator.FormatSqlObject(field);
            field = $"{AppGlobals.LookupContext.DataProcessor.SqlGenerator.FormatSqlObject(tableDefinition.TableName)}.{field}";
            var query = new SelectQuery(tableDefinition.TableName);
            query.AddSelectFormulaColumn("VersionDate", $"MAX({field})");

            field = tableDefinition.GetFieldDefinition(p => p.VersionId).FieldName;
            field = AppGlobals.LookupContext.DataProcessor.SqlGenerator.FormatSqlObject(field);
            field = $"{AppGlobals.LookupContext.DataProcessor.SqlGenerator.FormatSqlObject(tableDefinition.TableName)}.{field}";

            var targetField = AppGlobals.LookupContext.ProductVersions
                .GetFieldDefinition(p => p.Id).FieldName;
            targetField = AppGlobals.LookupContext.DataProcessor.SqlGenerator.FormatSqlObject(targetField);
            targetField = $"{AppGlobals.LookupContext.DataProcessor.SqlGenerator.FormatSqlObject(AppGlobals.LookupContext.ProductVersions.TableName)}.{targetField}";

            query.AddWhereItemFormula($"{field} = {targetField}");

            result = AppGlobals.LookupContext.DataProcessor.SqlGenerator.GenerateSelectStatement(query);

            return result;
        }

        private string MakeMaxDepartmentFormula()
        {
            var result = string.Empty;
            var tableDefinition = AppGlobals.LookupContext.ProductVersionDepartments;
            var descriptionField = AppGlobals.LookupContext.Departments.GetFieldDefinition(p => p.Description).FieldName;
            var query = new SelectQuery(tableDefinition.TableName);
            var departmentJoin =
                query.AddPrimaryJoinTable(JoinTypes.InnerJoin, AppGlobals.LookupContext.Departments.TableName)
                    .AddJoinField(AppGlobals.LookupContext.Departments.GetFieldDefinition(p => p.Id).FieldName
                    , tableDefinition.GetFieldDefinition(p => p.DepartmentId).FieldName);
            query.AddSelectColumn(descriptionField, departmentJoin);

            var field = tableDefinition.GetFieldDefinition(p => p.VersionId).FieldName;
            field = AppGlobals.LookupContext.DataProcessor.SqlGenerator.FormatSqlObject(field);
            field = $"{AppGlobals.LookupContext.DataProcessor.SqlGenerator.FormatSqlObject(tableDefinition.TableName)}.{field}";

            var targetField = AppGlobals.LookupContext.ProductVersions
                .GetFieldDefinition(p => p.Id).FieldName;
            targetField = AppGlobals.LookupContext.DataProcessor.SqlGenerator.FormatSqlObject(targetField);
            targetField = $"{AppGlobals.LookupContext.DataProcessor.SqlGenerator.FormatSqlObject(AppGlobals.LookupContext.ProductVersions.TableName)}.{targetField}";

            query.AddWhereItemFormula($"{field} = {targetField}");

            field = tableDefinition.GetFieldDefinition(p => p.ReleaseDateTime).FieldName;
            field = AppGlobals.LookupContext.DataProcessor.SqlGenerator.FormatSqlObject(field);
            field = $"{AppGlobals.LookupContext.DataProcessor.SqlGenerator.FormatSqlObject(tableDefinition.TableName)}.{field}";

            query.AddWhereItemFormula($"{field} = ({MakeVersionDateFormula()})");

            result = AppGlobals.LookupContext.DataProcessor.SqlGenerator.GenerateSelectStatement(query);
            return result;
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
                ProductVersionLookupDefinition.FilterDefinition.AddFixedFilter("ProductId", Conditions.Equals,
                    Id.ToString(), "ProductId");
                ProductVersionLookupCommand = GetLookupCommand(LookupCommands.Refresh);
            }

            return result;
        }

        protected override void LoadFromEntity(Product entity)
        {
            Notes = entity.Notes;
        }

        protected override Product GetEntityData()
        {
            var result = new Product()
            {
                Id = Id,
                Description = KeyAutoFillValue.Text,
                Notes = Notes
            };

            return result;
        }

        protected override void ClearData()
        {
            Id = 0;
            Notes = null;
            ProductVersionLookupCommand = GetLookupCommand(LookupCommands.Clear);
        }

        protected override bool SaveEntity(Product entity)
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            if (context != null)
            {
                return context.SaveEntity(entity, $"Saving Product '{entity.Description}'");
            }
            return false;

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
    }
}
