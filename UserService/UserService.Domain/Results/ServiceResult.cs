using System.Net;

namespace UserService.Domain.Results;

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
