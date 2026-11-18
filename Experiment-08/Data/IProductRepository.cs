using System.Collections.Generic;
using Experiment_08.Models;

namespace Experiment_08.Data
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAll(string? search = null, string? category = null);
        Product? GetById(int id);
        int Add(Product product);
        bool Update(Product product);
        bool Delete(int id);
        int GetTotalProducts();
        int GetTotalStockQuantity();
        decimal GetTotalInventoryValue();
        int GetLowStockCount(int threshold = 10);
        List<string> GetDistinctCategories();
        void EnsureDatabaseSetup();
    }
}
