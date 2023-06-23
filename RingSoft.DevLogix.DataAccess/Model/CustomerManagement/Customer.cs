using System.ComponentModel.DataAnnotations;

namespace RingSoft.DevLogix.DataAccess.Model.CustomerManagement
{
    public class Customer
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string CompanyName { get; set; }

        [MaxLength(50)]
        public string? ContactName { get; set; }

        [MaxLength(50)]
        public string? ContactTitle { get; set; }

        [MaxLength(100)]
        public string? Address { get; set; }

        [MaxLength(50)]
        public string? City { get; set; }

        [MaxLength(50)]
        public string? Region { get; set; }

        [MaxLength(50)]
        public string? PostalCode { get; set; }

        [MaxLength(50)]
        public string? Country { get; set; }

        [MaxLength(50)]
        [Required]
        public string Phone { get; set; }

        [Required]
        public int TimeZoneId { get; set; }

        public virtual TimeZone TimeZone { get; set; }

        [Required]
        public int AssignedUserId { get; set; }

        public virtual User AssignedUser { get; set; }

        public decimal? SupportMinutesPurchased { get; set; }

        public decimal? SupportMinutesSpent { get; set; }

        public decimal? SupportCost { get; set; }

        public decimal? SalesMinutesSpent { get; set; }

        public decimal? SalesCost { get; set; }

        public string? Notes { get; set; }
    }
}
