using System;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public class ProjectUsersGridRow : DbMaintenanceDataEntryGridRow<ProjectUser>
    {
        public new ProjectUsersGridManager Manager { get; private set; }

        public AutoFillSetup UserAutoFillSetup { get; private set; }

        public AutoFillValue UserAutoFillValue { get; private set; }

        public int UserId { get; private set; }

        public string TimeSpent { get; private set; }

        public decimal MinutesSpent { get; private set; }

        public decimal Cost { get; private set; }

        public ProjectUsersGridRow(ProjectUsersGridManager manager) : base(manager)
        {
            Manager = manager;
            UserAutoFillSetup =
                new AutoFillSetup(AppGlobals.LookupContext.ProjectUsers.GetFieldDefinition(p => p.UserId));
            TimeSpent = AppGlobals.MakeTimeSpent(MinutesSpent);
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (ProjectUserColumns)columnId;

            switch (column)
            {
                case ProjectUserColumns.User:
                    return new DataEntryGridAutoFillCellProps(this, columnId, UserAutoFillSetup, UserAutoFillValue);
                case ProjectUserColumns.MinutesSpent:
                    return new DataEntryGridTextCellProps(this, columnId, TimeSpent);
                case ProjectUserColumns.Cost:
                    return new DataEntryGridDecimalCellProps(this, columnId, new DecimalEditControlSetup
                    {
                        FormatType = DecimalEditFormatTypes.Currency,
                    }, Cost);
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return new DataEntryGridTextCellProps(this, columnId);
        }

        public override DataEntryGridCellStyle GetCellStyle(int columnId)
        {
            var column = (ProjectUserColumns)columnId;

            switch (column)
            {
                case ProjectUserColumns.User:
                    break;
                case ProjectUserColumns.MinutesSpent:
                case ProjectUserColumns.Cost:
                    return new DataEntryGridCellStyle
                    {
                        State = DataEntryGridCellStates.Disabled,
                    };
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return base.GetCellStyle(columnId);
        }

        public override void SetCellValue(DataEntryGridEditingCellProps value)
        {
            var column = (ProjectUserColumns)value.ColumnId;

            switch (column)
            {
                case ProjectUserColumns.User:
                    if (value is DataEntryGridAutoFillCellProps autoFillCellProps)
                    {
                        UserAutoFillValue = autoFillCellProps.AutoFillValue;
                        UserId = UserAutoFillValue.GetEntity(AppGlobals.LookupContext.Users).Id;
                    }
                    break;
                case ProjectUserColumns.MinutesSpent:
                    break;
                case ProjectUserColumns.Cost:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            base.SetCellValue(value);
        }

        public override void LoadFromEntity(ProjectUser entity)
        {
            UserId = entity.UserId;
            UserAutoFillValue = UserAutoFillSetup.GetAutoFillValueForIdValue(entity.UserId);
            MinutesSpent = entity.MinutesSpent;
            TimeSpent = AppGlobals.MakeTimeSpent(entity.MinutesSpent);
            Cost = entity.Cost;
        }

        public override bool ValidateRow()
        {
            throw new System.NotImplementedException();
        }

        public override void SaveToEntity(ProjectUser entity, int rowIndex)
        {
            entity.UserId = UserAutoFillValue.GetEntity(AppGlobals.LookupContext.Users).Id;
            entity.MinutesSpent = MinutesSpent;
            entity.Cost = Cost;
        }

        public void SetUser(int userId)
        {
            UserId = userId;
            UserAutoFillValue = UserAutoFillSetup.GetAutoFillValueForIdValue(userId);
        }
    }
}
