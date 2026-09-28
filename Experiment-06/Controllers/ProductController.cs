using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Experiment_06.Models;

namespace Experiment_06.Controllers
{
    public class ProductController : Controller
    {
        // GET: /Product or /Product/Index
        public IActionResult Index(string? category, string? search)
        {
            var products = ProductRepository.Products.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(category))
            {
                products = products.Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                products = products.Where(p => p.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                               p.Description.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            ViewBag.CurrentCategory = category;
            ViewBag.CurrentSearch = search;
            ViewBag.Categories = ProductRepository.Products.Select(p => p.Category).Distinct().ToList();

            return View(products.ToList());
        }

        // GET: /Product/Details/1
        public IActionResult Details(int id)
        {
            var product = ProductRepository.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound("Product not found.");
            }
            return View(product);
        }

        // GET: /Product/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                int nextId = ProductRepository.Products.Count > 0 ? ProductRepository.Products.Max(p => p.Id) + 1 : 1;
                product.Id = nextId;
                ProductRepository.Products.Add(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }
    }
}
