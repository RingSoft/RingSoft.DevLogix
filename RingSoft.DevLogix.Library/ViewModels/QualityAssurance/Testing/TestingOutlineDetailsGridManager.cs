using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model.QualityAssurance;
using System.Collections.Generic;
using System.Linq;

namespace RingSoft.DevLogix.Library.ViewModels.QualityAssurance.Testing
{
    public enum TestingOutlineDetailsColumns
    {
        Step = 1,
        Complete = 2,
        CompleteVersion = 3,
        Template = 4,
    }
    public class TestingOutlineDetailsGridManager : DbMaintenanceDataEntryGridManager<TestingOutlineDetails>
    {
        public const int StepColumnId = (int)TestingOutlineDetailsColumns.Step;
        public const int CompleteColumnId = (int)TestingOutlineDetailsColumns.Complete;
        public const int CompleteVersionColumnId = (int)TestingOutlineDetailsColumns.CompleteVersion;
        public const int TemplateColumnId = (int)TestingOutlineDetailsColumns.Template;

        public new TestingOutlineViewModel ViewModel { get; private set; }

        public AutoFillSetup CompletedVersionAutoFillSetup { get; set; }

        public TestingOutlineDetailsGridManager(TestingOutlineViewModel viewModel) : base(viewModel)
        {
            ViewModel = viewModel;
            CompletedVersionAutoFillSetup = new AutoFillSetup(AppGlobals.LookupContext.ProductVersionLookup.Clone())
            {
                AllowLookupAdd = false,
            };
        }

        public void UpdateProductVersion(int productId)
        {
            CompletedVersionAutoFillSetup.LookupDefinition.FilterDefinition.ClearFixedFilters();
            CompletedVersionAutoFillSetup.LookupDefinition.FilterDefinition.AddFixedFilter(
                AppGlobals.LookupContext.ProductVersions.GetFieldDefinition(p => p.ProductId)
                , Conditions.Equals, productId);
        }

        protected override DataEntryGridRow GetNewRow()
        {
            return new TestingOutlineDetailsGridRow(this);
        }

        protected override DbMaintenanceDataEntryGridRow<TestingOutlineDetails> ConstructNewRowFromEntity(TestingOutlineDetails entity)
        {
            return new TestingOutlineDetailsGridRow(this);
        }

        public void UpdateDetails(List<TestingOutlineDetails> newDetails)
        {
            var rows = Rows.OfType<TestingOutlineDetailsGridRow>()
                .Where(p => p.IsNew == false);
            var newRow = Rows.LastOrDefault(p => p.IsNew);
            var lastIndex = 0;
            if (newRow != null)
            {
                lastIndex = Rows.IndexOf(newRow);
            }
            foreach (var detail in newDetails)
            {
                var row = rows.FirstOrDefault(p => 
                    p.TemplateId == detail.TestingTemplateId
                    && p.Step == detail.Step);

                if (row == null)
                {
                    row = new TestingOutlineDetailsGridRow(this);
                    row.LoadFromEntity(detail);
                    AddRow(row, lastIndex);
                    lastIndex++;
                }
            }

            Grid?.RefreshGridView();
        }
    }
}
