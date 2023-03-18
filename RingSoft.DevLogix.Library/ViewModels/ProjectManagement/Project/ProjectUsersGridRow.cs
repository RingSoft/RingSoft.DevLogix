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

        public bool IsStandard { get; private set; } = true;

        public decimal SundayMinutes { get; set; }

        public decimal MondayMinutes { get; set; }

        public decimal TuesdayMinutes { get; set; }

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
                case ProjectUserColumns.IsStandard:
                    return new DataEntryGridCheckBoxCellProps(this, columnId, IsStandard);
                case ProjectUserColumns.Sunday:
                    return new TimeCostCellProps(this, columnId, SundayMinutes);
                case ProjectUserColumns.Monday:
                    return new TimeCostCellProps(this, columnId, MondayMinutes);
                case ProjectUserColumns.Tuesday:
                    return new TimeCostCellProps(this, columnId, TuesdayMinutes);
                case ProjectUserColumns.Wednesday:
                    break;
                case ProjectUserColumns.Thursday:
                    break;
                case ProjectUserColumns.Friday:
                    break;
                case ProjectUserColumns.Saturday:
                    break;
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
                case ProjectUserColumns.IsStandard:
                    return new DataEntryGridControlCellStyle
                    {

                    };
                case ProjectUserColumns.Sunday:
                case ProjectUserColumns.Monday:
                case ProjectUserColumns.Tuesday:
                case ProjectUserColumns.Wednesday:
                case ProjectUserColumns.Thursday:
                case ProjectUserColumns.Friday:
                case ProjectUserColumns.Saturday:
                    if (IsStandard)
                    {
                        return new DataEntryGridCellStyle
                        {
                            State = DataEntryGridCellStates.Disabled,
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
            var timeCellProps = value as TimeCostCellProps;

            var column = (ProjectUserColumns)value.ColumnId;

            switch (column)
            {
                case ProjectUserColumns.User:
                    if (value is DataEntryGridAutoFillCellProps autoFillCellProps)
                    {
                        UserAutoFillValue = autoFillCellProps.AutoFillValue;
                        UserId = UserAutoFillValue.GetEntity(AppGlobals.LookupContext.Users).Id;
                        SetStandardMinutes();
                    }
                    break;
                case ProjectUserColumns.MinutesSpent:
                    break;
                case ProjectUserColumns.Cost:
                    break;
                case ProjectUserColumns.IsStandard:
                    if (value is DataEntryGridCheckBoxCellProps checkBoxCellProps)
                    {
                        IsStandard = checkBoxCellProps.Value;
                        SetStandardMinutes();
                    }
                    break;

                case ProjectUserColumns.Sunday:
                    if (timeCellProps != null)
                    {
                        SundayMinutes = timeCellProps.Minutes;
                    }
                    break;
                case ProjectUserColumns.Monday:
                    if (timeCellProps != null)
                    {
                        MondayMinutes = timeCellProps.Minutes;
                    }
                    break;
                case ProjectUserColumns.Tuesday:
                    if (timeCellProps != null)
                    {
                        TuesdayMinutes = timeCellProps.Minutes;
                    }
                    break;
                case ProjectUserColumns.Wednesday:
                    break;
                case ProjectUserColumns.Thursday:
                    break;
                case ProjectUserColumns.Friday:
                    break;
                case ProjectUserColumns.Saturday:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            base.SetCellValue(value);
        }

        private void SetStandardMinutes()
        {
            if (!IsStandard)
            {
                return;
            }

            var enumTranslation = new EnumFieldTranslation();
            enumTranslation.LoadFromEnum<DayType>();
            foreach (var typeTranslation in enumTranslation.TypeTranslations)
            {
                var dayType = (DayType)typeTranslation.NumericValue;
                var standardMinutes = Manager.ViewModel.ProjectDaysGridManager.GetStandardMinutes(dayType);
                switch (dayType)
                {
                    case DayType.Sunday:
                        SundayMinutes = standardMinutes;
                        break;
                    case DayType.Monday:
                        MondayMinutes = standardMinutes;
                        break;
                    case DayType.Tuesday:
                        TuesdayMinutes = standardMinutes;   
                        break;
                    case DayType.Wednesday:
                        break;
                    case DayType.Thursday:
                        break;
                    case DayType.Friday:
                        break;
                    case DayType.Saturday:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public override void LoadFromEntity(ProjectUser entity)
        {
            UserId = entity.UserId;
            UserAutoFillValue = entity.User.GetAutoFillValue();
            MinutesSpent = entity.MinutesSpent;
            TimeSpent = AppGlobals.MakeTimeSpent(entity.MinutesSpent);
            Cost = entity.Cost;
            IsStandard = entity.IsStandard;
            if (IsStandard)
            {
                SetStandardMinutes();
            }
            else
            {
                SundayMinutes = entity.SundayMinutes.GetValueOrDefault();
                MondayMinutes = entity.MondayMinutes.GetValueOrDefault();
                TuesdayMinutes = entity.TuesdayMinutes.GetValueOrDefault();
            }
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
            entity.IsStandard = IsStandard;
            //if (!IsStandard)
            {
                entity.SundayMinutes = SundayMinutes;
                entity.MondayMinutes = MondayMinutes;
                entity.TuesdayMinutes = TuesdayMinutes;
            }
        }
    }
}
