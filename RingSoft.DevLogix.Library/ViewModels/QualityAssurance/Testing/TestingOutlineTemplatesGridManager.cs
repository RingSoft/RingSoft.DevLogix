using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model.QualityAssurance;

namespace RingSoft.DevLogix.Library.ViewModels.QualityAssurance.Testing
{
    public class TestingOutlineTemplatesGridManager : DbMaintenanceDataEntryGridManager<TestingOutlineTemplate>
    {
        public new TestingOutlineViewModel ViewModel { get; }

        public TestingOutlineTemplatesGridManager(TestingOutlineViewModel viewModel) : base(viewModel)
        {
            ViewModel = viewModel;
        }

        protected override DataEntryGridRow GetNewRow()
        {
            return new TestingOutlineTemplatesGridRow(this);
        }

        protected override DbMaintenanceDataEntryGridRow<TestingOutlineTemplate> ConstructNewRowFromEntity(TestingOutlineTemplate entity)
        {
            return new TestingOutlineTemplatesGridRow(this);
        }

        public void GenerateDetails()
        {
            var context = SystemGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<TestingTemplate>();
            var testingOutline = ViewModel.GetTestingOutline(ViewModel.Id, context);

            var rows = Rows.OfType<TestingOutlineTemplatesGridRow>().ToList()
                .Where(p => p.IsNew == false);
            var details = new List<TestingOutlineDetails>();

            var detailsRows =
                ViewModel.DetailsGridManager.Rows
                    .Where(p => p.IsNew == false);
            var startIndex = detailsRows.Count() + 1;

            foreach (var row in rows)
            {
                var templateId = row.AutoFillValue.GetEntity<TestingTemplate>().Id;
                if (templateId > 0)
                {
                    var generatedDetails = 
                        GenerateDetails(
                            templateId
                            , context
                            , table
                            , testingOutline
                            , startIndex);
                    details.AddRange(generatedDetails);
                    startIndex += generatedDetails.Count;
                }
            }

            testingOutline = ViewModel.GetTestingOutline(ViewModel.Id, context);
            if (testingOutline != null)
            {
                testingOutline.PercentComplete = AppGlobals.CalcPercentComplete(testingOutline.Details);
                context.SaveNoCommitEntity(testingOutline, "Saving Testing Outline");

                if (context.Commit("Generating Details"))
                {
                    foreach (var testingOutlineViewModel in AppGlobals.MainViewModel.TestingOutlineViewModels
                                 .Where(p => p.Id == ViewModel.Id))
                    {
                        testingOutlineViewModel.UpdateDetails(details);
                        testingOutlineViewModel.PercentComplete = testingOutline.PercentComplete;
                    }

                    var message = $"{details.Count} Steps generated.";
                    var caption = "Operation Complete";
                    ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Information);
                }
            }
        }

        private List<TestingOutlineDetails> GenerateDetails(
            int templateId
            , DbLookup.IDbContext context
            , IQueryable<TestingTemplate> table
            , TestingOutline testingOutline
            , int startRowIndex)
        {
            var result = new List<TestingOutlineDetails>();
            var template = table
                .Include(p => p.Items)
                .FirstOrDefault(p => p.Id == templateId);

            if (template != null && template.BaseTemplateId.HasValue)
            {
                var baseDetails = GenerateDetails(
                    template.BaseTemplateId.Value
                    , context, table
                    , testingOutline
                    , startRowIndex);
                if (baseDetails.Any())
                {
                    startRowIndex = baseDetails.Max(p => p.DetailId) + 1;
                    result.AddRange(baseDetails);
                }
            }

            foreach (var item in template.Items)
            {
                var testOutlineDetail = testingOutline.Details
                    .FirstOrDefault(p => p.TestingTemplateId == templateId
                                         && p.Step == item.Description);
                if (testOutlineDetail == null)
                {
                    testOutlineDetail = new TestingOutlineDetails
                    {
                        TestingOutlineId = testingOutline.Id,
                        TestingTemplateId = templateId,
                        Step = item.Description,
                        DetailId = startRowIndex,
                    };
                    result.Add(testOutlineDetail);
                    var list = new List<TestingOutlineDetails>
                    {
                        testOutlineDetail
                    };
                    context.AddRange(list);
                    startRowIndex++;
                }
            }

            return result;
        }

        //Peter Ringering - 01/09/2025 03:12:34 PM - E-94
        public override bool ValidateGrid()
        {
            var rows = Rows.OfType<TestingOutlineTemplatesGridRow>();
            foreach (var row in rows)
            {
                if (!row.IsNew)
                {
                    if (!row.ValidateRow())
                    {
                        return false;
                    }
                    var duplicateRow = rows.LastOrDefault(
                        p => p.TemplateId == row.TemplateId);
                    if (duplicateRow != row)
                    {
                        var message = "Duplicate Template detected.  Please correct the value";
                        var caption = "Validation Failure";
                        Grid?.GotoCell(duplicateRow, 0);
                        ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                        return false;
                    }
                }
            }
            return base.ValidateGrid();
        }

        //Peter Ringering - 01/09/2025 03:42:55 PM - E-95
        protected override void SelectRowForEntity(TestingOutlineTemplate entity)
        {
            var row = Rows.OfType<TestingOutlineTemplatesGridRow>()
                .FirstOrDefault(p => p.TemplateId == entity.TestingTemplateId);

            if (row != null)
            {
                ViewModel.View.SelectTab(SetFocusTabs.Templates);
                GotoCell(row, 1);
            }

            base.SelectRowForEntity(entity);
        }
    }
}
