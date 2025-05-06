using System.Net;

namespace PaymentService.Application.Results;

public class PaymentResults
{
    public static ServiceResult NoContent => new(
        statusCode: HttpStatusCode.NoContent,
        message: string.Empty);

    public static ServiceResult InternalServerError => new(
        statusCode: HttpStatusCode.InternalServerError,
        message: "Some unknown error occurred! Please try after sometime.");
}

public class PaymentResults<T> where T : class
{
    public static ServiceResult<T> InternalServerError => new(
        statusCode: HttpStatusCode.InternalServerError,
        message: "Some unknown error occurred! Please try after sometime.");

    public static ValidationResult<T> ValidationFailed(List<ValidationError> errors) => new(
        statusCode: HttpStatusCode.BadRequest,
        message: "Validation Failed, Please check errors and try again.",
        errors: errors);

    public static ServiceResult<T> PaymentNotFound(Guid id) => new(
        statusCode: HttpStatusCode.NotFound,
        message: $"Order with id:{id} has no payment details available.");

    public static ServiceResult<T> PaymentFetched(T data) => new(
        statusCode: HttpStatusCode.OK,
        message: "Payment fetched successfully.",
        data: data);

    public static ServiceResult<T> PaymentProcessed(T data) => new(
        statusCode: HttpStatusCode.OK,
        message: "Payment processed successfully.",
        data: data);

    public static ServiceResult<T> PaymentFailed(string message, T data) => new(
        statusCode: HttpStatusCode.BadRequest,
        message: message,
        data: data);

    public static ServiceResult<T> HttpRequestFailed(ServiceResult? result) => new(
        statusCode: result?.StatusCode ?? HttpStatusCode.BadRequest,
        message: result?.Message ?? "Http request failed. Please re-try after sometime.");

    public static ServiceResult<T> OrderNotFound(Guid id) => new(
        statusCode: HttpStatusCode.NotFound,
        message: $"Order with id:{id} not found in our database.");

    public static ServiceResult<T> InsufficientAmount => new(
        statusCode: HttpStatusCode.BadRequest,
        message: "Requested amount is less than order total price.");
}
