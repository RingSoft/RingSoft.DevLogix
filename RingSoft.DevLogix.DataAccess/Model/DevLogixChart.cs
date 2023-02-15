using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RingSoft.DevLogix.DataAccess.Model
{
    public class DevLogixChart
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public virtual ICollection<DevLogixChartBar> ChartBars { get; set; }
        public virtual ICollection<User> Users { get; set; }

        public DevLogixChart()
        {
            ChartBars = new HashSet<DevLogixChartBar>();
            Users = new HashSet<User>();
        }
    }
}
