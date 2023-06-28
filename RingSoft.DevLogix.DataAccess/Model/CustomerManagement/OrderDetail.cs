using System.ComponentModel.DataAnnotations;

namespace RingSoft.DevLogix.DataAccess.Model.CustomerManagement
{
    public class OrderDetail
    {
        [Required]
        public int OrderId { get; set; }

        public virtual Order Order { get; set; }

        [Required]
        public int DetailId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public virtual Product Product { get; set; }

        [Required]
        public double Quantity { get; set; }

        [Required]
        public double UnitPrice { get; set; }

        [Required]
        public double ExtendedPrice { get; set; }

        public double? Discount { get; set; }
    }
}
