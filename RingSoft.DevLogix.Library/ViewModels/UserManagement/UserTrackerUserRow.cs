using System;
using System.Linq;
using MySqlX.XDevAPI.Relational;
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

        public string Status { get; private set; }

        public decimal PunchedOutMinutes { get; private set; }

        public decimal PunchedInMinutes { get; private set; }

        public override int DisplayStyleId
        {
            get => _displayStyle;
            set => _displayStyle = value;
        }

        private int _displayStyle;

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
                    if (Status.IsNullOrEmpty())
                    {
                        if (PunchedOutMinutes > 0)
                        {
                            return new TimeCostCellProps(this, columnId, PunchedOutMinutes);
                        }
                        else
                        {
                            return new DataEntryGridTextCellProps(this, columnId);
                        }
                    }
                    else
                    {
                        return new DataEntryGridTextCellProps(this, columnId, Status);
                    }
                    break;
                case UserTrackerColumns.PunchedIn:
                    if (PunchedInMinutes > 0)
                    {
                        return new TimeCostCellProps(this, columnId, PunchedInMinutes);
                    }
                    else
                    {
                        return new DataEntryGridTextCellProps(this, columnId);
                    }
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
                    if (UserId == 0 || TimeClock == null)
                    {
                        return new DataEntryGridButtonCellStyle()
                        {
                            Content = "Last Time Clock",
                            State = DataEntryGridCellStates.Disabled,
                            IsVisible = false,
                        };
                    }
                    return new DataEntryGridButtonCellStyle()
                    {
                        Content = "Last Time Clock",
                        State = DataEntryGridCellStates.Enabled,
                        IsVisible = true,
                    };
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
                        UserId = UserAutoFillValue.GetEntity<User>().Id;
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
                    var now = DateTime.Now.ToUniversalTime();
                    var userTable = context.GetTable<User>();
                    if (userTable != null)
                    {
                        var user = userTable.FirstOrDefault(p => p.Id == UserId);
                        if (user == null)
                        {
                            Status = string.Empty;
                            PunchedInMinutes = 0;
                            PunchedOutMinutes = 0;
                        }
                        else
                        {
                            if (user.ClockDate == null)
                            {
                                Status = "Clocked Out";
                                TimeClock = table.Where(p => p.UserId == UserId)
                                    .OrderBy(p => p.PunchOutDate)
                                    .LastOrDefault();
                                PunchedInMinutes = 0;
                                PunchedOutMinutes = 0;
                                DisplayStyleId = 0;
                            }
                            else if (TimeClock == null)
                            {
                                TimeClock = table.Where(p => p.UserId == UserId)
                                    .OrderBy(p => p.PunchOutDate)
                                    .LastOrDefault();

                                var timeClock = table
                                    .Where(p => p.UserId == UserId
                                                && p.PunchOutDate != null
                                                && p.PunchOutDate.Value.Year == now.Year
                                                && p.PunchOutDate.Value.Month == now.Month
                                                && p.PunchOutDate.Value.Day == now.Day)
                                    .OrderBy(p => p.PunchOutDate)
                                    .LastOrDefault();
                                if (timeClock != null)
                                {
                                    if (timeClock.PunchOutDate != null)
                                    {
                                        var timeSpan = now - timeClock.PunchOutDate.Value;
                                        PunchedOutMinutes = (decimal)timeSpan.TotalMinutes;
                                        if (PunchedOutMinutes > Manager.ViewModel.RedAlertMinutes)
                                        {
                                            DisplayStyleId = UserTrackerUserManager.RedDisplayStyleId;
                                        }
                                        else
                                        {
                                            DisplayStyleId = 0;
                                        }
                                    }
                                }

                                PunchedInMinutes = 0;
                            }
                            else
                            {
                                Status = string.Empty;
                                var timeSpan = now - TimeClock.PunchInDate;
                                PunchedInMinutes = (decimal)timeSpan.TotalMinutes;
                                if (PunchedInMinutes > Manager.ViewModel.YellowAlertMinutes)
                                {
                                    DisplayStyleId = UserTrackerUserManager.YellowDisplayStyleId;
                                }
                                else
                                {
                                    DisplayStyleId = 0;
                                }
                                PunchedOutMinutes = 0;

                            }

                        }
                    }
                }
            }
        }
    }
}
