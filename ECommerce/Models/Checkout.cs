namespace ECommerce.Models
{
    public class Checkout
    {
        public List<CartItem> CartItems { get; set; }
        public decimal GrandTotalWithTax { get; set; }
        public decimal GrandTotalWithoutTax { get; set; }
        public decimal SalestaxAmount { get; set; }
    }
}
