using Microsoft.AspNetCore.Mvc;
using InsuranceServices.Models.Data;
using InsuranceServices.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System;

namespace InsuranceServices.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Helper to check if the current user is an Admin
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

        // ==========================================
        // CREATE SCHEME
        // ==========================================
        public IActionResult CreateScheme()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateScheme(InsuranceScheme scheme)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");
            if (ModelState.IsValid)
            {
                _context.InsuranceSchemes.Add(scheme);
                _context.SaveChanges();
                return RedirectToAction("ManageSchemes");
            }
            return View(scheme);
        }

        // ==========================================
        // EDIT SCHEME
        // ==========================================
        public IActionResult EditScheme(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");
            var scheme = _context.InsuranceSchemes.Find(id);
            if (scheme == null) return NotFound();
            return View(scheme);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditScheme(InsuranceScheme scheme)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");
            if (ModelState.IsValid)
            {
                _context.Update(scheme);
                _context.SaveChanges();
                return RedirectToAction("ManageSchemes");
            }
            return View(scheme);
        }

        // ==========================================
        // DELETE SCHEME
        // ==========================================
        public IActionResult DeleteScheme(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");
            var scheme = _context.InsuranceSchemes.Find(id);
            if (scheme != null)
            {
                _context.InsuranceSchemes.Remove(scheme);
                _context.SaveChanges();
            }
            return RedirectToAction("ManageSchemes");
        }

        // ==========================================
        // VIEW APPLICATIONS & APPROVAL
        // ==========================================
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

        // ==========================================
        // VIEW POLICY HOLDERS
        // ==========================================
        public IActionResult ViewPolicyHolders()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var users = _context.Users.Where(u => u.Role == "PolicyHolder").ToList();
            return View(users);
        }
        // GET: Admin/ViewLoans
        public IActionResult ViewLoans()
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin") return RedirectToAction("Login", "Account");

            var loans = _context.LoanRequests
                .Include(l => l.UserPolicy)
                .ThenInclude(p => p.User)
                .Include(l => l.UserPolicy.InsuranceScheme)
                .OrderByDescending(l => l.RequestDate)
                .ToList();

            return View(loans);
        }

        // POST: Admin/ProcessLoan
        [HttpPost]
        public IActionResult ProcessLoan(int id, string decision)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin") return RedirectToAction("Login", "Account");

            var loan = _context.LoanRequests.Find(id);
            if (loan != null)
            {
                loan.Status = (decision == "Approve") ? "Approved" : "Rejected";
                _context.SaveChanges();
            }
            return RedirectToAction("ViewLoans");
        }
    }
}