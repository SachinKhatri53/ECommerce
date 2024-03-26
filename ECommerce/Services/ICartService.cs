using ECommerce.Models;

namespace ECommerce.Services
{
    public interface ICartService
    {
        List<CartItem> GetCartItems();
        void AddToCart(int productId, string productName, decimal price, int quantity, int stockLeft);
        void DeleteFromCart(int productId);
        void UpdateCartItemQuantity(int productId, int availableStock, int quantity);
        void ClearCart();
    }
}
