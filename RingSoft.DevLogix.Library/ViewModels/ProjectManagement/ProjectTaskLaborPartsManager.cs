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
    }
    public class ProjectTaskLaborPartsManager : DbMaintenanceDataEntryGridManager<ProjectTaskLaborPart>
    {
        public const int LineTypeColumnId = (int)ProjectTaskLaborPartColumns.LineType;
        public const int LaborPartColumnId = (int)ProjectTaskLaborPartColumns.LaborPart;
        public const int QuantityColumnId = (int)ProjectTaskLaborPartColumns.Quantity;
        public const int MinutesCostColumnId = (int)ProjectTaskLaborPartColumns.MinutesCost;
        public const int ExtendedMinutesColumnId = (int)ProjectTaskLaborPartColumns.ExtendedMinutes;

        public const int MiscRowDisplayStyleId = 100;
        public const int CommentRowDisplayStyleId = 101;

        public new ProjectTaskViewModel ViewModel { get; private set; }

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
            ViewModel.TotalMinutesCost = total;
        }

        protected override void OnRowsChanged(NotifyCollectionChangedEventArgs e)
        {
            CalculateTotalMinutesCost();
            base.OnRowsChanged(e);
        }
    }
}
