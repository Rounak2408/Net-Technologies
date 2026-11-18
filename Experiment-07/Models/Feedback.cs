using System;
using System.ComponentModel.DataAnnotations;

namespace Experiment_07.Models
{
    public class Feedback
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Full Name is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Full Name must be between 3 and 100 characters.")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email Address is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone Number is required.")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Phone Number must be exactly 10 digits.")]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a feedback category.")]
        [Display(Name = "Feedback Category")]
        public string Category { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please provide a rating between 1 and 5 stars.")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5 stars.")]
        [Display(Name = "Rating")]
        public int Rating { get; set; }

        [Required(ErrorMessage = "Subject is required.")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "Subject must be between 3 and 150 characters.")]
        [Display(Name = "Subject")]
        public string Subject { get; set; } = string.Empty;

        [Required(ErrorMessage = "Feedback Message is required.")]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "Feedback Message must be at least 10 characters long.")]
        [Display(Name = "Feedback Message")]
        public string Message { get; set; } = string.Empty;

        [Display(Name = "Submission Date")]
        public DateTime SubmissionDate { get; set; } = DateTime.Now;
    }
}
