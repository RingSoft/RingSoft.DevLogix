﻿using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model;

namespace RingSoft.DevLogix.Library.ViewModels.UserManagement
{
    public class GroupsUsersGridRow : DbMaintenanceDataEntryGridRow<UsersGroup>
    {
        public new GroupsUsersManager Manager { get; set; }

        public AutoFillSetup AutoFillSetup { get; set; }

        public AutoFillValue AutoFillValue { get; set; }

        public GroupsUsersGridRow(GroupsUsersManager manager) : base(manager)
        {
            Manager = manager;
            AutoFillSetup = new AutoFillSetup(AppGlobals.LookupContext
                .UsersGroups.GetFieldDefinition(p => p.UserId))
            {
                AllowLookupAdd = AppGlobals.LookupContext.Users.HasRight(RightTypes.AllowAdd),
                AllowLookupView = AppGlobals.LookupContext.Users.HasRight(RightTypes.AllowView)
            };
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            return new DataEntryGridAutoFillCellProps(this, columnId, AutoFillSetup, AutoFillValue);
        }

        public override void SetCellValue(DataEntryGridEditingCellProps value)
        {
            if (value is DataEntryGridAutoFillCellProps autoFillCellProps)
            {
                AutoFillValue = autoFillCellProps.AutoFillValue;
            }
            base.SetCellValue(value);
        }

        public override void LoadFromEntity(UsersGroup entity)
        {
            AutoFillValue = entity.User.GetAutoFillValue();
        }

        public override void SaveToEntity(UsersGroup entity, int rowIndex)
        {
            entity.UserId = AutoFillValue.GetEntity<User>().Id;

        }
    }
}
