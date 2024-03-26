using System.ComponentModel.DataAnnotations;
using System.IO;

namespace ECommerce.Models
{
    public class Orders
    {
        [Key]
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public string FormattedOrderDate => OrderDate.ToString("d MMMM yyyy");

        public string Status { get; set; } = "processing";
        public string PaymentMethod { get; set; }
        public decimal Amount { get; set; }
        public string ShippingName { get; set; } = string.Empty;
        public string ShippingAddress { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string CompleteAddress => ShippingAddress + ", " + City + ", " + ZipCode + ", " + State + ", " + Country;


        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
