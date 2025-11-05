using System.Collections.Generic;
using System.Linq;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model.CustomerManagement;

namespace RingSoft.DevLogix.Library.ViewModels.CustomerManagement
{
    public enum CustomerProductColumns
    {
        Product = 1,
        ExpirationDate = 2,
    }
    public class CustomerProductManager(CustomerViewModel viewModel)
        : DbMaintenanceDataEntryGridManager<CustomerProduct>(viewModel)
    {
        public const int ProductColumnId = (int)CustomerProductColumns.Product;
        public const int ExpirationDateColumnId = (int)CustomerProductColumns.ExpirationDate;

        public new CustomerViewModel ViewModel { get; } = viewModel;

        protected override DataEntryGridRow GetNewRow()
        {
            return new CustomerProductRow(this);
        }

        protected override DbMaintenanceDataEntryGridRow<CustomerProduct> ConstructNewRowFromEntity(CustomerProduct entity)
        {
            return new CustomerProductRow(this);
        }

        public CustomerProductRow? GetCustomerProductRow(int productId)
        {
            var result = Rows.OfType<CustomerProductRow>()
                .FirstOrDefault(p => p.ProductId == productId);
            return result;
        }

        protected override void SelectRowForEntity(CustomerProduct entity)
        {
            var productRow = GetCustomerProductRow(entity.ProductId);
            if (productRow != null)
            {
                ViewModel.View.SelectGrid(CustomerGrids.Products);
                GotoCell(productRow, ProductColumnId);
            }
            base.SelectRowForEntity(entity);
        }

        public override void LoadGrid(IEnumerable<CustomerProduct> entityList)
        {
            base.LoadGrid(entityList);
        }
    }
}
