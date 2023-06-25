﻿using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model.CustomerManagement;

namespace RingSoft.DevLogix.Library.ViewModels.CustomerManagement
{
    public enum CustomerProductColumns
    {
        Product = 1,
        ExpirationDate = 2,
    }
    public class CustomerProductManager : DbMaintenanceDataEntryGridManager<CustomerProduct>
    {
        public const int ProductColumnId = (int)CustomerProductColumns.Product;
        public const int ExpirationDateColumnId = (int)CustomerProductColumns.ExpirationDate;

        public new CustomerViewModel ViewModel { get; }
        public CustomerProductManager(CustomerViewModel viewModel) : base(viewModel)
        {
            ViewModel = viewModel;
        }

        protected override DataEntryGridRow GetNewRow()
        {
            return new CustomerProductRow(this);
        }

        protected override DbMaintenanceDataEntryGridRow<CustomerProduct> ConstructNewRowFromEntity(CustomerProduct entity)
        {
            return new CustomerProductRow(this);
        }
    }
}
