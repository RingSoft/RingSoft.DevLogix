using System;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model;
using RingSoft.DevLogix.DataAccess.Model.QualityAssurance;

namespace RingSoft.DevLogix.Library.ViewModels.QualityAssurance.Testing
{
    public class TestingOutlineDetailsGridRow : DbMaintenanceDataEntryGridRow<TestingOutlineDetails>
    {
        public new TestingOutlineDetailsGridManager Manager { get; }

        public string Step { get; set; }
        public bool IsComplete { get; set; }
        public AutoFillValue CompletedVersionAutoFillValue { get; set; }
        public AutoFillSetup TemplateAutoFillSetup { get; set; }
        public AutoFillValue TemplateAutoFillValue { get; set; }
        public int TemplateId { get; private set; }
        
        public TestingOutlineDetailsGridRow(TestingOutlineDetailsGridManager manager) : base(manager)
        {
            Manager = manager;
            TemplateAutoFillSetup = new AutoFillSetup(AppGlobals.LookupContext.TestingTemplateLookup);
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (TestingOutlineDetailsColumns)columnId;

            switch (column)
            {
                case TestingOutlineDetailsColumns.Step:
                    return new DataEntryGridTextCellProps(this, columnId, Step)
                    {
                        MaxLength = 50,
                    };
                case TestingOutlineDetailsColumns.Complete:
                    return new DataEntryGridCheckBoxCellProps(this, columnId, IsComplete);
                case TestingOutlineDetailsColumns.CompleteVersion:
                    return new DataEntryGridAutoFillCellProps(this, columnId, Manager.CompletedVersionAutoFillSetup,
                        CompletedVersionAutoFillValue);
                case TestingOutlineDetailsColumns.Template:
                    return new DataEntryGridAutoFillCellProps(this, columnId, TemplateAutoFillSetup,
                        TemplateAutoFillValue);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override DataEntryGridCellStyle GetCellStyle(int columnId)
        {
            var column = (TestingOutlineDetailsColumns)columnId;

            switch (column)
            {
                case TestingOutlineDetailsColumns.Step:
                    if (TemplateId != 0)
                    {
                        return new DataEntryGridCellStyle()
                        {
                            State = DataEntryGridCellStates.Disabled,
                        };
                    }
                    break;
                case TestingOutlineDetailsColumns.Complete:
                    return new DataEntryGridControlCellStyle()
                    {
                        State = DataEntryGridCellStates.Enabled,
                        IsVisible = true,
                    };
                case TestingOutlineDetailsColumns.CompleteVersion:
                    break;
                case TestingOutlineDetailsColumns.Template:
                    return new DataEntryGridCellStyle()
                    {
                        State = DataEntryGridCellStates.Disabled,
                    };
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return base.GetCellStyle(columnId);
        }

        public override void SetCellValue(DataEntryGridEditingCellProps value)
        {
            var column = (TestingOutlineDetailsColumns)value.ColumnId;

            switch (column)
            {
                case TestingOutlineDetailsColumns.Step:
                    if (value is DataEntryGridTextCellProps textCellProps)
                    {
                        Step = textCellProps.Text;
                    }
                    break;
                case TestingOutlineDetailsColumns.Complete:
                    if (value is DataEntryGridCheckBoxCellProps checkBoxCellProps)
                    {
                        IsComplete = checkBoxCellProps.Value;
                    }
                    break;
                case TestingOutlineDetailsColumns.CompleteVersion:
                    if (value is DataEntryGridAutoFillCellProps autoFillCellProps)
                    {
                        CompletedVersionAutoFillValue = autoFillCellProps.AutoFillValue;
                    }
                    break;
                case TestingOutlineDetailsColumns.Template:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            base.SetCellValue(value);
        }

        public override void LoadFromEntity(TestingOutlineDetails entity)
        {
            Step = entity.Step;
            IsComplete = entity.IsComplete;
            CompletedVersionAutoFillValue = entity.CompletedVersion.GetAutoFillValue();
            TemplateAutoFillValue = entity.TestingTemplate.GetAutoFillValue();
            TemplateId = entity.TestingTemplateId.GetValueOrDefault();
        }

        public override bool ValidateRow()
        {
            throw new System.NotImplementedException();
        }

        public override void SaveToEntity(TestingOutlineDetails entity, int rowIndex)
        {
            entity.DetailId = rowIndex + 1;
            entity.Step = Step;
            entity.IsComplete = IsComplete;
            entity.CompletedVersionId = CompletedVersionAutoFillValue.GetEntity<ProductVersion>().Id;
            entity.TestingTemplateId = TemplateAutoFillValue.GetEntity<TestingTemplate>().Id;
            if (entity.CompletedVersionId == 0)
            {
                entity.CompletedVersionId = null;
            }
            if (entity.TestingTemplateId == 0)
            {
                entity.TestingTemplateId = null;
            }
        }
    }
}
