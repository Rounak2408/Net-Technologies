using System.Collections.Generic;
using System.Threading.Tasks;
using Experiment_10.DTOs;
using Experiment_10.Models;

namespace Experiment_10.Services
{
    public interface IStudentService
    {
        Task<ApiResponse<IEnumerable<Student>>> GetAllStudentsAsync(string? search = null, string? department = null);
        Task<ApiResponse<Student>> GetStudentByIdAsync(int id);
        Task<ApiResponse<Student>> CreateStudentAsync(StudentCreateDto dto);
        Task<ApiResponse<Student>> UpdateStudentAsync(int id, StudentUpdateDto dto);
        Task<ApiResponse<bool>> DeleteStudentAsync(int id);
    }
}
