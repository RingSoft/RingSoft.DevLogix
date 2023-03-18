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
        public decimal MinutesSpent { get; set; }

        [Required]
        [DefaultValue(0)]
        public decimal Cost { get; set; }

        [Required]
        [DefaultValue(true)]
        public bool IsStandard { get; set; }

        public decimal? SundayMinutes { get; set; }

        public decimal? MondayMinutes { get; set; }

        public decimal? TuesdayMinutes { get; set; }

        public decimal? WednesdayMinutes { get; set; }

        public decimal? ThursdayMinutes { get; set; }

        public decimal? FridayMinutes { get; set; }

        public decimal? SaturdayMinutes { get; set; }
    }
}
