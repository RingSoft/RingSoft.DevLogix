using System;
using System.Collections.Generic;
using System.Linq;
using RingSoft.DataEntryControls.Engine;
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
        public bool Disposing { get; private set; }
        
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
                    //Peter Ringering - 01/06/2025 01:45:24 PM - E-85
                    if (TemplateAutoFillValue.IsValid())
                    {
                        return new DataEntryGridAutoFillCellProps(this, columnId, TemplateAutoFillSetup,
                            TemplateAutoFillValue);
                    }
                    else
                    {
                        return new DataEntryGridTextCellProps(this, columnId, "");
                    }
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
                    if (IsComplete)
                    {
                        return new DataEntryGridCellStyle()
                        {
                            State = DataEntryGridCellStates.Enabled,
                        };
                    }
                    else
                    {
                        return new DataEntryGridCellStyle()
                        {
                            State = DataEntryGridCellStates.Disabled,
                        };
                    }
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
                        CompleteRow(IsComplete);
                        CalcPercentComplete();
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
            if (IsComplete)
            {
                if (!CompletedVersionAutoFillValue.IsValid(checkDb:true))
                {
                    var message = SystemGlobals.GetValFailMessage("Completed Version", false);
                    var caption = "Validation Failure";
                    Manager?.Grid.GotoCell(this, TestingOutlineDetailsGridManager.CompleteVersionColumnId);
                    ControlsGlobals.UserInterface.ShowMessageBox(message, caption,
                        RsMessageBoxIcons.Exclamation);
                    Manager?.Grid.HandleValFail();
                    return false;
                }
            }
            return base.ValidateRow();
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

        public override void Dispose()
        {
            Disposing = true;
            CalcPercentComplete();
            base.Dispose();
        }

        private void CalcPercentComplete()
        {
            var rows = Manager.Rows.OfType<TestingOutlineDetailsGridRow>()
                .Where(p => p.IsNew == false
                            && p.Disposing == false);

            var details = new List<TestingOutlineDetails>();
            foreach (var row in rows)
            {
                var entity = new TestingOutlineDetails();
                row.SaveToEntity(entity, 0);
                details.Add(entity);
            }

            Manager.ViewModel.PercentComplete = AppGlobals.CalcPercentComplete(details);
        }

        public void CompleteRow(bool complete, bool refresh = true)
        {
            if (complete)
            {
                if (Manager.ViewModel.ProductValue.IsValid())
                {
                    var product = Manager.ViewModel.ProductValue.GetEntity<Product>();
                    if (product != null)
                    {
                        CompletedVersionAutoFillValue = AppGlobals.GetVersionForUser(product);
                    }
                }
            }
            else
            {
                CompletedVersionAutoFillValue = null;
            }

            if (refresh)
            {
                Manager.Grid?.RefreshGridView();
            }
        }
    }
}
