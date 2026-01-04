using Microsoft.AspNetCore.Mvc;

namespace InsuranceServices.Controllers
{
    public class InsuranceController : Controller
    {
        // This matches the link in the navbar
        public IActionResult Calculator()
        {
            return View();
        }
    }
}