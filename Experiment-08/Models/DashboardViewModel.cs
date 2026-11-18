using System.Collections.Generic;

namespace Experiment_08.Models
{
    public class DashboardViewModel
    {
        public int TotalProducts { get; set; }
        public int TotalStockQuantity { get; set; }
        public decimal TotalInventoryValue { get; set; }
        public int LowStockCount { get; set; }
        public List<Product> Products { get; set; } = new();
        public string? SearchString { get; set; }
        public string? SelectedCategory { get; set; }
        public List<string> Categories { get; set; } = new();
    }
}
