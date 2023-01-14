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
        [MaxLength(50)]
        public string ErrorId { get; set; }

        [Required]
        public DateTime ErrorDate { get; set; }

        [Required]
        public int ErrorStatusId { get; set; }

        public virtual ErrorStatus ErrorStatus { get; set; }

        [Required]
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        [Required]
        public int ErrorPriorityId { get; set; }

        public virtual ErrorPriority ErrorPriority { get; set; }

        [Required]
        public int FoundVersionId { get; set; }

        [Required]
        public int FoundByUserId { get; set; }

        public virtual User FoundByUser { get; set; }

        public int? FixedByByUserId { get; set; }

        public virtual User FixedByUser { get; set; }

        public virtual ProductVersion FoundVersion { get; set; }

        public int? FixedVersionId { get; set; }

        public virtual ProductVersion FixedVersion { get; set; }

        public int? AssignedDeveloperId { get; set; }

        public virtual User AssignedDeveloper { get; set; }

        public int? AssignedTesterId { get; set;  }

        public virtual User AssignedTester { get; set; }

        public DateTime? FixedDate { get; set; }

        public DateTime? PassedDate { get; set; }

        public string Description { get; set; }

        public string? Resolution { get; set; }
    }
}
