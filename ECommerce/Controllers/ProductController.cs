using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ECommerce.Data;
using ECommerce.Models;

namespace ECommerce.Controllers
{
    public class ProductController : Controller
    {
        private readonly ECommerceContext _context;
        private readonly ILogger<ProductController> _logger;
        public ProductController(ECommerceContext context, ILogger<ProductController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Product
        public async Task<IActionResult> Index()
        {
            var eCommerceContext = _context.Product.Include(p => p.Category);
            return View(await eCommerceContext.ToListAsync());
        }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(int? id)
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

            return View(product);
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            ViewData["CategoryName"] = new SelectList(_context.Category, "CategoryId", "CategoryName");
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile photo, Product product)
        {
            if (ModelState.IsValid)
            {
                if (photo != null && photo.Length > 0)
                {
                    try
                    {
                        // Save the file to the server
                        string fileName = Path.GetFileName(photo.FileName);
                        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Assets", "Product", fileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await photo.CopyToAsync(fileStream);
                            product.ImageUrl = fileName;
                        }
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "Error: " + ex.Message;
                    }
                }
                
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            foreach (var modelStateEntry in ModelState.Values)
            {
                foreach (var error in modelStateEntry.Errors)
                {
                    // Log or inspect error messages
                    _logger.LogError($"Validation Error: {error.ErrorMessage}");

                    Console.WriteLine($"Validation Error: {error.ErrorMessage}");
                }
            }
            ViewData["CategoryName"] = new SelectList(_context.Category, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryName"] = new SelectList(_context.Category, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,Description,Price,StockQuantity,CategoryId,Brand,ImageUrl")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
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
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryId", product.CategoryId);
            return View(product);
        }

        // GET: Product/Delete/5
       
        public async Task<IActionResult> Delete(int? id)
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

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.Product == null)
            {
                return Problem("Entity set 'ECommerceContext.Product'  is null.");
            }
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                DeleteImage(product.ImageUrl);
                _context.Product.Remove(product);
            }
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Product deleted successfully" });
        }
        private void DeleteImage(string? fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Assets", "Product", fileName);
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
            }
        }

        private bool ProductExists(int id)
        {
          return (_context.Product?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
