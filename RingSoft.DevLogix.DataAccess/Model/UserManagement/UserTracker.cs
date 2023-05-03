using System.Collections.Generic;
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

        public ICollection<UserTrackerUser> Users { get; set; }

        public UserTracker()
        {
            Users = new HashSet<UserTrackerUser>();
        }
    }

    public class UserTrackerUser
    {
        [Required]
        public int UserTrackerId { get; set; }

        public virtual UserTracker UserTracker { get; set; }

        [Required]
        public int UserId { get; set; }

        public virtual User User { get; set; }
    }
}
