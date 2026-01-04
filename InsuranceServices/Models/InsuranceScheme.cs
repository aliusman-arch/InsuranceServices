using System.ComponentModel.DataAnnotations;

namespace InsuranceServices.Models
{
    public class InsuranceScheme
    {
        [Key]
        public int SchemeId { get; set; }

        [Required]
        [Display(Name = "Scheme Name")]
        public string SchemeName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Category")]
        public string SchemeType { get; set; } = string.Empty; // This was missing!

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(1, 100000000)]
        public decimal AmountLimit { get; set; }

        [Required]
        [Range(1, 50)]
        public int TenureYears { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Relationship to policies
        public virtual ICollection<UserPolicy>? UserPolicies { get; set; } = new List<UserPolicy>();
    }
}