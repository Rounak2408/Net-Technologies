namespace Experiment_10.Models
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int StatusCode { get; set; }
        public T? Data { get; set; }

        public static ApiResponse<T> SuccessResult(T data, string message = "Operation completed successfully", int statusCode = 200)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                StatusCode = statusCode,
                Data = data
            };
        }

        public static ApiResponse<T> ErrorResult(string message, int statusCode = 400)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                StatusCode = statusCode,
                Data = default
            };
        }
    }
}
