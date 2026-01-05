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

            policy.PolicyNumber = "POL-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
            policy.UserId = sessionUserId.Value;
            policy.StartDate = DateTime.Now;
            policy.EndDate = DateTime.Now.AddYears(1);
            policy.Status = "Pending";

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

        public IActionResult Calculator()
        {
            var schemes = _context.InsuranceSchemes.ToList();
            return View(schemes);
        }

        public IActionResult Payment()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            var approvedPolicies = _context.UserPolicies
                .Include(p => p.InsuranceScheme)
                .Where(p => p.UserId == userId && p.Status == "Approved")
                .ToList();

            return View(approvedPolicies);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ProcessPayment(int policyId, decimal amount, string paymentMode)
        {
            if (amount <= 0) return RedirectToAction("Payment");

            var payment = new PremiumPayment
            {
                PolicyId = policyId,
                AmountPaid = amount,
                PaymentMode = paymentMode,
                PaymentDate = DateTime.Now
            };

            _context.PremiumPayments.Add(payment);
            _context.SaveChanges();

            TempData["Success"] = "Payment successful!";
            return RedirectToAction("PaymentHistory"); // Redirecting to History to see result
        }

        public IActionResult LoanRequest()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            var activePolicies = _context.UserPolicies
                .Include(p => p.InsuranceScheme)
                .Where(p => p.UserId == userId && p.Status == "Approved")
                .ToList();

            return View(activePolicies);
        }

        [HttpPost]
        public IActionResult SubmitLoanRequest(int policyId, decimal amount, string reason)
        {
            var loan = new LoanRequest
            {
                PolicyId = policyId,
                RequestAmount = amount,
                Reason = reason,
                Status = "Pending"
            };

            _context.LoanRequests.Add(loan);
            _context.SaveChanges();

            TempData["Success"] = "Loan request submitted.";
            return RedirectToAction("MyPolicies");
        }

        public IActionResult PaymentHistory()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            // Optimized query: ensures we join the tables correctly before filtering
            var payments = _context.PremiumPayments
                .Include(p => p.UserPolicy)
                    .ThenInclude(p => p.InsuranceScheme)
                .Where(p => p.UserPolicy != null && p.UserPolicy.UserId == userId)
                .OrderByDescending(p => p.PaymentDate)
                .ToList();

            return View(payments);
        }

        // --- NEW: PHASE 5 FEEDBACK ---
        public IActionResult Feedback()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SubmitFeedback(string userName, string comments, int rating)
        {
            var feedback = new Feedback
            {
                UserName = userName,
                Comments = comments,
                Rating = rating,
                SubmittedAt = DateTime.Now
            };

            _context.Feedbacks.Add(feedback);
            _context.SaveChanges();

            TempData["Success"] = "Thank you! Your feedback has been recorded in the database.";
            return RedirectToAction("Index", "Home");
        }
    }
}