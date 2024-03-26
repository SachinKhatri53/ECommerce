using ECommerce.Models;

namespace ECommerce.Services
{
    public interface IShippingService
    {
        public void AddShippingAddress(ShippingAddress shippingAddress);
        public ShippingAddress CustomerShippingAddress();
        public void ClearShippingAddress();
    }
}
