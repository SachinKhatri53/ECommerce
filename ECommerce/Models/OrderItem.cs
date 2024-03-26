using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models
{
    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
