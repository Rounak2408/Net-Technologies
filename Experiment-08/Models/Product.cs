using System;
using System.ComponentModel.DataAnnotations;

namespace Experiment_08.Models
{
    public class Product
    {
        [Display(Name = "Product ID")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product Name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Product Name must be between 2 and 100 characters.")]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category is required.")]
        [StringLength(50, ErrorMessage = "Category cannot exceed 50 characters.")]
        [Display(Name = "Category")]
        public string Category { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(0, 1000000, ErrorMessage = "Quantity must be 0 or greater.")]
        [Display(Name = "Stock Quantity")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Unit Price is required.")]
        [Range(0.01, 10000000.00, ErrorMessage = "Unit Price must be greater than zero.")]
        [DataType(DataType.Currency)]
        [Display(Name = "Unit Price (₹)")]
        public decimal UnitPrice { get; set; }

        [Required(ErrorMessage = "Supplier Name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Supplier Name must be between 2 and 100 characters.")]
        [Display(Name = "Supplier Name")]
        public string SupplierName { get; set; } = string.Empty;

        [Display(Name = "Created Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Computed Properties
        public bool IsLowStock => Quantity < 10;
        public decimal TotalValue => Quantity * UnitPrice;
    }
}
