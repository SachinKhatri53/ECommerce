using ECommerce.Data;
using ECommerce.Models;
using ECommerce.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;

namespace ECommerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ECommerceContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CartService _cartService;
        private readonly UserService _userService;
        public HomeController(ECommerceContext context, ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor, CartService cartService, UserService userService)
        {
            _context = context;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _cartService = cartService;
            _userService = userService;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            SetNavigationMenuViewBag();
            SetCartViewBag();
            SetUserViewBag();
        }

        public async Task<IActionResult> Index()
        {
            var eCommerceContext = _context.Product.Include(p => p.Category);
            var testimonials = await _context.Testimonial.ToListAsync();
            ViewBag.Testimonials = testimonials;
            return View(await eCommerceContext.ToListAsync());
        }

        // GET: Product/Details/5
        public async Task<IActionResult> ProductDetail(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }
            ViewBag.Products = await _context.Product.Take(6).ToListAsync();
            return View(product);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Cart()
        {
            List<CartItem> cartItems = _cartService.GetCartItems();
            return View(cartItems);
        }

        private void SetUserViewBag()
        {
             ViewBag.User = _userService.GetUserSession();
        }

        private void SetCartViewBag()
        {
            ViewBag.CartQuantity = _cartService.GetCartItems().Count;
        }
        private void SetNavigationMenuViewBag()
        {
            ViewBag.NavigationMenu = _context.Category.ToList();
        }

        public IActionResult About()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        
    }
}