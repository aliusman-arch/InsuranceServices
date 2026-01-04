using Microsoft.AspNetCore.Mvc;
using InsuranceServices.Models.Data;
using InsuranceServices.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace InsuranceServices.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        private bool IsAdmin() => HttpContext.Session.GetString("UserRole") == "Admin";

        public IActionResult Dashboard()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");
            ViewBag.TotalUsers = _context.Users.Count();
            ViewBag.TotalSchemes = _context.InsuranceSchemes.Count();
            return View();
        }

        public IActionResult ManageSchemes()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");
            var schemes = _context.InsuranceSchemes.ToList();
            return View(schemes);
        }

        // --- NEW: VIEW ALL USER APPLICATIONS ---
        public IActionResult ViewApplications()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var applications = _context.UserPolicies
                .Include(p => p.User)
                .Include(p => p.InsuranceScheme)
                .OrderByDescending(p => p.StartDate)
                .ToList();

            return View(applications);
        }

        // --- NEW: APPROVE A POLICY ---
        [HttpPost]
        public IActionResult ApprovePolicy(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var policy = _context.UserPolicies.Find(id);
            if (policy != null)
            {
                policy.Status = "Approved";
                _context.SaveChanges();
            }
            return RedirectToAction("ViewApplications");
        }
        public IActionResult ViewPolicyHolders()
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin") return RedirectToAction("Login", "Account");

            // Get all users who have the role of "PolicyHolder"
            var users = _context.Users.Where(u => u.Role == "PolicyHolder").ToList();
            return View(users);
        }
        // Keep your existing Create, Edit, and Delete actions below...
    }
}