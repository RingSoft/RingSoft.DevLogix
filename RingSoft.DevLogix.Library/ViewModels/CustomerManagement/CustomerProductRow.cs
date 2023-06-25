using System;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model;
using RingSoft.DevLogix.DataAccess.Model.CustomerManagement;

namespace RingSoft.DevLogix.Library.ViewModels.CustomerManagement
{
    public class CustomerProductRow : DbMaintenanceDataEntryGridRow<CustomerProduct>
    {
        public new CustomerProductManager Manager { get; }

        public AutoFillSetup ProductAutoFillSetup { get; }

        public AutoFillValue ProductAutoFillValue { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public CustomerProductRow(CustomerProductManager manager) : base(manager)
        {
            Manager = manager;
            ProductAutoFillSetup = new AutoFillSetup(
                TableDefinition.GetFieldDefinition(p => p.ProductId));
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (CustomerProductColumns)columnId;

            switch (column)
            {
                case CustomerProductColumns.Product:
                    return new DataEntryGridAutoFillCellProps(this, columnId, ProductAutoFillSetup,
                        ProductAutoFillValue);
                case CustomerProductColumns.ExpirationDate:
                    return new DataEntryGridDateCellProps(this, columnId, new DateEditControlSetup()
                    {
                        DateFormatType = DateFormatTypes.DateOnly,
                    }, ExpirationDate);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void SetCellValue(DataEntryGridEditingCellProps value)
        {
            var column = (CustomerProductColumns)value.ColumnId;
            switch (column)
            {
                case CustomerProductColumns.Product:
                    if (value is DataEntryGridAutoFillCellProps autoFillCellProps)
                    {
                        ProductAutoFillValue = autoFillCellProps.AutoFillValue;
                    }
                    break;
                case CustomerProductColumns.ExpirationDate:
                    if (value is DataEntryGridDateCellProps dateCellProps)
                    {
                        ExpirationDate = dateCellProps.Value;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            base.SetCellValue(value);
        }

        public override void LoadFromEntity(CustomerProduct entity)
        {
            ProductAutoFillValue = entity.Product.GetAutoFillValue();
            ExpirationDate = entity.ExpirationDate;
        }

        public override void SaveToEntity(CustomerProduct entity, int rowIndex)
        {
            entity.ProductId = ProductAutoFillValue.GetEntity<Product>().Id;
            entity.ExpirationDate = ExpirationDate.GetValueOrDefault();
        }
    }
}
