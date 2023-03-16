using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RingSoft.DevLogix.DataAccess.Model
{
    public class SystemPreferences
    {
        [Required]
        [Key]
        public int Id { get; set; }

        public virtual ICollection<SystemPreferencesHolidays> Holidays { get; set; }

        public SystemPreferences()
        {
            Holidays = new HashSet<SystemPreferencesHolidays>();
        }
    }
}
