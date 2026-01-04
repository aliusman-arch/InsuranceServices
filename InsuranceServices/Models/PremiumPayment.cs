using System.ComponentModel.DataAnnotations;

namespace InsuranceServices.Models
{
    public class PremiumPayment
    {
        [Key]
        public int PaymentId { get; set; }
        public int PolicyId { get; set; }

        // Add = string.Empty; to fix the error
        public string PaymentMode { get; set; } = string.Empty;
        public decimal AmountPaid { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.Now;

        public virtual UserPolicy? UserPolicy { get; set; }
    }
}   