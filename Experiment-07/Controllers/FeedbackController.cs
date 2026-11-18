using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Experiment_07.Models;
using Experiment_07.Services;

namespace Experiment_07.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly IFeedbackRepository _repository;

        public FeedbackController(IFeedbackRepository repository)
        {
            _repository = repository;
        }

        // GET: /Feedback or /Feedback/Index
        public IActionResult Index()
        {
            ViewBag.TotalCount = _repository.GetTotalCount();
            ViewBag.AverageRating = _repository.GetAverageRating();
            ViewBag.CategoryCounts = _repository.GetCategoryCounts();

            var recentFeedbacks = _repository.GetAll();
            return View(recentFeedbacks);
        }

        // GET: /Feedback/Create
        [HttpGet]
        public IActionResult Create()
        {
            PopulateCategories();
            var model = new Feedback
            {
                Rating = 5 // Default recommended rating
            };
            return View(model);
        }

        // POST: /Feedback/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                _repository.Add(feedback);
                TempData["SuccessMessage"] = $"Thank you, {feedback.FullName}! Your feedback has been submitted successfully.";
                return RedirectToAction(nameof(List));
            }

            // If validation failed, reload categories and return view with errors
            PopulateCategories(feedback.Category);
            return View(feedback);
        }

        // GET: /Feedback/List
        [HttpGet]
        public IActionResult List(string? category)
        {
            var feedbacks = _repository.GetAll();

            if (!string.IsNullOrEmpty(category) && category != "All")
            {
                feedbacks = feedbacks.Where(f => string.Equals(f.Category, category, System.StringComparison.OrdinalIgnoreCase)).ToList();
                ViewBag.CurrentCategory = category;
            }
            else
            {
                ViewBag.CurrentCategory = "All";
            }

            ViewBag.TotalCount = _repository.GetTotalCount();
            ViewBag.AverageRating = _repository.GetAverageRating();
            ViewBag.CategoryCounts = _repository.GetCategoryCounts();

            return View(feedbacks);
        }

        // POST: /Feedback/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var deleted = _repository.Delete(id);
            if (deleted)
            {
                TempData["SuccessMessage"] = "Feedback record was deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Feedback record not found or could not be deleted.";
            }

            return RedirectToAction(nameof(List));
        }

        private void PopulateCategories(string? selectedCategory = null)
        {
            var categories = new List<SelectListItem>
            {
                new SelectListItem { Value = "Course", Text = "Course" },
                new SelectListItem { Value = "Faculty", Text = "Faculty" },
                new SelectListItem { Value = "Infrastructure", Text = "Infrastructure" },
                new SelectListItem { Value = "Other", Text = "Other" }
            };

            ViewBag.Categories = new SelectList(categories, "Value", "Text", selectedCategory);
        }
    }
}
