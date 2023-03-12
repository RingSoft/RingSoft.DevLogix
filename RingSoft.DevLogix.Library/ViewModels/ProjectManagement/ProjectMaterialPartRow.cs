using System;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;
using System.ComponentModel;
using System.Linq;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public enum MaterialPartLineTypes
    {
        [Description("New Row")]
        NewRow = 0,
        [Description("Material Part")]
        MaterialPart = 1,
        [Description("Miscellaneous")]
        Miscellaneous = 2,
        [Description("Overhead")]
        Overhead = 3,
        [Description("Comment")]
        Comment = 4,
    }

    public abstract class ProjectMaterialPartRow : DbMaintenanceDataEntryGridRow<ProjectMaterialPart>
    {
        public abstract MaterialPartLineTypes LineType { get; }

        public new ProjectMaterialPartManager Manager { get; private set; }

        public EnumFieldTranslation EnumTranslation { get; private set; } = new EnumFieldTranslation();

        protected ProjectMaterialPartRow(ProjectMaterialPartManager manager) : base(manager)
        {
            Manager = manager;
            EnumTranslation.LoadFromEnum<MaterialPartLineTypes>();
        }

        public abstract decimal GetExtendedCost();

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (ProjectMaterialPartColumns)columnId;
            switch (column)
            {
                case ProjectMaterialPartColumns.LineType:
                    return new DataEntryGridTextCellProps(this, columnId, EnumTranslation.TypeTranslations
                        .FirstOrDefault(p => p.NumericValue == (int)LineType).TextValue);
            }
            return new DataEntryGridTextCellProps(this, columnId);
        }

        public override DataEntryGridCellStyle GetCellStyle(int columnId)
        {
            var column = (ProjectMaterialPartColumns)columnId;

            switch (column)
            {
                case ProjectMaterialPartColumns.LineType:
                case ProjectMaterialPartColumns.ExtendedCost:
                    return new DataEntryGridCellStyle
                    {
                        State = DataEntryGridCellStates.Disabled
                    };
                case ProjectMaterialPartColumns.MaterialPart:
                    break;
                case ProjectMaterialPartColumns.Quantity:
                    break;
                case ProjectMaterialPartColumns.Cost:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return base.GetCellStyle(columnId);
        }
    }
}
