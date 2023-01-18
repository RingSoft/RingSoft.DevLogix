using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model;

namespace RingSoft.DevLogix.Library.ViewModels.QualityAssurance
{
    public class ErrorQaRow : DbMaintenanceDataEntryGridRow<ErrorQa>
    {
        public new ErrorQaManager Manager { get; private set; }

        public ErrorQaRow(ErrorQaManager manager) : base(manager)
        {
            Manager = manager;
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            return new DataEntryGridTextCellProps(this, columnId);
        }

        public override void LoadFromEntity(ErrorQa entity)
        {
            throw new System.NotImplementedException();
        }

        public override bool ValidateRow()
        {
            throw new System.NotImplementedException();
        }

        public override void SaveToEntity(ErrorQa entity, int rowIndex)
        {
            throw new System.NotImplementedException();
        }
    }
}
