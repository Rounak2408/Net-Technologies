using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Experiment_08.Models;

namespace Experiment_08.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;
        private readonly string _masterConnectionString;

        public ProductRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");

            // Build master DB connection string for database auto-creation check
            var builder = new SqlConnectionStringBuilder(_connectionString)
            {
                InitialCatalog = "master"
            };
            _masterConnectionString = builder.ConnectionString;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        /// <summary>
        /// Reads all products with optional search query (Name or Category) using SqlDataReader & ExecuteReader
        /// </summary>
        public IEnumerable<Product> GetAll(string? search = null, string? category = null)
        {
            var products = new List<Product>();

            string query = @"SELECT ProductId, ProductName, Category, Description, Quantity, UnitPrice, SupplierName, CreatedDate 
                             FROM Products 
                             WHERE 1=1";

            if (!string.IsNullOrWhiteSpace(search))
            {
                query += " AND (ProductName LIKE @Search OR SupplierName LIKE @Search)";
            }

            if (!string.IsNullOrWhiteSpace(category) && category != "All")
            {
                query += " AND Category = @Category";
            }

            query += " ORDER BY ProductId DESC";

            using (SqlConnection conn = GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                if (!string.IsNullOrWhiteSpace(search))
                {
                    cmd.Parameters.Add(new SqlParameter("@Search", SqlDbType.NVarChar) { Value = $"%{search.Trim()}%" });
                }

                if (!string.IsNullOrWhiteSpace(category) && category != "All")
                {
                    cmd.Parameters.Add(new SqlParameter("@Category", SqlDbType.NVarChar) { Value = category.Trim() });
                }

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        products.Add(MapReaderToProduct(reader));
                    }
                }
            }

            return products;
        }

        /// <summary>
        /// Reads a single product by its ProductId using parameterized query and ExecuteReader
        /// </summary>
        public Product? GetById(int id)
        {
            string query = @"SELECT ProductId, ProductName, Category, Description, Quantity, UnitPrice, SupplierName, CreatedDate 
                             FROM Products 
                             WHERE ProductId = @ProductId";

            using (SqlConnection conn = GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add(new SqlParameter("@ProductId", SqlDbType.Int) { Value = id });

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapReaderToProduct(reader);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Creates a new product using ExecuteScalar() to capture SCOPE_IDENTITY()
        /// </summary>
        public int Add(Product product)
        {
            string query = @"INSERT INTO Products (ProductName, Category, Description, Quantity, UnitPrice, SupplierName, CreatedDate)
                             VALUES (@ProductName, @Category, @Description, @Quantity, @UnitPrice, @SupplierName, @CreatedDate);
                             SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using (SqlConnection conn = GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add(new SqlParameter("@ProductName", SqlDbType.NVarChar, 100) { Value = product.ProductName });
                cmd.Parameters.Add(new SqlParameter("@Category", SqlDbType.NVarChar, 50) { Value = product.Category });
                cmd.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar, 500) { Value = (object?)product.Description ?? DBNull.Value });
                cmd.Parameters.Add(new SqlParameter("@Quantity", SqlDbType.Int) { Value = product.Quantity });
                cmd.Parameters.Add(new SqlParameter("@UnitPrice", SqlDbType.Decimal) { Value = product.UnitPrice, Precision = 10, Scale = 2 });
                cmd.Parameters.Add(new SqlParameter("@SupplierName", SqlDbType.NVarChar, 100) { Value = product.SupplierName });
                cmd.Parameters.Add(new SqlParameter("@CreatedDate", SqlDbType.DateTime) { Value = DateTime.Now });

                conn.Open();
                object? result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }

        /// <summary>
        /// Updates an existing product using ExecuteNonQuery()
        /// </summary>
        public bool Update(Product product)
        {
            string query = @"UPDATE Products 
                             SET ProductName = @ProductName,
                                 Category = @Category,
                                 Description = @Description,
                                 Quantity = @Quantity,
                                 UnitPrice = @UnitPrice,
                                 SupplierName = @SupplierName
                             WHERE ProductId = @ProductId";

            using (SqlConnection conn = GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add(new SqlParameter("@ProductId", SqlDbType.Int) { Value = product.ProductId });
                cmd.Parameters.Add(new SqlParameter("@ProductName", SqlDbType.NVarChar, 100) { Value = product.ProductName });
                cmd.Parameters.Add(new SqlParameter("@Category", SqlDbType.NVarChar, 50) { Value = product.Category });
                cmd.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar, 500) { Value = (object?)product.Description ?? DBNull.Value });
                cmd.Parameters.Add(new SqlParameter("@Quantity", SqlDbType.Int) { Value = product.Quantity });
                cmd.Parameters.Add(new SqlParameter("@UnitPrice", SqlDbType.Decimal) { Value = product.UnitPrice, Precision = 10, Scale = 2 });
                cmd.Parameters.Add(new SqlParameter("@SupplierName", SqlDbType.NVarChar, 100) { Value = product.SupplierName });

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        /// <summary>
        /// Deletes a product using ExecuteNonQuery()
        /// </summary>
        public bool Delete(int id)
        {
            string query = "DELETE FROM Products WHERE ProductId = @ProductId";

            using (SqlConnection conn = GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add(new SqlParameter("@ProductId", SqlDbType.Int) { Value = id });

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        /// <summary>
        /// Aggregates: Total Products count using ExecuteScalar()
        /// </summary>
        public int GetTotalProducts()
        {
            string query = "SELECT COUNT(*) FROM Products";

            using (SqlConnection conn = GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                object? result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }

        /// <summary>
        /// Aggregates: Total Stock Quantity using ExecuteScalar()
        /// </summary>
        public int GetTotalStockQuantity()
        {
            string query = "SELECT ISNULL(SUM(Quantity), 0) FROM Products";

            using (SqlConnection conn = GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                object? result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }

        /// <summary>
        /// Aggregates: Total Inventory Value using ExecuteScalar()
        /// </summary>
        public decimal GetTotalInventoryValue()
        {
            string query = "SELECT ISNULL(SUM(CAST(Quantity AS DECIMAL(18,2)) * UnitPrice), 0.0) FROM Products";

            using (SqlConnection conn = GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                object? result = cmd.ExecuteScalar();
                return result != null ? Convert.ToDecimal(result) : 0m;
            }
        }

        /// <summary>
        /// Aggregates: Low stock count (Quantity &lt; threshold) using ExecuteScalar()
        /// </summary>
        public int GetLowStockCount(int threshold = 10)
        {
            string query = "SELECT COUNT(*) FROM Products WHERE Quantity < @Threshold";

            using (SqlConnection conn = GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add(new SqlParameter("@Threshold", SqlDbType.Int) { Value = threshold });

                conn.Open();
                object? result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }

        /// <summary>
        /// Retrieves distinct categories for search filter using ExecuteReader()
        /// </summary>
        public List<string> GetDistinctCategories()
        {
            var categories = new List<string>();
            string query = "SELECT DISTINCT Category FROM Products WHERE Category IS NOT NULL AND LTRIM(RTRIM(Category)) <> '' ORDER BY Category";

            using (SqlConnection conn = GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        categories.Add(reader.GetString(0));
                    }
                }
            }

            return categories;
        }

        /// <summary>
        /// Automatically checks and creates InventoryDB database, Products table, and seeds data if not present
        /// </summary>
        public void EnsureDatabaseSetup()
        {
            try
            {
                // 1. Check/create database via master connection
                using (var masterConn = new SqlConnection(_masterConnectionString))
                {
                    masterConn.Open();
                    string checkDbQuery = @"IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'InventoryDB')
                                            BEGIN
                                                CREATE DATABASE InventoryDB;
                                            END";
                    using (var cmd = new SqlCommand(checkDbQuery, masterConn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                // 2. Check/create Products table and seed data
                using (var conn = GetConnection())
                {
                    conn.Open();
                    string createTableQuery = @"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = N'Products' AND type = 'U')
                    BEGIN
                        CREATE TABLE Products (
                            ProductId INT IDENTITY(1,1) PRIMARY KEY,
                            ProductName NVARCHAR(100) NOT NULL,
                            Category NVARCHAR(50) NOT NULL,
                            Description NVARCHAR(500) NULL,
                            Quantity INT NOT NULL CHECK (Quantity >= 0),
                            UnitPrice DECIMAL(10,2) NOT NULL CHECK (UnitPrice > 0),
                            SupplierName NVARCHAR(100) NOT NULL,
                            CreatedDate DATETIME NOT NULL DEFAULT GETDATE()
                        );

                        INSERT INTO Products (ProductName, Category, Description, Quantity, UnitPrice, SupplierName, CreatedDate)
                        VALUES
                        (N'Dell Latitude 5420 Laptop', N'Electronics', N'11th Gen Intel Core i5, 16GB RAM, 512GB SSD, 14-inch FHD Display', 25, 62499.00, N'Dell Global Logistics India', DATEADD(DAY, -15, GETDATE())),
                        (N'Logitech MX Master 3S Mouse', N'Accessories', N'Advanced wireless mouse with 8K DPI track-on-glass sensor and Quiet Clicks', 42, 8995.00, N'LogiTech Distribution India', DATEADD(DAY, -12, GETDATE())),
                        (N'Ergonomic Mesh Office Chair', N'Furniture', N'High-back executive chair with 3D armrests and dynamic lumbar support', 18, 15500.00, N'Urban Office Solutions India', DATEADD(DAY, -10, GETDATE())),
                        (N'Samsung 27-inch 4K UHD Monitor', N'Electronics', N'IPS display panel with HDR10, USB-C 65W charging and height adjustable stand', 8, 27990.00, N'Samsung Electronics India', DATEADD(DAY, -8, GETDATE())),
                        (N'Cisco 24-Port Gigabit Switch', N'Networking', N'Smart managed switch with Gigabit Ethernet ports and basic Layer 3 routing', 4, 19500.00, N'Cisco Systems Partner Hub', DATEADD(DAY, -5, GETDATE())),
                        (N'HP LaserJet Pro MFP 3101fdw', N'Office Supplies', N'Monochrome multifunction laser printer with automatic duplex and wireless printing', 6, 23800.00, N'HP India Enterprise', DATEADD(DAY, -4, GETDATE())),
                        (N'Mechanical Gaming Keyboard', N'Accessories', N'RGB backlit mechanical keyboard with tactile blue switches and detachable cable', 35, 4999.00, N'Redragon Tech Gear', DATEADD(DAY, -2, GETDATE())),
                        (N'Cat6 Ethernet Cable (100m Roll)', N'Networking', N'High performance unshielded twisted pair solid copper network bulk cable', 50, 3500.00, N'D-Link Connect Direct', DATEADD(DAY, -1, GETDATE())),
                        (N'Steelcase Dual Monitor Arm', N'Furniture', N'Heavy-duty gas spring dual monitor mount for desks up to 32-inch screens', 3, 9999.00, N'Urban Office Solutions India', GETDATE());
                    END";

                    using (var cmd = new SqlCommand(createTableQuery, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Log exception; app will surface helpful connection error page if SQL Server is not reachable
                Console.WriteLine($"[Database Setup Warning] Could not automatically create/verify DB: {ex.Message}");
            }
        }

        private static Product MapReaderToProduct(SqlDataReader reader)
        {
            return new Product
            {
                ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                Category = reader.GetString(reader.GetOrdinal("Category")),
                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                UnitPrice = reader.GetDecimal(reader.GetOrdinal("UnitPrice")),
                SupplierName = reader.GetString(reader.GetOrdinal("SupplierName")),
                CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate"))
            };
        }
    }
}
