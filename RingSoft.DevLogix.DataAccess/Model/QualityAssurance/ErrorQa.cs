using System;
using System.ComponentModel.DataAnnotations;

namespace RingSoft.DevLogix.DataAccess.Model
{
    public class ErrorQa
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int ErrorId { get; set; }

        public virtual Error Error { get; set; }

        [Required]
        public int TesterId { get; set; }

        public virtual User Tester { get; set; }

        [Required]
        public int NewStatusId { get; set; }

        public ErrorStatus NewErrorStatus { get; set; }

        [Required]
        public DateTime DateChanged { get; set; }
    }
}
