using System;
using System.ComponentModel.DataAnnotations;

namespace RingSoft.DevLogix.DataAccess.Model
{
    public class SystemPreferencesHolidays
    {
        [Required]
        public int SystemPreferencesId { get; set; }

        public virtual SystemPreferences SystemPreferences { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
