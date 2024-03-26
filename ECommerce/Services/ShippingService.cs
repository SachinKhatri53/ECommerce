using ECommerce.Models;
using Newtonsoft.Json;
using PayPalCheckoutSdk.Orders;

namespace ECommerce.Services
{
    public class ShippingService : IShippingService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ShippingService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void AddShippingAddress(ShippingAddress shippingAddress)
        {
           SaveShippingAddressToHttpContext(shippingAddress);
        }

        private ShippingAddress GetShippingAddressFromHttpContext()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext != null)
            {
                // Retrieve or create the cart from session
                var shippingAddressJson = httpContext.Session.GetString("ShippingAddress");
                var shippingAddress = string.IsNullOrEmpty(shippingAddressJson) ? new ShippingAddress() : JsonConvert.DeserializeObject<ShippingAddress>(shippingAddressJson);
                return shippingAddress;
            }

            // Return a default or empty cart if HttpContext is null
            return new ShippingAddress();
        }

        private void SaveShippingAddressToHttpContext(ShippingAddress shippingAddress)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext != null)
            {
                // Save the cart to session
                var shippingAddressJson = JsonConvert.SerializeObject(shippingAddress);
                httpContext.Session.SetString("ShippingAddress", shippingAddressJson);
            }
        }


        public void ClearShippingAddress()
        {
            throw new NotImplementedException();
        }

        public ShippingAddress CustomerShippingAddress()
        {
            return GetShippingAddressFromHttpContext();
        }
    }
}
