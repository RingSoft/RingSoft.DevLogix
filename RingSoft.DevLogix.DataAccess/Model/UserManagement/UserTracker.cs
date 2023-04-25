using System.ComponentModel.DataAnnotations;

namespace RingSoft.DevLogix.DataAccess.Model.UserManagement
{
    public class UserTracker
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public int RefreshInterval { get; set; }

        [Required]
        public byte RefreshType { get; set; }

        public decimal? RedMinutes { get; set; }

        public decimal? YellowMinutes { get; set; }
    }
}
