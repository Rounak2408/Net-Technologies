using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Experiment_08.Data;
using Experiment_08.Models;

namespace Experiment_08.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _repository;

        public ProductController(IProductRepository repository)
        {
            _repository = repository;
        }

        // GET: / or /Product/Index
        public IActionResult Index(string? searchString, string? category)
        {
            try
            {
                var viewModel = new DashboardViewModel
                {
                    TotalProducts = _repository.GetTotalProducts(),
                    TotalStockQuantity = _repository.GetTotalStockQuantity(),
                    TotalInventoryValue = _repository.GetTotalInventoryValue(),
                    LowStockCount = _repository.GetLowStockCount(threshold: 10),
                    Products = _repository.GetAll(searchString, category) as List<Product> ?? new List<Product>(_repository.GetAll(searchString, category)),
                    SearchString = searchString,
                    SelectedCategory = category,
                    Categories = _repository.GetDistinctCategories()
                };

                return View(viewModel);
            }
            catch (SqlException ex)
            {
                ViewBag.DatabaseError = $"SQL Server connection failed: {ex.Message}. Please ensure SQL Server or LocalDB is active.";
                return View(new DashboardViewModel());
            }
        }

        // GET: /Product/Details/5
        public IActionResult Details(int id)
        {
            try
            {
                var product = _repository.GetById(id);
                if (product == null)
                {
                    TempData["ErrorMessage"] = $"Product with ID #{id} was not found.";
                    return RedirectToAction(nameof(Index));
                }
                return View(product);
            }
            catch (SqlException ex)
            {
                TempData["ErrorMessage"] = $"Database error: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: /Product/Create
        public IActionResult Create()
        {
            PopulateCategories();
            var product = new Product
            {
                Quantity = 1,
                UnitPrice = 10.00m
            };
            return View(product);
        }

        // POST: /Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int newId = _repository.Add(product);
                    TempData["SuccessMessage"] = $"Product '{product.ProductName}' (ID #{newId}) added successfully to inventory!";
                    return RedirectToAction(nameof(Index));
                }
                catch (SqlException ex)
                {
                    ModelState.AddModelError("", $"Database error while adding product: {ex.Message}");
                }
            }

            PopulateCategories(product.Category);
            return View(product);
        }

        // GET: /Product/Edit/5
        public IActionResult Edit(int id)
        {
            try
            {
                var product = _repository.GetById(id);
                if (product == null)
                {
                    TempData["ErrorMessage"] = $"Product with ID #{id} was not found.";
                    return RedirectToAction(nameof(Index));
                }

                PopulateCategories(product.Category);
                return View(product);
            }
            catch (SqlException ex)
            {
                TempData["ErrorMessage"] = $"Database error: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: /Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool updated = _repository.Update(product);
                    if (updated)
                    {
                        TempData["SuccessMessage"] = $"Product '{product.ProductName}' (ID #{product.ProductId}) updated successfully!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Product update failed. Record may have been removed.";
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (SqlException ex)
                {
                    ModelState.AddModelError("", $"Database error while updating product: {ex.Message}");
                }
            }

            PopulateCategories(product.Category);
            return View(product);
        }

        // POST: /Product/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            try
            {
                bool deleted = _repository.Delete(id);
                if (deleted)
                {
                    TempData["SuccessMessage"] = $"Product ID #{id} was deleted successfully from inventory.";
                }
                else
                {
                    TempData["ErrorMessage"] = $"Product ID #{id} could not be deleted or does not exist.";
                }
            }
            catch (SqlException ex)
            {
                TempData["ErrorMessage"] = $"Database error while deleting product: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        private void PopulateCategories(string? selectedCategory = null)
        {
            var categories = new List<SelectListItem>
            {
                new SelectListItem { Value = "Electronics", Text = "Electronics" },
                new SelectListItem { Value = "Accessories", Text = "Accessories" },
                new SelectListItem { Value = "Furniture", Text = "Furniture" },
                new SelectListItem { Value = "Networking", Text = "Networking" },
                new SelectListItem { Value = "Office Supplies", Text = "Office Supplies" },
                new SelectListItem { Value = "Software", Text = "Software" },
                new SelectListItem { Value = "Other", Text = "Other" }
            };

            ViewBag.Categories = new SelectList(categories, "Value", "Text", selectedCategory);
        }
    }
}
