using System.Collections.Generic;
using Experiment_07.Models;

namespace Experiment_07.Services
{
    public interface IFeedbackRepository
    {
        IEnumerable<Feedback> GetAll();
        Feedback? GetById(int id);
        void Add(Feedback feedback);
        bool Delete(int id);
        int GetTotalCount();
        double GetAverageRating();
        Dictionary<string, int> GetCategoryCounts();
    }
}
