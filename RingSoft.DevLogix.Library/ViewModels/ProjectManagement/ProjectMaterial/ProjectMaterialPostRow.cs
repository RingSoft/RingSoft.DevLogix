using System;
using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public class ProjectMaterialPostRow : DataEntryGridRow
    {
        public new ProjectMaterialPostManager Manager { get; private set; }

        public DateTime Date { get; private set;  } = DateTime.Now;

        public AutoFillSetup MaterialAutoFillSetup { get; private set; }

        public AutoFillValue MaterialAutoFillValue { get; private set; }

        public decimal Quantity { get; private set; } = 1;

        public decimal Cost { get; private set; }

        public ProjectMaterialPostRow(ProjectMaterialPostManager manager) : base(manager)
        {
            Manager = manager;
            MaterialAutoFillSetup = new AutoFillSetup(AppGlobals.LookupContext.ProjectMaterialLookup);
            var projectField = AppGlobals.LookupContext.ProjectMaterials.GetFieldDefinition(p => p.ProjectId);
            var project = manager.ViewModel.ProjectAutoFillValue.GetEntity<Project>();
            MaterialAutoFillSetup.LookupDefinition.FilterDefinition.AddFixedFilter(projectField, Conditions.Equals,
                project.Id);
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (ProjectMaterialPostColumns)columnId;
            switch (column)
            {
                case ProjectMaterialPostColumns.Date:
                    return new DataEntryGridDateCellProps(this, columnId, new DateEditControlSetup
                    {
                        DateFormatType = DateFormatTypes.DateTime,
                        AllowNullValue = false,
                    }, Date);
                case ProjectMaterialPostColumns.Material:
                    return new DataEntryGridAutoFillCellProps(this, columnId, MaterialAutoFillSetup,
                        MaterialAutoFillValue);
                case ProjectMaterialPostColumns.Quantity:
                    return new DataEntryGridDecimalCellProps(this, columnId, new DecimalEditControlSetup
                    {
                        AllowNullValue = false,
                        FormatType = DecimalEditFormatTypes.Number,
                    }, Quantity);
                case ProjectMaterialPostColumns.Cost:
                    return new DataEntryGridDecimalCellProps(this, columnId, new DecimalEditControlSetup
                    {
                        AllowNullValue = false,
                        FormatType = DecimalEditFormatTypes.Currency,
                    }, Cost);

                case ProjectMaterialPostColumns.Extended:
                    return new DataEntryGridDecimalCellProps(this, columnId, new DecimalEditControlSetup
                    {
                        AllowNullValue = false,
                        FormatType = DecimalEditFormatTypes.Currency,
                    }, Quantity * Cost);
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return new DataEntryGridTextCellProps(this, columnId);
        }

        public override DataEntryGridCellStyle GetCellStyle(int columnId)
        {
            var column = (ProjectMaterialPostColumns)columnId;
            switch (column)
            {
                case ProjectMaterialPostColumns.Extended:
                    return new DataEntryGridCellStyle
                    {
                        State = DataEntryGridCellStates.Disabled
                    };
            }
            return base.GetCellStyle(columnId);
        }

        public override void SetCellValue(DataEntryGridEditingCellProps value)
        {
            var column = (ProjectMaterialPostColumns)value.ColumnId;
            switch (column)
            {
                case ProjectMaterialPostColumns.Date:
                    if (value is DataEntryGridDateCellProps dateCellProps)
                    {
                        Date = dateCellProps.Value.Value;
                    }
                    break;
                case ProjectMaterialPostColumns.Material:
                    if (value is DataEntryGridAutoFillCellProps autoFillCellProps)
                    {
                        MaterialAutoFillValue = autoFillCellProps.AutoFillValue;
                        var projectMaterial = MaterialAutoFillValue.GetEntity<ProjectMaterial>();
                        if (projectMaterial != null)
                        {
                            var context = AppGlobals.DataRepository.GetDataContext();
                            projectMaterial = context.GetTable<ProjectMaterial>()
                                .FirstOrDefault(p => p.Id == projectMaterial.Id);
                        }

                        if (projectMaterial != null && Cost == 0)
                        {
                            Cost = projectMaterial.Cost;
                        }
                    }
                    break;
                case ProjectMaterialPostColumns.Quantity:
                    if (value is DataEntryGridDecimalCellProps decimalCellProps)
                    {
                        Quantity = decimalCellProps.Value.Value;
                    }
                    break;
                case ProjectMaterialPostColumns.Cost:
                    if (value is DataEntryGridDecimalCellProps costDecimalCellProps)
                    {
                        Cost = costDecimalCellProps.Value.Value;
                    }

                    break;
                case ProjectMaterialPostColumns.Extended:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            base.SetCellValue(value);
        }

        public bool Validate()
        {
            if (!MaterialAutoFillValue.IsValid())
            {
                var message = $"The Material code is invalid.";
                var caption = "Validation Error";
                Manager.Grid?.GotoCell(this, (int)ProjectMaterialPostColumns.Material);
                ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                return false;
            }
            return true;
        }
    }
}
