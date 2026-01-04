using System;
using System.ComponentModel.DataAnnotations;

namespace InsuranceServices.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        // We will include both to be safe, or just pick one
        public string? PhoneNumber { get; set; }
        public string? ContactNumber { get; set; }

        public string? Address { get; set; }

        public DateTime? DateOfBirth { get; set; }

        // This fixes the "CreatedAt" error
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string Role { get; set; } = "PolicyHolder";
    }
}