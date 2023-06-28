using System;
using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model;
using RingSoft.DevLogix.DataAccess.Model.CustomerManagement;
using RingSoft.DevLogix.Library.ViewModels.CustomerManagement;

namespace RingSoft.DevLogix.Library.ViewModels
{
    public class OrderDetailsRow : DbMaintenanceDataEntryGridRow<OrderDetail>
    {
        public new OrderDetailsManager Manager { get; }

        public AutoFillSetup ProductAutoFillSetup { get; }

        public AutoFillValue ProductAutoFillValue { get; set; }

        public double Quantity { get; set; }

        public double UnitPrice { get; set; }

        public double ExtendedPrice { get; set; }

        public double Discount { get; set; }

        private AutoFillValue _oldAutoFillValue;

        public OrderDetailsRow(OrderDetailsManager manager) : base(manager)
        {
            Manager = manager;
            ProductAutoFillSetup = new AutoFillSetup(TableDefinition
                .GetFieldDefinition(p => p.ProductId));
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (OrderDetailsColumns)columnId;

            switch (column)
            {
                case OrderDetailsColumns.Product:
                    return new DataEntryGridAutoFillCellProps(
                        this, columnId, ProductAutoFillSetup, ProductAutoFillValue);
                case OrderDetailsColumns.Quantity:
                    return new DataEntryGridDecimalCellProps(this, columnId
                        , new DecimalEditControlSetup
                        {
                            FormatType = DecimalEditFormatTypes.Number,
                        }, (decimal)Quantity);
                case OrderDetailsColumns.Price:
                    return new DataEntryGridDecimalCellProps(this, columnId
                        , new DecimalEditControlSetup
                        {
                            FormatType = DecimalEditFormatTypes.Currency,
                        }, (decimal)UnitPrice);
                case OrderDetailsColumns.ExtendedPrice:
                    return new DataEntryGridDecimalCellProps(this, columnId
                        , new DecimalEditControlSetup
                        {
                            FormatType = DecimalEditFormatTypes.Currency,
                        }, (decimal)ExtendedPrice);
                case OrderDetailsColumns.Discount:
                    return new DataEntryGridDecimalCellProps(this, columnId
                        , new DecimalEditControlSetup
                        {
                            FormatType = DecimalEditFormatTypes.Currency,
                        }, (decimal)Discount);
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return new DataEntryGridTextCellProps(this, columnId);
        }

        public override DataEntryGridCellStyle GetCellStyle(int columnId)
        {
            var column = (OrderDetailsColumns)columnId;
            switch (column)
            {
                case OrderDetailsColumns.ExtendedPrice:
                    return new DataEntryGridCellStyle() { State = DataEntryGridCellStates.Disabled };
            }
            return base.GetCellStyle(columnId); 
        }

        public override void SetCellValue(DataEntryGridEditingCellProps value)
        {
            var column = (OrderDetailsColumns)value.ColumnId;
            switch (column)
            {
                case OrderDetailsColumns.Product:
                    if (value is DataEntryGridAutoFillCellProps autoFillCellProps)
                    {
                        ProductAutoFillValue = autoFillCellProps.AutoFillValue;
                        SetProductValues();
                    }
                    break;
                case OrderDetailsColumns.Quantity:
                    if (value is DataEntryGridDecimalCellProps decimalCellProps)
                    {
                        Quantity = decimalCellProps.Value.GetValueOrDefault().ToDouble();
                    }
                    break;
                case OrderDetailsColumns.Price:
                    if (value is DataEntryGridDecimalCellProps decimalCellProps1)
                    {
                        UnitPrice = decimalCellProps1.Value.GetValueOrDefault().ToDouble();
                    }
                    break;
                case OrderDetailsColumns.Discount:
                    if (value is DataEntryGridDecimalCellProps decimalCellProps2)
                    {
                        Discount = decimalCellProps2.Value.GetValueOrDefault().ToDouble();
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            Manager.CalculateTotals();
            base.SetCellValue(value);
        }

        public override void LoadFromEntity(OrderDetail entity)
        {
            ProductAutoFillValue = entity.Product.GetAutoFillValue();
            Quantity = entity.Quantity;
            UnitPrice = entity.UnitPrice;
            ExtendedPrice = entity.ExtendedPrice;
            Discount = entity.Discount.GetValueOrDefault();
        }

        public override void SaveToEntity(OrderDetail entity, int rowIndex)
        {
            entity.DetailId = rowIndex;
            entity.ProductId = ProductAutoFillValue.GetEntity<Product>().Id;
            entity.Quantity = Quantity;
            entity.UnitPrice = UnitPrice;
            entity.ExtendedPrice = ExtendedPrice;
            entity.Discount = Discount;
        }

        private void SetProductValues()
        {
            if (ProductAutoFillValue.IsValid())
            {
                if (_oldAutoFillValue == null 
                    || ProductAutoFillValue.PrimaryKeyValue
                        .IsEqualTo(_oldAutoFillValue.PrimaryKeyValue) == false)
                {
                    var context = AppGlobals.DataRepository.GetDataContext();
                    var table = context.GetTable<Product>();
                    var product = ProductAutoFillValue.GetEntity<Product>();
                    product = table.FirstOrDefault(p => p.Id == product.Id);
                    if (product != null)
                    {
                        Quantity = 1;
                        UnitPrice = product.Price.GetValueOrDefault();
                    }
                    _oldAutoFillValue = ProductAutoFillValue;
                }
            }
        }
    }
}
