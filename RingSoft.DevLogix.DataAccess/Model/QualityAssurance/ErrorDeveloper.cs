using System.ComponentModel.DataAnnotations;

namespace RingSoft.DevLogix.DataAccess.Model
{
    public class ErrorDeveloper
    {
        [Required]
        public int ErrorId { get; set; }

        public virtual Error Error { get; set; }

        [Required]
        public int DeveloperId { get; set; }

        public virtual User Developer { get; set; }
    }
}
