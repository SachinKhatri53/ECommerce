using ECommerce.Data;
using ECommerce.Models;
using ECommerce.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.CodeAnalysis;
using PayPal.Api;

namespace ECommerce.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly CartService _cartService;
        private readonly ShippingService _shippingService;
        private readonly IPaypalService _paypalService;
        private readonly ECommerceContext _context;
        private readonly UserService _userService;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            SetNavigationMenuViewBag();
            SetUserViewBag();
        }
        public CheckoutController(CartService cartService, IPaypalService paypalService, ECommerceContext context, ShippingService shippingService, UserService userService)
        {
            _cartService = cartService;
            _paypalService = paypalService;
            _context = context;
            _shippingService = shippingService;
            _userService = userService;
        }
        [HttpGet]
        public IActionResult Index()
        {
           /* SetUserViewBag();*/
            var cartItems = _cartService.GetCartItems();
            var checkout = new Checkout
            {
                CartItems = cartItems,
                GrandTotalWithTax = CalculateGrandTotalWithTax(cartItems),
                GrandTotalWithoutTax = CalculateGrandTotalWithoutTax(cartItems),
                SalestaxAmount = CalculateTaxAmount(cartItems)
            };
            return View(checkout);
        }
        private decimal CalculateGrandTotalWithTax(List<CartItem> cartItems)
        {
            decimal subtotal = cartItems.Sum(item => item.Price * item.Quantity);
            decimal taxRate = 0.05m;
            return subtotal + (subtotal * taxRate);
        }
        private decimal CalculateGrandTotalWithoutTax(List<CartItem> cartItems)
        {
            return cartItems.Sum(item => item.Price * item.Quantity);
        }

       private decimal CalculateTaxAmount(List<CartItem> cartItems)
        {
            decimal subtotal = cartItems.Sum(item => item.Price * item.Quantity);
            decimal taxRate = 0.05m;
            return subtotal * taxRate;
        }

        [HttpPost]
        public IActionResult CashOnDeliveryPayment(Models.ShippingAddress shippingAddress)
        {
            var cartItems = _cartService.GetCartItems();
            AddOrderToDatabase("cash on delivery", cartItems, shippingAddress);
            UpdateProductAfterCheckout(cartItems);
            _cartService.ClearCart();
            string redirectUrl = "https://localhost:7178/Checkout/Success";
            return Json(new { redirectUrl });
        }

        [HttpPost]
        public IActionResult CreatePayment(Models.ShippingAddress shippingAddress)
        {
            _shippingService.AddShippingAddress(shippingAddress);
            string redirectUrl = _paypalService.CreatePayment(shippingAddress.Amount);
            return Json(new { redirectUrl });
        }

        public IActionResult Success(string token, string payerId)
        {
            if(!String.IsNullOrEmpty(token)  || !String.IsNullOrEmpty(payerId))
            {
                var shippingAddress = _shippingService.CustomerShippingAddress();
                var cartItems = _cartService.GetCartItems();
                AddOrderToDatabase("paypal", cartItems, shippingAddress);
                UpdateProductAfterCheckout(cartItems);
                _cartService.ClearCart();
            }
            return View();
        }

        public IActionResult Cancel (string token)
        {
            return View();
        }

        // Add CartItems to Database
        private void AddOrderToDatabase(string paymentMethod, List<CartItem> cart, Models.ShippingAddress shippingAddress)
        {
            Orders order = new Orders()
            {
                PaymentMethod = paymentMethod,
                ShippingName = shippingAddress.ShippingName,
                Email = shippingAddress.Email,
                Phone = shippingAddress.Phone,
                Country = shippingAddress.Country,
                State = shippingAddress.State,
                City = shippingAddress.City,
                ZipCode = shippingAddress.ZipCode,
                ShippingAddress = shippingAddress.Street,
                Amount = shippingAddress.Amount
            };

            foreach (CartItem item in cart)
            {
                var orderItem = new OrderItem()
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };
                _context.OrderItem.Add(orderItem);
                order.OrderItems.Add(orderItem);
            }
            _context.Order.Add(order);
            _context.SaveChanges();
        }

        // Update Inventory
        private void UpdateProductAfterCheckout(List<CartItem> cart)
        {
            foreach(var item in cart)
            {
                var existingProduct = _context.Product.FirstOrDefault(x => x.ProductId == item.ProductId);
                if (existingProduct != null)
                {
                    var existingStock = existingProduct.StockQuantity;
                    existingProduct.StockQuantity = existingStock - item.Quantity;
                    _context.Product.Update(existingProduct);
                    _context.SaveChanges();
                }
            }
            
        }
        private void SetNavigationMenuViewBag()
        {
            ViewBag.NavigationMenu = _context.Category.ToList();
        }
        private void SetUserViewBag()
        {
            ViewBag.User = _userService.GetUserSession();
        }
    }
}
