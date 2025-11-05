using System;
using System.Linq;
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

        public int ProductId { get; private set; }

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

        public override async void SetCellValue(DataEntryGridEditingCellProps value)
        {
            var column = (CustomerProductColumns)value.ColumnId;
            switch (column)
            {
                case CustomerProductColumns.Product:
                    if (value is DataEntryGridAutoFillCellProps autoFillCellProps)
                    {
                        ProductAutoFillValue = autoFillCellProps.AutoFillValue;
                        ProductId = ProductAutoFillValue.GetEntity<Product>().Id;
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
            ProductId = entity.ProductId;
            ExpirationDate = entity.ExpirationDate.ToLocalTime();
        }

        public override void SaveToEntity(CustomerProduct entity, int rowIndex)
        {
            entity.ProductId = ProductAutoFillValue.GetEntity<Product>().Id;
            entity.ExpirationDate = ExpirationDate.GetValueOrDefault().ToUniversalTime();
        }

        public override bool ValidateRow()
        {
            if (!base.ValidateRow())
            {
                return false;
            }
            if (!IsNew && ProductId != 0)
            {
                var rows = Manager.Rows.OfType<CustomerProductRow>();
                var dupRows = rows.Where(p => p.ProductId == ProductId).ToList();
                if (dupRows.Count > 1)
                {
                    var message = "Duplicate Products not allowed!";
                    var caption = "Validation Fail";
                    Manager.Grid?.GotoCell(this, CustomerProductManager.ProductColumnId);
                    ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                    return false;
                }

            }
            return true;

        }
    }
}
