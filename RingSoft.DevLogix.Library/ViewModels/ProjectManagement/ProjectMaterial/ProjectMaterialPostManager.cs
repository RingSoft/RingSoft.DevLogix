using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DevLogix.DataAccess.Model;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public enum ProjectMaterialPostColumns
    {
        Date = 1,
        Material = 2,
        Quantity = 3,
        Cost = 4,
        Extended = 5,
    }
    public class ProjectMaterialPostManager : DataEntryGridManager
    {
        public const int DateColumnId = (int)ProjectMaterialPostColumns.Date;
        public const int MaterialColumnId = (int)ProjectMaterialPostColumns.Material;
        public const int QuantityColumnId = (int)ProjectMaterialPostColumns.Quantity;
        public const int CostColumnId = (int)ProjectMaterialPostColumns.Cost;
        public const int ExtendedColumnId = (int)ProjectMaterialPostColumns.Extended;

        public ProjectMaterialPostViewModel ViewModel { get; private set; }

        public ProjectMaterialPostManager(ProjectMaterialPostViewModel viewModel)
        {
            ViewModel = viewModel;
        }
        protected override DataEntryGridRow GetNewRow()
        {
            return new ProjectMaterialPostRow(this);
        }

        public bool Post()
        {
            var rows = Rows.OfType<ProjectMaterialPostRow>()
                .Where(p => p.IsNew == false);

            foreach (var projectMaterialPostRow in rows)
            {
                if (!projectMaterialPostRow.Validate())
                {
                    return false;
                }

            }

            var projectMaterialList = new List<ProjectMaterial>();
            var context = AppGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<ProjectMaterial>();
            foreach (var projectMaterialPostRow in rows)
            {
                var historyItem = new ProjectMaterialHistory();
                var projectMaterial = projectMaterialPostRow.MaterialAutoFillValue.GetEntity<ProjectMaterial>();
                historyItem.ProjectMaterialId = projectMaterial.Id;

                var existingMaterial = projectMaterialList.FirstOrDefault(p => p.Id == projectMaterial.Id);
                if (existingMaterial == null)
                {
                    existingMaterial = table.FirstOrDefault(p => p.Id == projectMaterial.Id);
                    projectMaterialList.Add(existingMaterial);
                }
                var extendedCost = projectMaterialPostRow.Quantity * projectMaterialPostRow.Cost;
                existingMaterial.ActualCost += extendedCost;

                historyItem.Date = projectMaterialPostRow.Date;
                historyItem.UserId = ViewModel.UserAutoFillValue.GetEntity<User>().Id;
                historyItem.Quantity = projectMaterialPostRow.Quantity;
                historyItem.Cost = projectMaterialPostRow.Cost;
                if (!context.SaveNoCommitEntity(historyItem, "Saving History"))
                {
                    return false;
                }
            }

            foreach (var projectMaterial in projectMaterialList)
            {
                var materialViewModel =
                    AppGlobals.MainViewModel.MaterialViewModels.FirstOrDefault(p => p.Id == projectMaterial.Id);
                if (materialViewModel != null)
                {
                    materialViewModel.ActualCost = projectMaterial.ActualCost;
                }
                if (!context.SaveNoCommitEntity(projectMaterial, "Saving Project Material"))
                {
                    return false;
                }
            }
            return context.Commit("Committing History");
        }
    }
}
