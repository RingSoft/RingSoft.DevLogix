using System.Linq;
using MySqlX.XDevAPI.Relational;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model.CustomerManagement;
using RingSoft.DevLogix.Library.ViewModels.UserManagement;

namespace RingSoft.DevLogix.Library.ViewModels.CustomerManagement
{
    public enum OrderDetailsColumns
    {
        Product = 1,
        Quantity = 2,
        Price = 3,
        ExtendedPrice = 4,
        Discount = 5,
    }

    public class OrderDetailsManager : DbMaintenanceDataEntryGridManager<OrderDetail>
    {
        public const int ProductColumnId = (int)OrderDetailsColumns.Product;
        public const int QuantityColumnId = (int)OrderDetailsColumns.Quantity;
        public const int PriceColumnId = (int)OrderDetailsColumns.Price;
        public const int ExtendedColumnId = (int)OrderDetailsColumns.ExtendedPrice;
        public const int DiscountColumnId = (int)OrderDetailsColumns.Discount;

        public new OrderViewModel ViewModel { get; }

        public OrderDetailsManager(OrderViewModel viewModel) : base(viewModel)
        {
            ViewModel = viewModel;
        }

        protected override DataEntryGridRow GetNewRow()
        {
            return new OrderDetailsRow(this);
        }

        protected override DbMaintenanceDataEntryGridRow<OrderDetail> ConstructNewRowFromEntity(OrderDetail entity)
        {
            return new OrderDetailsRow(this);
        }

        public void CalculateTotals()
        {
            var subTotal = (decimal)0;
            var totalDiscount = (decimal)0;
            var rows = Rows.OfType<OrderDetailsRow>();
            foreach (var row in rows)
            {
                row.ExtendedPrice = row.Quantity * row.UnitPrice;
                subTotal += row.ExtendedPrice;
                totalDiscount += row.Discount;
            }

            ViewModel.SubTotal = subTotal;
            ViewModel.TotalDiscount = totalDiscount;
            ViewModel.Total = (subTotal + ViewModel.Freight) - totalDiscount;
        }
    }
}