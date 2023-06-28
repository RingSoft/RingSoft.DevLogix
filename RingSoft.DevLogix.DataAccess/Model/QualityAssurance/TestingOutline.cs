using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public double PercentComplete { get; set; }

        [Required]
        [DefaultValue(0)]
        public double MinutesSpent { get; set; }

        [Required]
        [DefaultValue(0)]
        public double TotalCost { get; set; }

        public string? Notes { get; set; }

        public virtual ICollection<TestingOutlineDetails> Details { get; set; }
        public virtual ICollection<TestingOutlineTemplate> Templates { get; set; }
        public virtual ICollection<TestingOutlineCost> Costs { get; set; }
        public virtual ICollection<Error> Errors { get; set; }
        public virtual ICollection<TimeClock> TimeClocks { get; set; }

        public TestingOutline()
        {
            Details = new HashSet<TestingOutlineDetails>();
            Templates = new HashSet<TestingOutlineTemplate>();
            Costs = new HashSet<TestingOutlineCost>();
            Errors = new HashSet<Error>();
            TimeClocks = new HashSet<TimeClock>();
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

    public class TestingOutlineCost
    {
        [Required]
        public int TestingOutlineId { get; set; }

        public virtual TestingOutline TestingOutline { get; set; }

        [Required]
        public int UserId { get; set; }

        public virtual User User { get; set; }

        public double TimeSpent { get; set; }

        public double Cost { get; set; }
    }
}
