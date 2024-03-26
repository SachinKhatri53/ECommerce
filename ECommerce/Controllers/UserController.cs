using ECommerce.Data;
using ECommerce.Models;
using ECommerce.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ECommerce.Controllers
{
    public class UserController : Controller
    {
        private readonly ECommerceContext _context;
        private readonly PasswordHasher _passwordHasher;
        private readonly UserService _userService;

        public UserController(ECommerceContext context, PasswordHasher passwordHasher, UserService userService)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _userService = userService;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            SetUserViewBag();
            SetNavigationMenuViewBag();
            base.OnActionExecuting(context);
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {

            return _context.User != null ?
                        View(await _context.User.ToListAsync()) :
                        Problem("Entity set 'ECommerceContext.User'  is null.");
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            if (ModelState.IsValid)
            {
                user.Password = _passwordHasher.GenerateHashedPassword(user.Password);
                _context.Add(user);
                await _context.SaveChangesAsync();
                ViewBag.RegistrationSuccess = "Registration Successful";
                return View();
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, User user)
        {
            if (id != user.Id) return NotFound();
            var dbUser = await _context.User.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
            dbUser.FullName = user.FullName;
            dbUser.Email = user.Email;
            dbUser.Contact = user.Contact;
            dbUser.Birthdate = user.Birthdate;
            dbUser.Country = user.Country;
            dbUser.State = user.State;
            dbUser.City = user.City;
            dbUser.Postal = user.Postal;
            dbUser.Street = user.Street;
            try
            {
                _context.Update(dbUser);
                await _context.SaveChangesAsync();
                _userService.UpdateUserSession(dbUser);
                return RedirectToAction("Profile", "Home");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(user.Id)) return NotFound();
                throw;
            }
        }

        // POST: Change Password
        [HttpPost]
        public async Task<IActionResult> Profile(int id, User user, string oldPassword)
        {
            if (String.IsNullOrEmpty(oldPassword) || String.IsNullOrEmpty(user.Password) || String.IsNullOrEmpty(user.ConfirmPassword))
            {
                ViewBag.UpdateMessage = "None of the fields can be empty.";
                return View();
            }
            if (user != null)
            {
                var existingUser = _context.User.AsNoTracking().FirstOrDefault(u => u.Id == id);
                if (_passwordHasher.VerifyPassword(oldPassword, existingUser.Password))
                {
                    if (oldPassword.Equals(user.Password) || oldPassword.Equals(user.ConfirmPassword))
                    {
                        ViewBag.UpdateMessage = "New password is same as old password.";
                        return View();
                    }
                    if (!user.Password.Equals(user.ConfirmPassword))
                    {
                        ViewBag.UpdateMessage = "New Password and confirm password do not match.";
                        return View();
                    }
                }
                else
                {
                    ViewBag.UpdateMessage = "Incorrect old password.";
                    return View();
                }
                existingUser.Password = _passwordHasher.GenerateHashedPassword(user.Password);
                _context.Update(existingUser);
                await _context.SaveChangesAsync();
                ViewBag.UpdateMessage = "Password changed successfully.";
            }
            return View();
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.User == null) return NotFound();
            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null) return NotFound();
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.User == null) return Problem("Entity set 'ECommerceContext.User'  is null.");
            var user = await _context.User.FindAsync(id);
            if (user != null) _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(long id)
        {
            return (_context.User?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        // GET: User Login
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            var existingUser = await _context.User.FirstOrDefaultAsync(u => u.Email.Equals(user.Email));
            if (existingUser != null)
            {
                if (_passwordHasher.VerifyPassword(user.Password, existingUser.Password))
                {
                    if (!existingUser.IsActive)
                    {
                        
                        ViewBag.LoginMessage = "Inactive user.";
                        return View();
                    }
                    _userService.CreateUserSession(existingUser);
                    
                    /*                    HttpContext.Session.SetString("CurrentUser", JsonConvert.SerializeObject(existingUser));
                    */
                    if (existingUser.IsAdmin) return RedirectToAction("Index", "User");
                    return RedirectToAction("Index", "Home");
                }
                ViewBag.LoginMessage = "Invalid login credentials.";
                return View();
            }
            ViewBag.LoginMessage = "Email does not exist.";
            return View();
        }
        public IActionResult Profile()
        {
            ViewBag.User = _userService.GetUserSession();
            return View();
        }
        public IActionResult Logout()
        {
            _userService.ClearUserSession();
            return RedirectToAction(nameof(Login));
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
