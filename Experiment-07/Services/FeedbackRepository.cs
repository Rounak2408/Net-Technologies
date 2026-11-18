using System;
using System.Collections.Generic;
using System.Linq;
using Experiment_07.Models;

namespace Experiment_07.Services
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly List<Feedback> _feedbacks = new();
        private readonly object _lock = new();
        private int _nextId = 1;

        public FeedbackRepository()
        {
            // Seed initial realistic feedback items for college practical demonstration
            _feedbacks.AddRange(new List<Feedback>
            {
                new Feedback
                {
                    Id = _nextId++,
                    FullName = "Aarav Sharma",
                    Email = "aarav.sharma@example.edu",
                    Phone = "9876543210",
                    Category = "Course",
                    Rating = 5,
                    Subject = "Excellent .NET Core & Cloud Curriculum",
                    Message = "The ASP.NET Core syllabus is structured very well. The hands-on MVC assignments and real-world projects help in developing practical skills.",
                    SubmissionDate = DateTime.Now.AddDays(-2).AddHours(3)
                },
                new Feedback
                {
                    Id = _nextId++,
                    FullName = "Priya Patel",
                    Email = "priya.patel@example.com",
                    Phone = "9812345678",
                    Category = "Faculty",
                    Rating = 5,
                    Subject = "Interactive and Supportive Teaching Approach",
                    Message = "The professor clarifies programming concepts and design patterns with clear live demonstrations. Very supportive during lab practicals.",
                    SubmissionDate = DateTime.Now.AddDays(-1).AddHours(5)
                },
                new Feedback
                {
                    Id = _nextId++,
                    FullName = "Rohan Verma",
                    Email = "rohan.verma@example.org",
                    Phone = "9765432109",
                    Category = "Infrastructure",
                    Rating = 4,
                    Subject = "Computer Lab Hardware and High-Speed Network",
                    Message = "The newly upgraded Core i7 systems and Visual Studio environment in Lab 3 are very fast. Requesting dual monitors for coding ease.",
                    SubmissionDate = DateTime.Now.AddHours(-14)
                },
                new Feedback
                {
                    Id = _nextId++,
                    FullName = "Sneha Kulkarni",
                    Email = "sneha.k@example.com",
                    Phone = "9988776655",
                    Category = "Other",
                    Rating = 4,
                    Subject = "Library Digital Resources and E-Books",
                    Message = "The digital library portal has a great collection of C# reference books and video tutorials. It would be helpful to have extended evening access.",
                    SubmissionDate = DateTime.Now.AddHours(-4)
                }
            });
        }

        public IEnumerable<Feedback> GetAll()
        {
            lock (_lock)
            {
                return _feedbacks.OrderByDescending(f => f.SubmissionDate).ToList();
            }
        }

        public Feedback? GetById(int id)
        {
            lock (_lock)
            {
                return _feedbacks.FirstOrDefault(f => f.Id == id);
            }
        }

        public void Add(Feedback feedback)
        {
            lock (_lock)
            {
                feedback.Id = _nextId++;
                feedback.SubmissionDate = DateTime.Now;
                _feedbacks.Insert(0, feedback);
            }
        }

        public bool Delete(int id)
        {
            lock (_lock)
            {
                var item = _feedbacks.FirstOrDefault(f => f.Id == id);
                if (item != null)
                {
                    _feedbacks.Remove(item);
                    return true;
                }
                return false;
            }
        }

        public int GetTotalCount()
        {
            lock (_lock)
            {
                return _feedbacks.Count;
            }
        }

        public double GetAverageRating()
        {
            lock (_lock)
            {
                return _feedbacks.Any() ? Math.Round(_feedbacks.Average(f => f.Rating), 1) : 0.0;
            }
        }

        public Dictionary<string, int> GetCategoryCounts()
        {
            lock (_lock)
            {
                var categories = new[] { "Course", "Faculty", "Infrastructure", "Other" };
                return categories.ToDictionary(
                    c => c,
                    c => _feedbacks.Count(f => string.Equals(f.Category, c, StringComparison.OrdinalIgnoreCase))
                );
            }
        }
    }
}
