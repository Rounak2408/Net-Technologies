using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Experiment_10.DTOs;
using Experiment_10.Models;
using Experiment_10.Repositories;

namespace Experiment_10.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _repository;

        public StudentService(IStudentRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse<IEnumerable<Student>>> GetAllStudentsAsync(string? search = null, string? department = null)
        {
            var students = await _repository.GetAllAsync(search, department);
            return ApiResponse<IEnumerable<Student>>.SuccessResult(
                students, 
                "Students retrieved successfully", 
                200
            );
        }

        public async Task<ApiResponse<Student>> GetStudentByIdAsync(int id)
        {
            var student = await _repository.GetByIdAsync(id);
            if (student == null)
            {
                return ApiResponse<Student>.ErrorResult($"Student with ID {id} was not found", 404);
            }

            return ApiResponse<Student>.SuccessResult(student, "Student details retrieved successfully", 200);
        }

        public async Task<ApiResponse<Student>> CreateStudentAsync(StudentCreateDto dto)
        {
            // Business rule: Check if email already exists
            var existingByEmail = await _repository.GetByEmailAsync(dto.Email);
            if (existingByEmail != null)
            {
                return ApiResponse<Student>.ErrorResult($"A student with email '{dto.Email}' already exists", 400);
            }

            var student = new Student
            {
                FullName = dto.FullName.Trim(),
                Email = dto.Email.Trim().ToLower(),
                Phone = dto.Phone?.Trim(),
                Department = dto.Department.Trim(),
                Semester = dto.Semester,
                CGPA = dto.CGPA,
                CreatedAt = DateTime.Now
            };

            int newId = await _repository.AddAsync(student);
            student.StudentId = newId;

            return ApiResponse<Student>.SuccessResult(student, "Student created successfully", 201);
        }

        public async Task<ApiResponse<Student>> UpdateStudentAsync(int id, StudentUpdateDto dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
            {
                return ApiResponse<Student>.ErrorResult($"Student with ID {id} was not found", 404);
            }

            // Check if email belongs to another student
            var duplicateEmail = await _repository.GetByEmailAsync(dto.Email);
            if (duplicateEmail != null && duplicateEmail.StudentId != id)
            {
                return ApiResponse<Student>.ErrorResult($"Email '{dto.Email}' is already registered with another student", 400);
            }

            existing.FullName = dto.FullName.Trim();
            existing.Email = dto.Email.Trim().ToLower();
            existing.Phone = dto.Phone?.Trim();
            existing.Department = dto.Department.Trim();
            existing.Semester = dto.Semester;
            existing.CGPA = dto.CGPA;

            bool updated = await _repository.UpdateAsync(existing);
            if (!updated)
            {
                return ApiResponse<Student>.ErrorResult("Failed to update student record", 500);
            }

            return ApiResponse<Student>.SuccessResult(existing, "Student updated successfully", 200);
        }

        public async Task<ApiResponse<bool>> DeleteStudentAsync(int id)
        {
            bool exists = await _repository.ExistsAsync(id);
            if (!exists)
            {
                return ApiResponse<bool>.ErrorResult($"Student with ID {id} was not found", 404);
            }

            bool deleted = await _repository.DeleteAsync(id);
            if (!deleted)
            {
                return ApiResponse<bool>.ErrorResult("Failed to delete student record", 500);
            }

            return ApiResponse<bool>.SuccessResult(true, $"Student with ID {id} deleted successfully", 200);
        }
    }
}
