using System.ComponentModel.DataAnnotations;

namespace InsuranceServices.Models
{
    public class Feedback
    {
        [Key]
        public int FeedbackId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string Comments { get; set; } = string.Empty;
        public DateTime SubmittedAt { get; set; } = DateTime.Now;
    }
}