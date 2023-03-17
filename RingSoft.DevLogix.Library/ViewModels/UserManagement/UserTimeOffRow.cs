using System;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model.UserManagement;

namespace RingSoft.DevLogix.Library.ViewModels.UserManagement
{
    public class UserTimeOffRow : DbMaintenanceDataEntryGridRow<UserTimeOff>
    {
        public new UserTimeOffGridManager Manager { get; private set; }

        public DateTime? StartDateTime { get; private set; }

        public DateTime? EndDateTime { get; private set; }

        public string Description { get; private set; }

        public UserTimeOffRow(UserTimeOffGridManager manager) : base(manager)
        {
            Manager = manager;
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (UserTimeOffColumns)columnId;
            switch (column)
            {
                case UserTimeOffColumns.StartDate:
                    return new DataEntryGridDateCellProps(this, columnId, new DateEditControlSetup
                    {
                        DateFormatType = DateFormatTypes.DateTime,
                    }, StartDateTime);
                case UserTimeOffColumns.EndDate:
                    return new DataEntryGridDateCellProps(this, columnId, new DateEditControlSetup
                    {
                        DateFormatType = DateFormatTypes.DateTime,
                    }, EndDateTime);

                case UserTimeOffColumns.Description:
                    return new DataEntryGridTextCellProps(this, columnId, Description);
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return new DataEntryGridTextCellProps(this, columnId);
        }

        public override void SetCellValue(DataEntryGridEditingCellProps value)
        {
            var column = (UserTimeOffColumns)value.ColumnId;

            switch (column)
            {
                case UserTimeOffColumns.StartDate:
                    if (value is DataEntryGridDateCellProps dateCellProps)
                    {
                        StartDateTime = dateCellProps.Value;
                    }
                    break;
                case UserTimeOffColumns.EndDate:
                    if (value is DataEntryGridDateCellProps endDateCellProps)
                    {
                        EndDateTime = endDateCellProps.Value;
                    }
                    break;
                case UserTimeOffColumns.Description:
                    if (value is DataEntryGridTextCellProps textCellProps)
                    {
                        Description = textCellProps.Text;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            base.SetCellValue(value);
        }

        public override void LoadFromEntity(UserTimeOff entity)
        {
            StartDateTime = entity.StartDate.ToLocalTime();
            EndDateTime = entity.EndDate.ToLocalTime();
            Description = entity.Description;
        }

        public override bool ValidateRow()
        {
            var caption = "Validation Failure";
            if (StartDateTime == null)
            {
                Manager.ViewModel.View.OnValGridFail(UserGrids.TimeOff);
                var message = "Start Date has an invalid value";
                ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                Manager.Grid?.GotoCell(this, (int)UserTimeOffColumns.StartDate);
                return false;
            }

            if (EndDateTime == null)
            {
                Manager.ViewModel.View.OnValGridFail(UserGrids.TimeOff);
                var message = "End Date has an invalid value";
                ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                Manager.Grid?.GotoCell(this, (int)UserTimeOffColumns.EndDate);
                return false;
            }

            if (StartDateTime.Value > EndDateTime.Value)
            {
                Manager.ViewModel.View.OnValGridFail(UserGrids.TimeOff);
                var message = "Start Date can not come after End Date";
                ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                Manager.Grid?.GotoCell(this, (int)UserTimeOffColumns.StartDate);
                return false;
            }


            if (Description.IsNullOrEmpty())
            {
                Manager.ViewModel.View.OnValGridFail(UserGrids.TimeOff);
                var message = "Description has an invalid value";
                ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                Manager.Grid?.GotoCell(this, (int)UserTimeOffColumns.Description);
                return false;
            }

            return true;
        }

        public override void SaveToEntity(UserTimeOff entity, int rowIndex)
        {
            entity.RowId = rowIndex;
            entity.StartDate = StartDateTime.Value.ToUniversalTime();
            entity.EndDate = EndDateTime.Value.ToUniversalTime();
            entity.Description = Description;
        }
    }
}
