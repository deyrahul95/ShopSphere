using System.Net;

namespace PaymentService.Application.Results;

public class ServiceResult(HttpStatusCode statusCode, string message)
{
    public HttpStatusCode StatusCode { get; init; } = statusCode;
    public string Message { get; init; } = message;
}

public class ServiceResult<T>(
    HttpStatusCode statusCode,
    string message,
    T? data = null) : ServiceResult(statusCode: statusCode, message: message) where T : class
{
    public T? Data { get; set;} = data;
}

public class ValidationResult (
    HttpStatusCode statusCode,
    string message,
    List<ValidationError> errors) : ServiceResult(statusCode: statusCode, message: message) 
{
    public List<ValidationError> Errors { get; set; } = errors;
}

public sealed class ValidationResult<T>(
    HttpStatusCode statusCode,
    string message,
    List<ValidationError> errors) : ServiceResult<T>(statusCode: statusCode, message: message, data: null) where T: class
{
    public List<ValidationError> Errors { get; set; } = errors;
}
