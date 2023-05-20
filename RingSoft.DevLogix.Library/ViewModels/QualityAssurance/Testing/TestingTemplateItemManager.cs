using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model.QualityAssurance;

namespace RingSoft.DevLogix.Library.ViewModels.QualityAssurance.Testing
{
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

        public void UpdateTestingOutlines()
        {
            var context = AppGlobals.DataRepository.GetDataContext();
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
        }
    }
}
