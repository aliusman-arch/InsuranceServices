using Microsoft.AspNetCore.Mvc;
using InsuranceServices.Models;
using InsuranceServices.Models.Data;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System;

namespace InsuranceServices.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Register() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(User user)
        {
            // Remove validation for fields handled by logic, not the user
            ModelState.Remove("Role");
            ModelState.Remove("CreatedAt");
            ModelState.Remove("ContactNumber"); // We map this manually below

            if (ModelState.IsValid)
            {
                if (_context.Users.Any(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "Email already exists.");
                    return View(user);
                }

                // FIX: Sync both phone fields so neither is NULL
                user.ContactNumber = user.PhoneNumber;

                // Set Metadata
                user.Role = "PolicyHolder";
                user.CreatedAt = DateTime.Now;

                // Safety check: If DOB is null, provide a default (e.g., 18 years ago)
                if (user.DateOfBirth == null)
                {
                    user.DateOfBirth = DateTime.Now.AddYears(-18);
                }

                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Login");
            }
            return View(user);
        }

        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
            if (user != null)
            {
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