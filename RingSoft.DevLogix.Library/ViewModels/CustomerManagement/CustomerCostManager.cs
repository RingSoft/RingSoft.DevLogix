﻿using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model.CustomerManagement;
using System.Collections.Generic;
using System.Linq;

namespace RingSoft.DevLogix.Library.ViewModels.CustomerManagement
{
    public enum CustomerUserColumns
    {
        User = 1,
        TimeSpent = 2,
        Cost = 3,
    }

    public class CustomerCostManager : DbMaintenanceDataEntryGridManager<CustomerUser>
    {
        public const int UserColumnId = (int)CustomerUserColumns.User;
        public const int TimeSpentColumnId = (int)CustomerUserColumns.TimeSpent;
        public const int CostColumnId = (int)CustomerUserColumns.Cost;

        public new CustomerViewModel ViewModel { get; }

        public CustomerCostManager(CustomerViewModel viewModel) : base(viewModel)
        {
            ViewModel = viewModel;
        }

        public void AddUserRow(CustomerUser user)
        {
            var userRow = new CustomerCostRow(this);
            userRow.LoadFromEntity(user);
            AddRow(userRow, Rows.Count - 1);
        }

        protected override DbMaintenanceDataEntryGridRow<CustomerUser> ConstructNewRowFromEntity(CustomerUser entity)
        {
            return new CustomerCostRow(this);
        }

        protected override DataEntryGridRow GetNewRow()
        {
            throw new System.NotImplementedException();
        }

        public void RefreshCost(List<CustomerUser> users)
        {
            ClearRows();
            LoadGrid(users);
            Grid?.RefreshGridView();
        }
        public void RefreshCost(CustomerUser customerUser)
        {
            var rows = Rows.OfType<CustomerCostRow>();
            var userRow = rows.FirstOrDefault(p => p.UserId == customerUser.UserId);
            if (userRow != null)
            {
                userRow.LoadFromEntity(customerUser);
                Grid?.RefreshGridView();
            }
        }

        public void GetTotals(out double minutesSpent, out double totalCost)
        {
            var rows = Rows.OfType<CustomerCostRow>();
            minutesSpent = rows.Sum(p => p.MinutesSpent);
            totalCost = rows.Sum(p => p.Cost);
        }

    }
}
