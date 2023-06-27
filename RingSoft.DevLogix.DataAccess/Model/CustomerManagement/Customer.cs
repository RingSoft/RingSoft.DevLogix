﻿using System.Collections.Generic;
using System.ComponentModel;
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
        public int TerritoryId { get; set; }

        public virtual Territory Territory { get; set; }

        public decimal? SupportMinutesPurchased { get; set; }

        public decimal? SupportMinutesSpent { get; set; }

        public decimal? SupportCost { get; set; }

        public string? Notes { get; set; }

        [MaxLength(100)]
        public string? EmailAddress { get; set; }

        [MaxLength(100)]
        public string? WebAddress { get; set; }

        [DefaultValue(0)]
        public decimal TotalSales { get; set; }

        [DefaultValue(0)]
        public decimal TotalCost { get; set; }

        [DefaultValue(0)]
        public decimal MinutesCost { get; set; }

        public virtual ICollection<TimeClock> TimeClocks { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<CustomerProduct> CustomerProducts { get; set; }
        public virtual ICollection<CustomerComputer> CustomerComputers { get; set; }

        public Customer()
        {
            TimeClocks = new HashSet<TimeClock>();
            Orders = new HashSet<Order>();
            CustomerProducts = new HashSet<CustomerProduct>();
            CustomerComputers = new HashSet<CustomerComputer>();
        }
    }
}
