using ECommerce.Data;
using ECommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Controllers
{
    public class OrderController : Controller
    {
        private readonly ECommerceContext _context;

        public OrderController(ECommerceContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(AllOrders());
        }

        private List<Orders> AllOrders()
        {
            var ordersWithItems = _context.Order
                .Include(o => o.OrderItems)
                .ToList();
            return ordersWithItems;
        }
    }
}
