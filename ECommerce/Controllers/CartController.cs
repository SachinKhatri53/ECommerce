using ECommerce.Data;
using ECommerce.Models;
using ECommerce.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Controllers
{
    public class CartController : Controller
    {
        private readonly CartService _cartService;
        private readonly ECommerceContext _context;
        public CartController(CartService cartService, ECommerceContext eCommerceContext)
        {
            _cartService = cartService;
            _context = eCommerceContext;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            SetNavigationMenuViewBag();
            SetCartViewBag();
        }


        // Add to Cart
        [HttpPost]
        public async Task<IActionResult> AddItemToCart(CartItem newItem)
        {
            if (ModelState.IsValid)
            {
                int remainingStock = newItem.AvailableStock - newItem.Quantity;
                _cartService.AddToCart(newItem.ProductId, newItem.ProductName, newItem.Price, newItem.Quantity, remainingStock);
                return Ok(new { Message = "Item added to cart successfully", CartItems = _cartService.GetCartItems()});
            }
            return BadRequest(new { Message = "Invalid request", ModelStateErrors = ModelState.Values.SelectMany(v => v.Errors) });
        }
        private void SetCartViewBag()
        {
            ViewBag.CartQuantity = _cartService.GetCartItems().Count;
        }



        // Increase or decrease the quantity in the cart
        [HttpPost]
        public IActionResult UpdateQuantity(int productId, string action, int quantity, int availableStock)
        {
            if (action.Equals("increase"))
            {
                _cartService.UpdateCartItemQuantity(productId, --availableStock, ++quantity);
            }
            else if (action.Equals("decrease"))
            {
                _cartService.UpdateCartItemQuantity(productId, ++availableStock, --quantity);
            }
            return Ok(new { Message = "Quantity updated successfully", CartItems = _cartService.GetCartItems() });
        }



        // Remove item from the cart
        [HttpPost]
        public IActionResult DeleteFromCart(int productId)
        {
            // Implement logic to remove the item based on productId
            // Update the cart in the session or database
            _cartService.DeleteFromCart(productId);
            return Ok(new { Message = "Item removed successfully", CartItems = _cartService.GetCartItems() });
        }

        // Clear the cart
        [HttpPost]
        public IActionResult ClearCart()
        {
            // Implement logic to clear the entire cart
            // Clear the cart in the session or database
            _cartService.ClearCart();
            return Ok(new { Message = "Cart cleared successfully" });
        }

        private void SetNavigationMenuViewBag()
        {
            ViewBag.NavigationMenu = _context.Category.ToList();
        }
    }
}
