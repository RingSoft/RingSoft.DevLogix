using System.ComponentModel.DataAnnotations;

namespace RingSoft.DevLogix.DataAccess.Model
{
    public class UsersGroup
    {
        [Required]
        [Key]
        public int UserId { get; set; }

        public virtual User User { get; set; }

        [Required]
        [Key]
        public int GroupId { get; set; }

        public virtual Group Group { get; set; }
    }
}
