using System;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public class ProjectTotalsRow : DataEntryGridRow
    {
        public new ProjectTotalsManager Manager { get; private set; }

        public string RowTitle { get; set; }

        public ProjectTotalsRow(ProjectTotalsManager manager) : base(manager)
        {
            Manager = manager;
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (TotalsColumns)columnId;

            switch (column)
            {
                case TotalsColumns.Type:
                    return new DataEntryGridTextCellProps(this, columnId, RowTitle);
                case TotalsColumns.Time:
                    break;
                case TotalsColumns.Cost:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return new DataEntryGridTextCellProps(this, columnId);
        }
    }
}
