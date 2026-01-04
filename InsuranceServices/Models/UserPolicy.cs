using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsuranceServices.Models
{
    public class UserPolicy
    {
        [Key]
        public int PolicyId { get; set; }

        [Required]
        public string PolicyNumber { get; set; } = string.Empty;

        [Required]
        public int UserId { get; set; }
       
        [Required]
        public int SchemeId { get; set; }

        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; }

        [Required]
        public string Status { get; set; } = "Pending";

        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        [ForeignKey("SchemeId")]
        public virtual InsuranceScheme? InsuranceScheme { get; set; }
    }
}