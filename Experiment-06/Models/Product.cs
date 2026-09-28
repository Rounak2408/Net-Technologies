using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Experiment_06.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product Name is required.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category is required.")]
        public string Category { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price is required.")]
        [Range(1, 1000000, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        public string Description { get; set; } = string.Empty;

        public bool IsInStock { get; set; } = true;
    }

    public static class ProductRepository
    {
        public static List<Product> Products = new List<Product>
        {
            new Product { Id = 1, Name = "Laptop Pro 15", Category = "Electronics", Price = 75000, Description = "High performance laptop with 16GB RAM and 512GB SSD storage.", IsInStock = true },
            new Product { Id = 2, Name = "Wireless Headphones", Category = "Electronics", Price = 3499, Description = "Active noise cancelling over-ear Bluetooth headphones with high bass.", IsInStock = true },
            new Product { Id = 3, Name = "Ergonomic Office Chair", Category = "Furniture", Price = 12500, Description = "Adjustable lumbar support mesh office chair for comfortable seating.", IsInStock = true },
            new Product { Id = 4, Name = "Smart Fitness Watch", Category = "Electronics", Price = 4999, Description = "Heart rate monitor and AMOLED display fitness tracking smartwatch.", IsInStock = false },
            new Product { Id = 5, Name = "Wooden Desk Lamp", Category = "Furniture", Price = 1899, Description = "Dimmable warm LED desk lamp with built-in USB charging port.", IsInStock = true }
        };
    }
}
