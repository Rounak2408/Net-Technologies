using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Experiment_10.DTOs;
using Experiment_10.Models;
using Experiment_10.Services;

namespace Experiment_10.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _service;

        public StudentsController(IStudentService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retrieves all student records with optional search and department filter.
        /// </summary>
        /// <param name="search">Optional search query matching Full Name or Email.</param>
        /// <param name="department">Optional department filter (e.g. 'Computer Science').</param>
        /// <returns>Standardized list of students.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<Student>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] string? department)
        {
            var response = await _service.GetAllStudentsAsync(search, department);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Retrieves a specific student record by ID.
        /// </summary>
        /// <param name="id">Unique Student ID.</param>
        /// <returns>Student record details.</returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<Student>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<Student>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _service.GetStudentByIdAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Creates a new student record in SQL Server database.
        /// </summary>
        /// <param name="dto">Student creation payload.</param>
        /// <returns>Created student record with generated ID.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<Student>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<Student>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] StudentCreateDto dto)
        {
            var response = await _service.CreateStudentAsync(dto);
            if (response.Success && response.Data != null)
            {
                return CreatedAtAction(nameof(GetById), new { id = response.Data.StudentId }, response);
            }
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Updates an existing student's information.
        /// </summary>
        /// <param name="id">Student ID to update.</param>
        /// <param name="dto">Updated student information.</param>
        /// <returns>Updated student details.</returns>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<Student>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<Student>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<Student>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] StudentUpdateDto dto)
        {
            var response = await _service.UpdateStudentAsync(id, dto);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Deletes a student record by ID.
        /// </summary>
        /// <param name="id">Student ID to delete.</param>
        /// <returns>Deletion confirmation.</returns>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _service.DeleteStudentAsync(id);
            return StatusCode(response.StatusCode, response);
        }
    }
}
