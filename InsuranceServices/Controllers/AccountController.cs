using Microsoft.AspNetCore.Mvc;
using InsuranceServices.Models;
using InsuranceServices.Models.Data;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace InsuranceServices.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Registration Page (Empty View)
        public IActionResult Register() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(User user)
        {
            // FORCE removal of validation for fields not in your form
            ModelState.Remove("Role");
            ModelState.Remove("DateOfBirth");
            ModelState.Remove("UserPolicies");

            if (ModelState.IsValid)
            {
                if (_context.Users.Any(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "Email already exists.");
                    return View(user);
                }

                user.Role = "PolicyHolder";
                user.CreatedAt = DateTime.Now;

                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Login");
            }
            return View(user);
        }

        // Login Page (Empty View)
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
            if (user != null)
            {
                // Set Session Data
                HttpContext.Session.SetString("UserName", user.FullName);
                HttpContext.Session.SetInt32("UserId", user.UserId);
                HttpContext.Session.SetString("UserRole", user.Role ?? "PolicyHolder");

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Invalid Login Attempt";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}