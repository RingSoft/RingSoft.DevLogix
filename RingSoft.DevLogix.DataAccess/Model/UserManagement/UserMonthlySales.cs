using System;
using System.ComponentModel.DataAnnotations;

namespace RingSoft.DevLogix.DataAccess.Model.UserManagement
{
    public class UserMonthlySales
    {
        [Required]
        public int UserId { get; set; }

        public virtual User User { get; set; }

        [Required]
        public DateTime MonthEnding { get; set; }

        [Required]
        public double Quota { get; set; }

        [Required]
        public double TotalSales { get; set; }

    }
}
