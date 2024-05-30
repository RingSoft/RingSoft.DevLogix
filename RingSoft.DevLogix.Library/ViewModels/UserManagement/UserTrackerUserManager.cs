using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model;
using RingSoft.DevLogix.DataAccess.Model.UserManagement;
using System;
using System.Linq;
using RingSoft.DbLookup;

namespace RingSoft.DevLogix.Library.ViewModels.UserManagement
{
    public enum UserTrackerColumns
    {
        User = 1,
        PunchedOut = 2,
        PunchedIn = 3,
        TimeClock = 4,
        DisableBalloon = 5,
    }
    public class UserTrackerUserManager : DbMaintenanceDataEntryGridManager<UserTrackerUser>
    {
        public const int UserColumnId = (int)UserTrackerColumns.User;
        public const int PunchedOutColumnId = (int)UserTrackerColumns.PunchedOut;
        public const int PunchedInColumnId = (int)UserTrackerColumns.PunchedIn;
        public const int TimeClockColumnId = (int)UserTrackerColumns.TimeClock;
        public const int DisableBalloonColumnId = (int)UserTrackerColumns.DisableBalloon;

        public const int YellowDisplayStyleId = 500;
        public const int RedDisplayStyleId = 501;

        public new UserTrackerViewModel ViewModel { get; }

        public EnumFieldTranslation ClockOutReasons { get; }

        public UserTrackerUserManager(UserTrackerViewModel viewModel) : base(viewModel)
        {
            ViewModel = viewModel;
            ClockOutReasons = new EnumFieldTranslation();
            ClockOutReasons.LoadFromEnum<ClockOutReasons>();
        }

        protected override DataEntryGridRow GetNewRow()
        {
            return new UserTrackerUserRow(this);
        }

        protected override DbMaintenanceDataEntryGridRow<UserTrackerUser> ConstructNewRowFromEntity(UserTrackerUser entity)
        {
            return new UserTrackerUserRow(this);
        }

        public void RefreshGrid()
        {
            var rows = Rows.OfType<UserTrackerUserRow>()
                .Where(p => p.IsNew == false);
            foreach (var row in rows)
            {
                row.RefreshRow();
            }

            if (rows.Count() > 0 && Grid != null)
            {
                ViewModel.View.RefreshGrid();
            }
        }

        public override void RaiseDirtyFlag()
        {
            if (Grid != null)
            {
                if (Grid.CurrentColumnId == UserTrackerUserManager.DisableBalloonColumnId)
                {
                    return;
                }
            }
            base.RaiseDirtyFlag();
        }

        public string MakeClockOutText(User user)
        {
            var result = string.Empty;
            if (user != null)
            {
                var typeTran =
                    ClockOutReasons.TypeTranslations.FirstOrDefault(p => p.NumericValue == user.ClockOutReason);
                var nowDate = GblMethods.NowDate().ToUniversalTime();
                if (typeTran != null)
                {
                    var clockText = typeTran.TextValue;
                    var clockReason = (ClockOutReasons)user.ClockOutReason;
                    if (clockReason == DataAccess.Model.ClockOutReasons.Other)
                    {
                        clockText = user.OtherClockOutReason;
                    }
                    var timeSpan = nowDate - user.ClockDate;
                    if (timeSpan == null)
                    {
                        result = clockText;
                    }
                    else
                    {
                        result = $"{clockText} - {AppGlobals.MakeTimeSpent((double)timeSpan.Value.TotalMinutes)}";
                    }
                }
            }
            return result;
        }
    }
}
