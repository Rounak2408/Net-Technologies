using System.Collections.Generic;
using System.Threading.Tasks;
using Experiment_10.Models;

namespace Experiment_10.Repositories
{
    public interface IStudentRepository
    {
        Task<IEnumerable<Student>> GetAllAsync(string? search = null, string? department = null);
        Task<Student?> GetByIdAsync(int id);
        Task<Student?> GetByEmailAsync(string email);
        Task<int> AddAsync(Student student);
        Task<bool> UpdateAsync(Student student);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        void EnsureDatabaseSetup();
    }
}
