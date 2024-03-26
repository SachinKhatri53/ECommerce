using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ECommerce.Data;
using ECommerce.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using ECommerce.Services;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ECommerce.Controllers
{
    public class DealController : Controller
    {
        private readonly ECommerceContext _context;
        private readonly UserService _userService;
        private readonly CartService _cartService;

        //delete later
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DealController(ECommerceContext context, CartService cartService, UserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _cartService = cartService;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            SetNavigationMenuViewBag();
            SetCartViewBag();
            SetUserViewBag();
        }
        // GET: Deal
        public async Task<IActionResult> Index()
        {
            var eCommerceContext = _context.Deal.Include(d => d.Product);
            return View(await eCommerceContext.ToListAsync());
        }

        // GET: Deal/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Deal == null)
            {
                return NotFound();
            }

            var deal = await _context.Deal
                .Include(d => d.Product)
                .FirstOrDefaultAsync(m => m.DealId == id);
            if (deal == null)
            {
                return NotFound();
            }

            return View(deal);
        }

        // GET: Deal/Create
        public IActionResult Create()
        {
            ViewData["ProductName"] = new SelectList(_context.Product, "ProductId", "ProductName");
            return View();
        }

        // POST: Deal/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile photo, Deal deal)
        {
            if (photo != null && photo.Length > 0)
            {
                try
                {
                    // Save the file to the server
                    string fileName = Path.GetFileName(photo.FileName);
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Assets", "Deal", fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await photo.CopyToAsync(fileStream);
                        deal.DealThumbnail = fileName;
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Error: " + ex.Message;
                }
            }
            if (ModelState.IsValid)
            {
                _context.Add(deal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            foreach (var modelStateKey in ModelState.Keys)
            {
                var modelStateVal = ModelState[modelStateKey];
                foreach (ModelError error in modelStateVal.Errors)
                {
                    Console.WriteLine("____________--------_______________");
                    Console.WriteLine("____________--------_______________");
                    Console.WriteLine($"Key: {modelStateKey}, Error: {error.ErrorMessage}");
                    Console.WriteLine("____________--------_______________");
                    Console.WriteLine("____________--------_______________");
                }
            }

            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductId", deal.ProductId);
            return View(deal);
        }

        // GET: Deal/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Deal == null)
            {
                return NotFound();
            }

            var deal = await _context.Deal.FindAsync(id);
            if (deal == null)
            {
                return NotFound();
            }
            ViewData["ProductName"] = new SelectList(_context.Product, "ProductId", "ProductName", deal.ProductId);
            return View(deal);
        }

        // POST: Deal/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormFile photo, Deal deal)
        {
            if (id != deal.DealId)
            {
                return NotFound();
            }
            if (photo != null && photo.Length > 0)
            {
                try
                {
                    // Save the file to the server
                    string fileName = Path.GetFileName(photo.FileName);
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Assets", "Deal", fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await photo.CopyToAsync(fileStream);
                        DeleteImage(deal.DealThumbnail);
                        deal.DealThumbnail = fileName;
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Error: " + ex.Message;
                }
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(deal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DealExists(deal.DealId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductId", deal.ProductId);
            return View(deal);
        }

        // GET: Deal/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Deal == null)
            {
                return NotFound();
            }

            var deal = await _context.Deal
                .Include(d => d.Product)
                .FirstOrDefaultAsync(m => m.DealId == id);
            if (deal == null)
            {
                return NotFound();
            }

            return View(deal);
        }

        // POST: Deal/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.Deal == null)
            {
                return Problem("Entity set 'ECommerceContext.Deal'  is null.");
            }
            var deal = await _context.Deal.FindAsync(id);
            if (deal != null)
            {
                DeleteImage(deal.DealThumbnail);
                _context.Deal.Remove(deal);
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Deal deleted successfully" });
        }

        private void DeleteImage(string? fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Assets", "Deal", fileName);
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
            }
        }

        private bool DealExists(int id)
        {
            return (_context.Deal?.Any(e => e.DealId == id)).GetValueOrDefault();
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
    }
}
