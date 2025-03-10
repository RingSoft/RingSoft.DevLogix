﻿using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model.QualityAssurance;
using System.Collections.Generic;
using System.Linq;

namespace RingSoft.DevLogix.Library.ViewModels.QualityAssurance.Testing
{
    public enum TestingOutlineCostColumns
    {
        User = 1,
        TimeSpent = 2,
        Cost = 3,
    }

    public class TestingOutlineCostManager : DbMaintenanceDataEntryGridManager<TestingOutlineCost>
    {
        public const int UserColumnId = (int)TestingOutlineCostColumns.User;
        public const int TimeSpentColumnId = (int)TestingOutlineCostColumns.TimeSpent;
        public const int CostColumnId = (int)TestingOutlineCostColumns.Cost;

        public new TestingOutlineViewModel ViewModel { get; }
        public TestingOutlineCostManager(TestingOutlineViewModel viewModel) : base(viewModel)
        {
            ViewModel = viewModel;
        }

        protected override DataEntryGridRow GetNewRow()
        {
            throw new System.NotImplementedException();
        }

        protected override DbMaintenanceDataEntryGridRow<TestingOutlineCost> ConstructNewRowFromEntity(TestingOutlineCost entity)
        {
            return new TestingOutlineCostRow(this);
        }

        public void AddUserRow(TestingOutlineCost user)
        {
            var userRow = new TestingOutlineCostRow(this);
            userRow.LoadFromEntity(user);
            AddRow(userRow, Rows.Count - 1);
            Grid?.RefreshGridView();
        }
        public void RefreshCost(List<TestingOutlineCost> users)
        {
            ClearRows();
            LoadGrid(users);
            Grid?.RefreshGridView();
        }
        public void RefreshCost(TestingOutlineCost testOutlineUser)
        {
            var rows = Rows.OfType<TestingOutlineCostRow>();
            var userRow = rows.FirstOrDefault(p => p.UserId == testOutlineUser.UserId);
            if (userRow != null)
            {
                userRow.LoadFromEntity(testOutlineUser);
                Grid?.RefreshGridView();
            }
        }

        public void GetTotals(out double minutesSpent, out double totalCost)
        {
            var rows = Rows.OfType<TestingOutlineCostRow>();
            minutesSpent = rows.Sum(p => p.MinutesSpent);
            totalCost = rows.Sum(p => p.Cost);
        }

        //Peter Ringering - 01/09/2025 03:42:55 PM - E-95
        protected override void SelectRowForEntity(TestingOutlineCost entity)
        {
            var row = Rows.OfType<TestingOutlineCostRow>()
                .FirstOrDefault(p => p.UserId == entity.UserId);

            if (row != null)
            {
                ViewModel.View.SelectTab(SetFocusTabs.Cost);
                GotoCell(row, UserColumnId);
            }

            base.SelectRowForEntity(entity);
        }

    }
}
