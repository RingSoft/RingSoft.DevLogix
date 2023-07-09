using System.Linq;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;

namespace RingSoft.DevLogix.Library.ViewModels.UserManagement
{
    public enum UserBillabilityColumns
    {
        Name = 0,
        TimeSpent = 1,
        Billability = 2,
    }

    public enum UserBillabilityRows
    {
        BillableProjects = 0,
        NonBillableProjects = 1,
        Errors = 2,
        TestingOutlines = 3,
        Customers = 4,
        Support = 5,
    }
    public class UserBillabilityGridManager : DataEntryGridManager
    {
        public const int NameColumnId = (int)UserBillabilityColumns.Name;
        public const int TimeSpentColumnId = (int)UserBillabilityColumns.TimeSpent;
        public const int BillabilityColumnId = (int)UserBillabilityColumns.Billability;

        public UserMaintenanceViewModel ViewModel { get; private set; }

        public UserBillabilityGridManager(UserMaintenanceViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        protected override DataEntryGridRow GetNewRow()
        {
            throw new System.NotImplementedException();
        }

        public void MakeGrid()
        {
            var billableProjectsRow = new UserBillabilityGridRow(this, UserBillabilityRows.BillableProjects);
            AddRow(billableProjectsRow);

            billableProjectsRow = new UserBillabilityGridRow(this, UserBillabilityRows.NonBillableProjects);
            AddRow(billableProjectsRow);

            billableProjectsRow = new UserBillabilityGridRow(this, UserBillabilityRows.Errors);
            AddRow(billableProjectsRow);

            var testingOutlineRow = new UserBillabilityGridRow(this, UserBillabilityRows.TestingOutlines);
            AddRow(testingOutlineRow);

            var customersRow = new UserBillabilityGridRow(this, UserBillabilityRows.Customers);
            AddRow(customersRow);

            var supportRow = new UserBillabilityGridRow(this, UserBillabilityRows.Support);
            AddRow(supportRow);

            Grid?.RefreshGridView();
        }

        public void SetRowValues(UserBillabilityRows rowType, double minutesSpent, double billability)
        {
            var rows = Rows.OfType<UserBillabilityGridRow>();
            var row = rows.FirstOrDefault(p => p.RowType == rowType);
            if (row != null)
            {
                row.SetRowValues(minutesSpent, billability);
            }
            Grid?.RefreshGridView();
        }

        public double GetMinutesSpent(UserBillabilityRows rowType)
        {
            var rows = Rows.OfType<UserBillabilityGridRow>();
            var row = rows.FirstOrDefault(p => p.RowType == rowType);
            if (row != null)
            {
                return row.MinutesSpent;
            }

            return 0;
        }
    }
}
