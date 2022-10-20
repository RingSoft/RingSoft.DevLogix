using System.ComponentModel.DataAnnotations;

namespace RingSoft.DevLogix.DataAccess.Model
{
    public class SystemMaster
    {
        [Required]
        [Key]
        [MaxLength(50)]
        public string OrganizationName { get; set; }
    }
}
