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
    public class GenerateData
    {
        public TestingOutline TestingOutline { get; set; }

        public List<TestingOutlineDetails> Details { get; } = new List<TestingOutlineDetails>();

        public int LastDetailId { get; set; }
    }
    public class TestingTemplateItemManager : DbMaintenanceDataEntryGridManager<TestingTemplateItem>
    {
        public new TestingTemplateViewModel ViewModel { get; }
        public TestingTemplateItemManager(TestingTemplateViewModel viewModel) : base(viewModel)
        {
            ViewModel = viewModel;
        }

        protected override DataEntryGridRow GetNewRow()
        {
            return new TestingTemplateItemRow(this);
        }

        protected override DbMaintenanceDataEntryGridRow<TestingTemplateItem> ConstructNewRowFromEntity(TestingTemplateItem entity)
        {
            return new TestingTemplateItemRow(this);
        }

        public override List<TestingTemplateItem> GetEntityList()
        {
            var tempList = base.GetEntityList()
                .Where(p => !p.Description.IsNullOrEmpty());
            var result = new List<TestingTemplateItem>(tempList);

            return result;
        }

        public void UpdateTestingOutlines()
        {
            var context = SystemGlobals.DataRepository.GetDataContext();
            var templatesTable = context.GetTable<TestingTemplate>();
            var template = templatesTable
                .Include(p => p.Items)
                .FirstOrDefault(p => p.Id == ViewModel.Id);

            var templates = templatesTable
                .Include(p => p.Items)
                .Where(p => p.BaseTemplateId == ViewModel.Id);

            var templatesToProcess = new List<TestingTemplate>();
            templatesToProcess.Add(template);
            templatesToProcess.AddRange(templates);

            var generatedData = new List<GenerateData>();
            var rows = Rows.OfType<TestingTemplateItemRow>()
                .Where(p => p.IsNew == false
                && !p.Description.IsNullOrEmpty());

            foreach (var testingTemplate in templatesToProcess)
            {
                var outlineTemplatesTable = context.GetTable<TestingOutlineTemplate>();
                var outlines = outlineTemplatesTable
                    .Include(p => p.TestingOutline)
                    .ThenInclude(p => p.Details)
                    .Where(p => p.TestingTemplateId == testingTemplate.Id);

                foreach (var outline in outlines)
                {
                    var generatedOutline = generatedData
                        .FirstOrDefault(p => p.TestingOutline == outline.TestingOutline);

                    foreach (var itemRow in rows)
                    {
                        var step = outline.TestingOutline.Details
                            .FirstOrDefault(p => p.TestingTemplateId == ViewModel.Id
                                                 && p.Step == itemRow.Description);
                        if (step == null)
                        {
                            if (generatedOutline == null)
                            {
                                var detailId = outline.TestingOutline.Details
                                    .ToList().Max(p => p.DetailId) + 1;
                                generatedOutline = new GenerateData()
                                {
                                    TestingOutline = outline.TestingOutline,
                                    LastDetailId = detailId,
                                };
                                generatedData.Add(generatedOutline);
                            }

                            step = new TestingOutlineDetails()
                                {
                                    TestingOutlineId = outline.TestingOutlineId,
                                    DetailId = generatedOutline.LastDetailId,
                                    TestingTemplateId = ViewModel.Id,
                                    Step = itemRow.Description,
                                };
                                generatedOutline.Details.Add(step);
                                generatedOutline.LastDetailId++;
                        }
                    }

                    if (generatedOutline != null && generatedOutline.Details.Any())
                    {
                        context.AddRange(generatedOutline.Details);
                        var testingOutline = context
                            .GetTable<TestingOutline>()
                            .Include(p => p.Details)
                            .FirstOrDefault(p => p.Id == generatedOutline.TestingOutline.Id);
                        testingOutline.PercentComplete = AppGlobals.CalcPercentComplete(testingOutline.Details);
                        context.SaveNoCommitEntity(testingOutline, "Saving Testing Outline");

                        var outlineViewModels = AppGlobals.MainViewModel.TestingOutlineViewModels
                            .Where(p => p.Id == testingOutline.Id);

                        if (outlineViewModels.Any())
                        {
                            foreach (var outlineViewModel in outlineViewModels)
                            {
                                outlineViewModel.PercentComplete = testingOutline.PercentComplete;
                                outlineViewModel.UpdateDetails(generatedOutline.Details);
                            }
                        }
                        else
                        {
                            var primaryKey =
                                AppGlobals.LookupContext.TestingOutlines.GetPrimaryKeyValueFromEntity(testingOutline);

                            GblMethods.DoRecordLock(primaryKey);
                        }
                    }
                }
            }

            var itemsCreated = 0;
            foreach (var generateData in generatedData)
            {
                itemsCreated += generateData.Details.Count;
            }

            var showMessage = true;

            if (itemsCreated > 0)
            {
                showMessage = context.Commit("Committing Testing Outlines");
            }

            if (showMessage)
            {
                var message = $"{itemsCreated} Steps Created";
                var caption = "Operation Complete";
                ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Information);
            }
        }
    }
}
