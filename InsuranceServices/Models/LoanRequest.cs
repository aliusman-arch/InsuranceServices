using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsuranceServices.Models
{
    public class LoanRequest
    {
        [Key]
        public int LoanRequestId { get; set; }

        [Required]
        public int PolicyId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal RequestAmount { get; set; }

        [Required]
        public string Reason { get; set; } = string.Empty;

        public DateTime RequestDate { get; set; } = DateTime.Now;

        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected

        // Navigation Properties
        public virtual UserPolicy? UserPolicy { get; set; }
    }
}