using System.ComponentModel.DataAnnotations;

namespace RingSoft.DevLogix.DataAccess.Model.ProjectManagement
{
    public class ProjectTaskDependency
    {
        [Required]
        public int ProjectTaskId { get; set; }

        public virtual ProjectTask ProjectTask { get; set; }

        [Required]
        public int DependsOnProjectTaskId { get; set; }

        public virtual ProjectTask DependsOnProjectTask { get; set; }
    }
}
