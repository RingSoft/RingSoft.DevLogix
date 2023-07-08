using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RingSoft.DevLogix.DataAccess.Model.CustomerManagement
{
    public class Order
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string OrderId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        public virtual Customer Customer { get; set; }

        public int? SalespersonId { get; set; }

        public virtual User Salesperson { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }
        
        public DateTime? ShippedDate { get; set; }

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

        public double? SubTotal { get; set; }

        public double? TotalDiscount { get; set; }

        public double? Freight { get; set; }

        public double? Total { get; set; }

        public virtual ICollection<OrderDetail> Details { get; set; }

        public Order()
        {
            Details = new HashSet<OrderDetail>();
        }
    }
}
