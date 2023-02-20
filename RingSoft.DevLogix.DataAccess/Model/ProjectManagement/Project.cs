using System;
using System.ComponentModel.DataAnnotations;

namespace RingSoft.DevLogix.DataAccess.Model.ProjectManagement
{
    public class Project
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public DateTime Deadline { get; set; }

        [Required]
        public DateTime OriginalDeadline { get; set; }

        public int? ProductId { get; set; }

        public virtual Product Product { get; set; }

        public decimal? SundayHours { get; set; }

        public decimal? MondayHours { get; set; }
        
        public decimal? TuesdayHours { get; set; }

        public decimal? WednesdayHours { get; set; }

        public decimal? ThursdayHours { get; set; }

        public decimal? FridayHours { get; set; }

        public decimal? SaturdayHours { get; set; }
    }
}
