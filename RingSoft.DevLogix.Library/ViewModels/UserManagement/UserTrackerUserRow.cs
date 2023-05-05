using System;
using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model;
using RingSoft.DevLogix.DataAccess.Model.UserManagement;

namespace RingSoft.DevLogix.Library.ViewModels.UserManagement
{
    public class UserTrackerUserRow : DbMaintenanceDataEntryGridRow<UserTrackerUser>
    {
        public new UserTrackerUserManager Manager { get; }

        public AutoFillSetup UserAutoFillSetup { get; }

        public AutoFillValue UserAutoFillValue { get; private set; }

        public int UserId { get; private set; }

        public TimeClock TimeClock { get; private set; }

        public UserTrackerUserRow(UserTrackerUserManager manager) : base(manager)
        {
            Manager = manager;
            UserAutoFillSetup = new AutoFillSetup(AppGlobals.LookupContext.UserLookup);
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (UserTrackerColumns)columnId;
            switch (column)
            {
                case UserTrackerColumns.User:
                    return new DataEntryGridAutoFillCellProps(this, columnId, UserAutoFillSetup, UserAutoFillValue);
                case UserTrackerColumns.PunchedOut:
                    break;
                case UserTrackerColumns.PunchedIn:
                    break;
                case UserTrackerColumns.TimeClock:
                    return new DataEntryGridButtonCellProps(this, columnId);
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return new DataEntryGridTextCellProps(this, columnId);
        }

        public override DataEntryGridCellStyle GetCellStyle(int columnId)
        {
            var column = (UserTrackerColumns)columnId;

            switch (column)
            {
                case UserTrackerColumns.User:
                    break;
                case UserTrackerColumns.PunchedOut:
                case UserTrackerColumns.PunchedIn:
                    return new DataEntryGridCellStyle()
                    {
                        State = DataEntryGridCellStates.ReadOnly,
                    };
                    break;
                case UserTrackerColumns.TimeClock:
                    if (TimeClock == null)
                    {
                        return new DataEntryGridButtonCellStyle()
                        {
                            State = DataEntryGridCellStates.ReadOnly,
                            IsVisible = false,
                        };

                    }
                    else
                    {
                        return new DataEntryGridButtonCellStyle()
                        {
                            Content = "Time Clock",
                            State = DataEntryGridCellStates.Enabled,
                            IsVisible = true,
                        };
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return base.GetCellStyle(columnId);
        }

        public override void SetCellValue(DataEntryGridEditingCellProps value)
        {
            var column = (UserTrackerColumns)value.ColumnId;
            switch (column)
            {
                case UserTrackerColumns.User:
                    if (value is DataEntryGridAutoFillCellProps autoFillCellProps)
                    {
                        UserAutoFillValue = autoFillCellProps.AutoFillValue;
                        if (UserAutoFillValue.IsValid())
                        {
                            UserId = UserAutoFillValue.GetEntity<User>().Id;
                        }
                        RefreshRow();
                        Manager.Grid?.RefreshGridView();
                    }
                    break;
                case UserTrackerColumns.PunchedOut:
                    break;
                case UserTrackerColumns.PunchedIn:
                    break;
                case UserTrackerColumns.TimeClock:
                    if (value is DataEntryGridButtonCellProps buttonCellProps)
                    {
                        var timeClockPrimaryKey = AppGlobals.LookupContext.TimeClocks
                            .GetPrimaryKeyValueFromEntity(TimeClock);
                        AppGlobals.LookupContext.TimeClockLookup.ShowAddOnTheFlyWindow(timeClockPrimaryKey);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            base.SetCellValue(value);
        }

        public override void LoadFromEntity(UserTrackerUser entity)
        {
            UserAutoFillValue = entity.User.GetAutoFillValue();
            UserId = entity.UserId;
        }

        public override bool ValidateRow()
        {
            throw new System.NotImplementedException();
        }

        public override void SaveToEntity(UserTrackerUser entity, int rowIndex)
        {
            entity.UserId = UserId;
        }

        public void RefreshRow()
        {
            if (!UserAutoFillValue.IsValid())
                return;

            var userId = UserAutoFillValue.GetEntity<User>();
            var context = AppGlobals.DataRepository.GetDataContext();
            if (context != null)
            {
                var table = context.GetTable<TimeClock>();
                if (table != null)
                {
                    TimeClock = table
                        .FirstOrDefault(p => p.UserId == UserId
                                             && p.PunchOutDate == null);
                }
            }
        }
    }
}
