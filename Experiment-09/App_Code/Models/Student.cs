using System;

namespace Experiment_09.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
        public decimal CGPA { get; set; }
        public int GraduationYear { get; set; }
        public string PlacementStatus { get; set; }
        public int? CompanyId { get; set; }
        public string CompanyName { get; set; }
        public decimal? Package { get; set; }
    }
}
