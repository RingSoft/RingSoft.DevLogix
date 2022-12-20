using System;
using System.ComponentModel.DataAnnotations;

namespace RingSoft.DevLogix.DataAccess.Model
{
    public class Error
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        public string ErrorId { get; set; }

        [Required]
        public DateTime ErrorDate { get; set; }

        [Required]
        public int ErrorStatusId { get; set; }

        public virtual ErrorStatus ErrorStatus { get; set; }

        [Required]
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}
