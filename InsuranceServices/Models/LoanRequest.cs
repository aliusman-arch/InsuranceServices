using System.ComponentModel.DataAnnotations;

namespace InsuranceServices.Models
{
    public class LoanRequest
    {
        [Key]
        public int LoanId { get; set; }
        public int PolicyId { get; set; }
        public decimal LoanAmount { get; set; }

        // Add = "Pending"; and = string.Empty; here
        public string Status { get; set; } = "Pending";
        public string AdminComments { get; set; } = string.Empty;

        public DateTime RequestDate { get; set; } = DateTime.Now;

        public virtual UserPolicy? UserPolicy { get; set; }
    }
}