using ECommerce.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace ECommerce.Services
{
    public class CartService : ICartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public List<CartItem> GetCartItems()
        {
            var cart = GetCartFromHttpContext();
            return cart.Items;
        }

        public void AddToCart(int productId, string productName, decimal price, int quantity, int stockLeft)
        {
            var cart = GetCartFromHttpContext();

            // Check if the product is already in the cart
            var existingItem = cart.Items.Find(item => item.ProductId == productId);

            if (existingItem != null)
            {
                // If the product is already in the cart, update the quantity
                existingItem.Quantity += quantity;
                existingItem.AvailableStock -= quantity;
            }
            else
            {
                // If the product is not in the cart, add a new item
                var newItem = new CartItem
                {
                    ProductId = productId,
                    ProductName = productName,
                    Price = price,
                    Quantity = quantity,
                    AvailableStock = stockLeft
                };

                cart.Items.Add(newItem);
            }

            // Save the updated cart back to HttpContext
            SaveCartToHttpContext(cart);
        }

        private Cart GetCartFromHttpContext()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext != null)
            {
                // Retrieve or create the cart from session
                var cartJson = httpContext.Session.GetString("Cart");
                var cart = string.IsNullOrEmpty(cartJson) ? new Cart() : JsonConvert.DeserializeObject<Cart>(cartJson);
                return cart;
            }

            // Return a default or empty cart if HttpContext is null
            return new Cart();
        }

        private void SaveCartToHttpContext(Cart cart)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext != null)
            {
                // Save the cart to session
                var cartJson = JsonConvert.SerializeObject(cart);
                httpContext.Session.SetString("Cart", cartJson);
            }
        }

        public void DeleteFromCart(int productId)
        {
            var cart = GetCartFromHttpContext();
            var itemToRemove = cart?.Items.FirstOrDefault(item => item.ProductId == productId);

            if (itemToRemove != null)
            {
                cart.Items.Remove(itemToRemove);
                SaveCartToHttpContext(cart);
            }
        }

        public void UpdateCartItemQuantity(int productId, int availableStock, int quantity)
        {
            var cart = GetCartFromHttpContext();
            var existingItem = cart?.Items.FirstOrDefault(item => item.ProductId == productId);

            if (existingItem != null)
            {
                // If the item exists, update its quantity
                existingItem.AvailableStock = availableStock;
                existingItem.Quantity = quantity;
                SaveCartToHttpContext(cart);
            }
        }

        public void ClearCart()
        {
            SaveCartToHttpContext(new Cart());
        }
    }
}
