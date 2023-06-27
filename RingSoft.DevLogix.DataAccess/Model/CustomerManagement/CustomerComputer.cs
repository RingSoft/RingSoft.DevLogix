using System.ComponentModel.DataAnnotations;

namespace RingSoft.DevLogix.DataAccess.Model.CustomerManagement
{
    public class CustomerComputer
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public int CustomerId { get; set; }

        public virtual Customer Customer { get; set; }

        [MaxLength(50)]
        public string? OperatingSystem { get; set; }

        public decimal? Speed { get; set; }

        [MaxLength(50)]
        public string? ScreenResolution { get; set; }

        public int? RamSize { get; set; }

        public int? HardDriveSize { get; set; }

        public int? HardDriveFree { get; set; }

        public int? InternetSpeed { get; set; }

        [Required]
        [MaxLength(50)]
        public byte DatabasePlatform { get; set; }

        [MaxLength(50)]
        public string? Printer { get; set; }

        public string? Notes { get; set; }
    }
}
