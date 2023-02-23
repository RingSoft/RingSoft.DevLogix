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

        public decimal? SundayHours { get; set; }

        public decimal? MondayHours { get; set; }

        public decimal? TuesdayHours { get; set; }

        public decimal? WednesdayHours { get; set; }

        public decimal? ThursdayHours { get; set; }

        public decimal? FridayHours { get; set; }

        public decimal? SaturdayHours { get; set; }
    }
}
