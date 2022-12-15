using System;
using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model;

namespace RingSoft.DevLogix.Library.ViewModels.QualityAssurance
{
    public class ProductVersionDepartmentsRow : DbMaintenanceDataEntryGridRow<ProductVersionDepartment>
    {
        public new ProductVersionDepartmentsManager Manager { get; set; }

        public AutoFillSetup DepartmentAutoFillSetup { get; set; }
        public AutoFillValue DepartmentAutoFillValue { get; set; }
        public int DepartmentId { get; set; }
        public DateTime? ReleaseDateTime { get; set; } = null;

        public ProductVersionDepartmentsRow(ProductVersionDepartmentsManager manager) : base(manager)
        {
            Manager = manager;
            DepartmentAutoFillSetup = new AutoFillSetup(AppGlobals.LookupContext.ProductVersionDepartments
                .GetFieldDefinition(p => p.DepartmentId));
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (ProductVersionDepartmentsColumns)columnId;
            switch (column)
            {
                case ProductVersionDepartmentsColumns.Department:
                    return new DataEntryGridAutoFillCellProps(this, columnId, DepartmentAutoFillSetup,
                        DepartmentAutoFillValue);
                case ProductVersionDepartmentsColumns.ReleaseDate:
                    return new DataEntryGridDateCellProps(this, columnId,
                        new DateEditControlSetup { DateFormatType = DateFormatTypes.DateTime }, ReleaseDateTime);
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return null;
        }

        public override void SetCellValue(DataEntryGridEditingCellProps value)
        {
            var column = (ProductVersionDepartmentsColumns)value.ColumnId;
            switch (column)
            {
                case ProductVersionDepartmentsColumns.Department:
                    if (value is DataEntryGridAutoFillCellProps dataEntryGridEditingCellProps)
                    {
                        DepartmentAutoFillValue = dataEntryGridEditingCellProps.AutoFillValue;
                        if (DepartmentAutoFillValue.IsValid())
                        {
                            DepartmentId = AppGlobals.LookupContext.Departments
                                .GetEntityFromPrimaryKeyValue(DepartmentAutoFillValue.PrimaryKeyValue).Id;
                        }
                        if (ReleaseDateTime == null)
                        {
                            var newDate = DateTime.Now;
                            ReleaseDateTime = new DateTime(newDate.Year, newDate.Month, newDate.Day, newDate.Hour,
                                newDate.Minute, newDate.Second);
                        }
                    }
                    break;
                case ProductVersionDepartmentsColumns.ReleaseDate:
                    if (value is DataEntryGridDateCellProps dataEntryGridDateCellProps)
                    {
                        ReleaseDateTime = dataEntryGridDateCellProps.Value;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            base.SetCellValue(value);
        }

        public override void LoadFromEntity(ProductVersionDepartment entity)
        {
            DepartmentAutoFillValue =
                AppGlobals.LookupContext.OnAutoFillTextRequest(AppGlobals.LookupContext.Departments,
                    entity.DepartmentId.ToString());
            ReleaseDateTime = entity.ReleaseDateTime.ToLocalTime();
        }

        public override bool ValidateRow()
        {
            if (!IsNew)
            {
                if (!DepartmentAutoFillValue.IsValid())
                {
                    return false;
                }

                var department =
                    AppGlobals.LookupContext.Departments.GetEntityFromPrimaryKeyValue(DepartmentAutoFillValue
                        .PrimaryKeyValue);

                var rows = Manager.Rows.OfType<ProductVersionDepartmentsRow>();
                if (rows.Where(p => p.DepartmentId == DepartmentId).ToList().Count > 1)
                {
                    return false;
                }

            }
            return true;
        }

        public override void SaveToEntity(ProductVersionDepartment entity, int rowIndex)
        {
            var department =
                AppGlobals.LookupContext.Departments.GetEntityFromPrimaryKeyValue(DepartmentAutoFillValue
                    .PrimaryKeyValue);

            entity.DepartmentId = department.Id;
            entity.ReleaseDateTime = ReleaseDateTime.Value.ToUniversalTime();
        }
    }
}
