using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public enum ProjectTaskLaborPartColumns
    {
        LineType = 1,
        LaborPart = 2,
        Quantity = 3,
        MinutesCost = 4,
        ExtendedMinutes = 5,
        Complete = 6,
        PercentComplete = 7,
    }

    public enum DisplayModes
    {
        All = 0,
        User = 1,
        Disabled = 2,
    }
    public class ProjectTaskLaborPartsManager : DbMaintenanceDataEntryGridManager<ProjectTaskLaborPart>
    {
        public const int LineTypeColumnId = (int)ProjectTaskLaborPartColumns.LineType;
        public const int LaborPartColumnId = (int)ProjectTaskLaborPartColumns.LaborPart;
        public const int QuantityColumnId = (int)ProjectTaskLaborPartColumns.Quantity;
        public const int MinutesCostColumnId = (int)ProjectTaskLaborPartColumns.MinutesCost;
        public const int ExtendedMinutesColumnId = (int)ProjectTaskLaborPartColumns.ExtendedMinutes;
        public const int CompleteColumnId = (int)ProjectTaskLaborPartColumns.Complete;
        public const int PercentCompleteColumnId = (int)ProjectTaskLaborPartColumns.PercentComplete;

        public const int MiscRowDisplayStyleId = 100;
        public const int CommentRowDisplayStyleId = 101;

        public double TotalMinutes { get; private set; }

        public IEnumerable<ProjectTaskLaborPart> Details { get; private set; }

        public new ProjectTaskViewModel ViewModel { get; private set; }

        public DisplayModes DisplayMode { get; private set; }

        public ProjectTaskLaborPartsManager(ProjectTaskViewModel viewModel) : base(viewModel)
        {
            ViewModel = viewModel;
        }

        protected override DataEntryGridRow GetNewRow()
        {
            return new ProjectTaskLaborPartNewRow(this);
        }

        protected override DbMaintenanceDataEntryGridRow<ProjectTaskLaborPart> 
            ConstructNewRowFromEntity(ProjectTaskLaborPart entity)
        {
            var lineType = (LaborPartLineTypes)entity.LineType;
            return ConstructRowFromLineType(lineType);

        }

        public ProjectTaskLaborPartRow ConstructRowFromLineType(LaborPartLineTypes lineType)
        {
            switch (lineType)
            {
                case LaborPartLineTypes.NewRow:
                    break;
                case LaborPartLineTypes.LaborPart:
                    return new ProjectTaskLaborPartLaborPartRow(this);
                case LaborPartLineTypes.Miscellaneous:
                    return new ProjectTaskLaborPartMiscRow(this);
                case LaborPartLineTypes.Comment:
                    return new ProjectTaskLaborPartCommentRow(this);
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return new ProjectTaskLaborPartLaborPartRow(this);
        }

        public void CalculateTotalMinutesCost()
        {
            
            var rows = Rows.OfType<ProjectTaskLaborPartRow>().ToList();
            var total = rows.Sum(p => p.GetExtendedMinutesCost());
            TotalMinutes = total;
            ViewModel.SetTotalMinutesCost(total);
        }

        protected override void OnRowsChanged(NotifyCollectionChangedEventArgs e)
        {
            CalculateTotalMinutesCost();
            base.OnRowsChanged(e);
        }

        public override void LoadGrid(IEnumerable<ProjectTaskLaborPart> entityList)
        {
            Details = entityList;
            base.LoadGrid(entityList);
        }

        protected override string GetParentRowIdFromEntity(ProjectTaskLaborPart entity)
        {
            return entity.ParentRowId;
        }

        public void CalcPercentComplete()
        {
            var rows = Rows.OfType<ProjectTaskLaborPartRow>()
                .Where(p => p.IsNew == false)
                .ToList();

            //var minutesComplete = rows.Sum(p => p.GetExtendedMinutesCost());
            var minutesComplete = (double)0;
            foreach (var row in rows)
            {
                if (row.IsComplete)
                {
                    row.PercentComplete = 1;
                }
                var rowMinutesComplete = row.GetExtendedMinutesCost() * row.PercentComplete;
                minutesComplete += rowMinutesComplete;
            }
            if (ViewModel.MinutesCost > 0 && minutesComplete > 0)
            {
                var percentComplete = minutesComplete / ViewModel.MinutesCost;
                //if (ViewModel.PercentComplete < percentComplete)
                {
                    ViewModel.PercentComplete = percentComplete;
                }
            }
        }

        public void SetDisplayMode(DisplayModes value)
        {
            DisplayMode = value;
        }

        protected override bool CanInsertRow(int startIndex)
        {
            switch (DisplayMode)
            {
                case DisplayModes.All:
                    break;
                case DisplayModes.User:
                case DisplayModes.Disabled:
                    return false;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return base.CanInsertRow(startIndex);
        }

        public override bool IsDeleteOk(int rowIndex)
        {
            switch (DisplayMode)
            {
                case DisplayModes.All:
                    break;
                case DisplayModes.User:
                case DisplayModes.Disabled:
                    return false;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return base.IsDeleteOk(rowIndex);
        }
    }
}
