using System.ComponentModel.DataAnnotations;

namespace Experiment_10.DTOs
{
    public class StudentUpdateDto
    {
        [Required(ErrorMessage = "Full Name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Full Name must be between 2 and 100 characters.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Please provide a valid email address.")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Please provide a valid phone number.")]
        [StringLength(15, ErrorMessage = "Phone number cannot exceed 15 digits.")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Department is required.")]
        [StringLength(100, ErrorMessage = "Department cannot exceed 100 characters.")]
        public string Department { get; set; } = string.Empty;

        [Required(ErrorMessage = "Semester is required.")]
        [Range(1, 8, ErrorMessage = "Semester must be an integer between 1 and 8.")]
        public int Semester { get; set; }

        [Required(ErrorMessage = "CGPA is required.")]
        [Range(0.00, 10.00, ErrorMessage = "CGPA must be between 0.00 and 10.00.")]
        public decimal CGPA { get; set; }
    }
}
