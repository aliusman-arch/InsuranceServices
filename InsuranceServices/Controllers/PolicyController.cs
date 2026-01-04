using Microsoft.AspNetCore.Mvc;
using InsuranceServices.Models.Data;
using InsuranceServices.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System;

namespace InsuranceServices.Controllers
{
    public class PolicyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PolicyController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult BrowseSchemes()
        {
            var schemes = _context.InsuranceSchemes.ToList();
            return View(schemes);
        }

        public IActionResult Apply(int id)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var scheme = _context.InsuranceSchemes.Find(id);
            if (scheme == null) return NotFound();

            ViewBag.SchemeName = scheme.SchemeName;
            ViewBag.SchemeId = id;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Apply(UserPolicy policy)
        {
            int? sessionUserId = HttpContext.Session.GetInt32("UserId");
            if (sessionUserId == null) return RedirectToAction("Login", "Account");

            // Generate a unique Policy Number
            policy.PolicyNumber = "POL-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper();

            policy.UserId = sessionUserId.Value;
            policy.StartDate = DateTime.Now;
            policy.EndDate = DateTime.Now.AddYears(1);
            policy.Status = "Pending";

            // Remove validation for objects we aren't filling in the form
            ModelState.Remove("User");
            ModelState.Remove("InsuranceScheme");
            ModelState.Remove("PolicyNumber");

            if (ModelState.IsValid)
            {
                _context.UserPolicies.Add(policy);
                _context.SaveChanges();
                return RedirectToAction("MyPolicies");
            }

            return View(policy);
        }

        public IActionResult MyPolicies()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            var myPolicies = _context.UserPolicies
                .Include(p => p.InsuranceScheme)
                .Where(p => p.UserId == userId)
                .ToList();

            return View(myPolicies);
        }
    }
}