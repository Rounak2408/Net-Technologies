using System;

namespace Experiment_09.Models
{
    public class Company
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string Location { get; set; }
        public string JobRole { get; set; }
        public decimal Package { get; set; } // LPA
        public DateTime DriveDate { get; set; }
    }
}
