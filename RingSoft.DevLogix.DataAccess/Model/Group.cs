using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RingSoft.DevLogix.DataAccess.Model
{
    public class Group
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(250)]
        public string? Rights { get; set; }

        public virtual ICollection<UsersGroup> UserGroups { get; set; }

        public Group()
        {
            UserGroups = new HashSet<UsersGroup>();
        }

    }
}
