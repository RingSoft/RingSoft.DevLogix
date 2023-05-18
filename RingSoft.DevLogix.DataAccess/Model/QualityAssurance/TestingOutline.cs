using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RingSoft.DevLogix.DataAccess.Model.QualityAssurance
{
    public class TestingOutline
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        [Required]
        public int CreatedByUserId { get; set; }

        public virtual User CreatedByUser { get; set; }

        public int? AssignedToUserId { get; set; }

        public virtual User AssignedToUser { get; set; }

        public DateTime? DueDate { get; set; }

        [Required]
        public decimal PercentComplete { get; set; }

        public string? Notes { get; set; }

        public virtual ICollection<TestingOutlineDetails> Details { get; set; }
        public virtual ICollection<TestingOutlineTemplate> Templates { get; set; }
        public virtual ICollection<Error> Errors { get; set; }

        public TestingOutline()
        {
            Details = new HashSet<TestingOutlineDetails>();
            Templates = new HashSet<TestingOutlineTemplate>();
            Errors = new HashSet<Error>();
        }
    }

    public class TestingOutlineDetails
    {
        [Required]
        public int TestingOutlineId { get; set; }

        public virtual TestingOutline TestingOutline { get; set; }

        public int DetailId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Step { get; set; }

        [Required]
        public bool IsComplete { get; set; }

        public int? CompletedVersionId { get; set; }

        public virtual ProductVersion CompletedVersion { get; set; }

        public int? TestingTemplateId { get; set; }

        public virtual TestingTemplate TestingTemplate { get; set; }
    }

    public class TestingOutlineTemplate
    {
        [Required]
        public int TestingOutlineId { get; set; }

        public virtual TestingOutline TestingOutline { get; set; }

        [Required]
        public int TestingTemplateId { get; set; }

        public virtual TestingTemplate TestingTemplate { get; set; }
    }
}
