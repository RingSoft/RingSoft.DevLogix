﻿using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model.QualityAssurance;

namespace RingSoft.DevLogix.Library.ViewModels.QualityAssurance
{
    public class ErrorUserRow : DbMaintenanceDataEntryGridRow<ErrorUser>
    {
        public new ErrorUserGridManager Manager { get; private set; }

        public ErrorUserRow(ErrorUserGridManager manager) : base(manager)
        {
            Manager = manager;
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            return new DataEntryGridTextCellProps(this, columnId);
        }

        public override void LoadFromEntity(ErrorUser entity)
        {
            throw new System.NotImplementedException();
        }

        public override bool ValidateRow()
        {
            throw new System.NotImplementedException();
        }

        public override void SaveToEntity(ErrorUser entity, int rowIndex)
        {
            throw new System.NotImplementedException();
        }
    }
}
