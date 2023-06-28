using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RingSoft.DevLogix.DataAccess.Model.ProjectManagement
{
    public class ProjectUser
    {
        [Required]
        public int ProjectId { get; set; }

        public virtual Project Project { get; set; }

        [Required]
        public int UserId { get; set; }

        public virtual User User { get; set; }

        [Required]
        [DefaultValue(0)]
        public double MinutesSpent { get; set; }

        [Required]
        [DefaultValue(0)]
        public double Cost { get; set; }

        [Required]
        [DefaultValue(true)]
        public bool IsStandard { get; set; }

        public double? SundayMinutes { get; set; }

        public double? MondayMinutes { get; set; }

        public double? TuesdayMinutes { get; set; }

        public double? WednesdayMinutes { get; set; }

        public double? ThursdayMinutes { get; set; }

        public double? FridayMinutes { get; set; }

        public double? SaturdayMinutes { get; set; }
    }
}
