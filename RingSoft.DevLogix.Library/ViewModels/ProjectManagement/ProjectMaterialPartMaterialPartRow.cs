using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;
using System.Linq;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    internal class ProjectMaterialPartMaterialPartRow : ProjectMaterialPartRow
    {
        public override MaterialPartLineTypes LineType => MaterialPartLineTypes.MaterialPart;

        public AutoFillSetup MaterialPartAutoFillSetup { get; private set; }

        public AutoFillValue MaterialPartAutoFillValue { get; private set; }

        public int Quantity { get; private set; } = 1;

        public decimal Cost { get; private set; }

        public decimal ExtendedCost { get; private set; }

        public ProjectMaterialPartMaterialPartRow(ProjectMaterialPartManager manager) : base(manager)
        {
            MaterialPartAutoFillSetup = new AutoFillSetup(AppGlobals.LookupContext.MaterialPartLookup);
        }

        public void SetAutoFillValue(AutoFillValue autoFillValue)
        {
            MaterialPartAutoFillValue = autoFillValue;
            SetCost();
        }

        private void SetCost()
        {
            if (MaterialPartAutoFillValue.IsValid())
            {
                var materialPart = MaterialPartAutoFillValue.GetEntity<MaterialPart>();
                if (materialPart != null)
                {
                    var context = AppGlobals.DataRepository.GetDataContext();
                    materialPart = context.GetTable<MaterialPart>()
                        .FirstOrDefault(p => p.Id == materialPart.Id);
                }

                if (materialPart != null)
                {
                    Cost = materialPart.Cost;
                    CalculateRow();
                }
            }
        }

        private void CalculateRow()
        {
            ExtendedCost = GetExtendedCost();
            Manager.CalculateTotalCost();
        }

        public override decimal GetExtendedCost()
        {
            return Quantity * Cost;
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (ProjectMaterialPartColumns)columnId;
            switch (column)
            {
                case ProjectMaterialPartColumns.MaterialPart:
                    return new DataEntryGridAutoFillCellProps(this, columnId, MaterialPartAutoFillSetup,
                        MaterialPartAutoFillValue);
            }
            return base.GetCellProps(columnId);
        }

        public override void LoadFromEntity(ProjectMaterialPart entity)
        {
            throw new System.NotImplementedException();
        }

        public override bool ValidateRow()
        {
            throw new System.NotImplementedException();
        }

        public override void SaveToEntity(ProjectMaterialPart entity, int rowIndex)
        {
            throw new System.NotImplementedException();
        }
    }
}
