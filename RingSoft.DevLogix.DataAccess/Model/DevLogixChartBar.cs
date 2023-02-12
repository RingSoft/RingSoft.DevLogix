using System.ComponentModel.DataAnnotations;

namespace RingSoft.DevLogix.DataAccess.Model
{
    public class DevLogixChartBar
    {
        [Required]
        public int ChartId { get; set; }

        public virtual DevLogixChart Chart { get; set; }

        [Required]
        public int BarId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public int AdvancedFindId { get; set; }
    }
}
