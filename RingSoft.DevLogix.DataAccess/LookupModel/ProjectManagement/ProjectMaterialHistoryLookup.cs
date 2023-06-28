using System;

namespace RingSoft.DevLogix.DataAccess.LookupModel.ProjectManagement
{
    public class ProjectMaterialHistoryLookup
    {
        public string UserName { get; set; }

        public DateTime Date { get; set; }

        public string ProjectMaterial { get; set; }

        public double Quantity { get; set; }

        public double Cost { get; set; }
    }
}
