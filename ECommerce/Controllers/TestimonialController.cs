using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ECommerce.Data;
using ECommerce.Models;
using ECommerce.Services;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ECommerce.Controllers
{
    public class TestimonialController : Controller
    {
        private readonly ECommerceContext _context;
        private readonly UserService _userService;

        public TestimonialController(ECommerceContext context, UserService userService)
        {
            _context = context;
            _userService = userService;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            SetNavigationMenuViewBag();
            SetUserViewBag();
        }

        // GET: Testimonial
        public async Task<IActionResult> Index()
        {
              return _context.Testimonial != null ? 
                          View(await _context.Testimonial.ToListAsync()) :
                          Problem("Entity set 'ECommerceContext.Testimonial'  is null.");
        }

        // GET: Testimonial/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Testimonial == null)
            {
                return NotFound();
            }

            var testimonial = await _context.Testimonial
                .FirstOrDefaultAsync(m => m.Id == id);
            if (testimonial == null)
            {
                return NotFound();
            }

            return View(testimonial);
        }

        // GET: Testimonial/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Testimonial/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile photo, Testimonial testimonial)
        {
            if (photo != null && photo.Length > 0)
            {
                try
                {
                    // Save the file to the server
                    string fileName = Path.GetFileName(photo.FileName);
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Assets", "Testimonial", fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await photo.CopyToAsync(fileStream);
                        testimonial.Thumbnail = fileName;
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Error: " + ex.Message;
                }
            }
            if (ModelState.IsValid)
            {
                _context.Add(testimonial);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(testimonial);
        }

        // GET: Testimonial/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Testimonial == null)
            {
                return NotFound();
            }

            var testimonial = await _context.Testimonial.FindAsync(id);
            if (testimonial == null)
            {
                return NotFound();
            }
            return View(testimonial);
        }

        // POST: Testimonial/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormFile photo, Testimonial testimonial)
        {
            if (id != testimonial.Id)
            {
                return NotFound();
            }
            if (photo != null && photo.Length > 0)
            {
                try
                {
                    // Save the file to the server
                    string fileName = Path.GetFileName(photo.FileName);
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Assets", "Testimonial", fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await photo.CopyToAsync(fileStream);
                        DeleteImage(testimonial.Thumbnail);
                        testimonial.Thumbnail = fileName;
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
                    _context.Update(testimonial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestimonialExists(testimonial.Id))
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
            return View(testimonial);
        }

        private void DeleteImage(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Assets", "Testimonial", fileName);
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
            }
        }

        // GET: Testimonial/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Testimonial == null)
            {
                return NotFound();
            }

            var testimonial = await _context.Testimonial
                .FirstOrDefaultAsync(m => m.Id == id);
            if (testimonial == null)
            {
                return NotFound();
            }

            return View(testimonial);
        }

        // POST: Testimonial/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.Testimonial == null)
            {
                return Problem("Entity set 'ECommerceContext.Testimonial'  is null.");
            }
            var testimonial = await _context.Testimonial.FindAsync(id);
            if (testimonial != null)
            {
                DeleteImage(testimonial.Thumbnail);
                _context.Testimonial.Remove(testimonial);
            }
            
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Testimonial deleted successfully" });
        }

        private bool TestimonialExists(int id)
        {
          return (_context.Testimonial?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        private void SetUserViewBag()
        {
            ViewBag.User = _userService.GetUserSession();
        }
        private void SetNavigationMenuViewBag()
        {
            ViewBag.NavigationMenu = _context.Category.ToList();
        }
    }
}
