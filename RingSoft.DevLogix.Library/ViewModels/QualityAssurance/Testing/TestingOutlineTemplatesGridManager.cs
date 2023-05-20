﻿using System.Collections.Generic;
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
            var context = AppGlobals.DataRepository.GetDataContext();
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
                }
            }

            if (context.Commit("Generating Details"))
            {
                foreach (var testingOutlineViewModel in AppGlobals.MainViewModel.TestingOutlineViewModels
                             .Where(p => p.Id == ViewModel.Id))
                {
                    testingOutlineViewModel.UpdateDetails(details);
                }
                var message = $"{details.Count} Steps generated.";
                var caption = "Operation Complete";
                ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Information);
            }
        }

        private List<TestingOutlineDetails> GenerateDetails(
            int templateId
            , DataAccess.IDbContext context
            , IQueryable<TestingTemplate> table
            , TestingOutline testingOutline
            , int startRowIndex)
        {
            var result = new List<TestingOutlineDetails>();
            var template = table
                .Include(p => p.Items)
                .FirstOrDefault(p => p.Id == templateId);

            if (template.BaseTemplateId.HasValue)
            {
                var baseDetails = GenerateDetails(
                    template.BaseTemplateId.Value
                    , context, table
                    , testingOutline
                    , startRowIndex);
                if (baseDetails != null)
                {
                    if (baseDetails.Any())
                    {
                        startRowIndex = baseDetails.Max(p => p.DetailId) + 1;
                        result.AddRange(baseDetails);
                    }
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
    }
}
