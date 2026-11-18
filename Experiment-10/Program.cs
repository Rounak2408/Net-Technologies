using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Experiment_10.Middleware;
using Experiment_10.Models;
using Experiment_10.Repositories;
using Experiment_10.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Add Controllers with standardized validation error format
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .Where(e => e.Value != null && e.Value.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value!.Errors.Select(err => err.ErrorMessage).ToArray()
                );

            var response = new ApiResponse<object>
            {
                Success = false,
                Message = "Input validation failed. Please review the provided fields.",
                StatusCode = StatusCodes.Status400BadRequest,
                Data = errors
            };

            return new BadRequestObjectResult(response);
        };
    });

// 2. Configure Dependency Injection
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IStudentService, StudentService>();

// 3. Configure Swagger / OpenAPI Documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Student Management REST API",
        Version = "v1",
        Description = "Exercise 10 – ASP.NET Core Web API with Custom Middleware Pipeline, Dependency Injection & SQL Server ADO.NET Persistence.",
        Contact = new OpenApiContact
        {
            Name = "Net Technologies Practical Evaluation",
            Email = "student.admin@college.edu"
        }
    });
});

var app = builder.Build();

// 4. Auto-setup SQL Server Database and Seed data on startup
using (var scope = app.Services.CreateScope())
{
    var repo = scope.ServiceProvider.GetRequiredService<IStudentRepository>();
    repo.EnsureDatabaseSetup();
}

// 5. Middleware Pipeline Registration (Ordered Correctly)
// Middleware A: Global Exception Handler (outermost to catch all exceptions)
app.UseMiddleware<ExceptionMiddleware>();

// Middleware B: Request Timing (measures overall execution duration)
app.UseMiddleware<RequestTimingMiddleware>();

// Middleware C: Request Logging (logs HTTP method, path, timestamp & response status)
app.UseMiddleware<RequestLoggingMiddleware>();

// Enable Swagger UI at root (http://localhost:5100) and /swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Student Management API v1");
    c.RoutePrefix = string.Empty; // Serves Swagger UI at application root
    c.DocumentTitle = "Student Management API - Swagger UI";
});

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();
