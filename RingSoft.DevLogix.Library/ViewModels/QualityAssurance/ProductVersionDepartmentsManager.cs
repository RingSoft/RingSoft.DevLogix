using System.Linq;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model;

namespace RingSoft.DevLogix.Library.ViewModels.QualityAssurance
{
    public enum ProductVersionDepartmentsColumns
    {
        Department = 0,
        ReleaseDate = 1,
    }
    public class ProductVersionDepartmentsManager : DbMaintenanceDataEntryGridManager<ProductVersionDepartment>
    {
        public const int DepartmentColumnId = (int)ProductVersionDepartmentsColumns.Department;
        public const int ReleaseDateColumnId = (int)ProductVersionDepartmentsColumns.ReleaseDate;

        public new ProductVersionViewModel ViewModel { get; set; }

        public ProductVersionDepartmentsManager(ProductVersionViewModel viewModel) : base(viewModel)
        {
            ViewModel = viewModel;
        }

        protected override DataEntryGridRow GetNewRow()
        {
            return new ProductVersionDepartmentsRow(this);
        }

        protected override DbMaintenanceDataEntryGridRow<ProductVersionDepartment>
            ConstructNewRowFromEntity(ProductVersionDepartment entity)
        {
            return new ProductVersionDepartmentsRow(this);
        }

        public void AddNewDepartment(int departmentId)
        {
            var rows = Rows.OfType<ProductVersionDepartmentsRow>();
            var row = rows.FirstOrDefault(p => p.IsNew);
            row.AddNewDepartment(departmentId);
            row.IsNew = false;
            InsertNewRow();
            Grid?.RefreshGridView();
        }
    }
}
