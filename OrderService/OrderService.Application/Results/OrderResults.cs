using System.Net;

namespace OrderService.Application.Results;

public class OrderResults
{
    public static ServiceResult NoContent => new(
        statusCode: HttpStatusCode.NoContent,
        message: string.Empty);

    public static ServiceResult InternalServerError => new(
        statusCode: HttpStatusCode.InternalServerError,
        message: "Some unknown error occurred! Please try after sometime.");
}

public class OrderResults<T> where T : class
{
    public static ServiceResult<T> InternalServerError => new(
        statusCode: HttpStatusCode.InternalServerError,
        message: "Some unknown error occurred! Please try after sometime.");

    public static ServiceResult<T> HttpRequestFailed(ServiceResult? result) => new(
        statusCode: result?.StatusCode ?? HttpStatusCode.BadRequest,
        message: result?.Message ?? "Http request failed. Please re-try after sometime.");

    public static ServiceResult<T> CartNotFound(Guid id) => new(
        statusCode: HttpStatusCode.NotFound,
        message: $"Cart with id:{id} not found in our database.");

    public static ServiceResult<T> NoItemsFound(Guid id) => new(
        statusCode: HttpStatusCode.NotFound,
        message: $"Cart with id:{id} has no items. Please add some items and try again");

    public static ServiceResult<T> OrderAccepted(T data) => new(
        statusCode: HttpStatusCode.Accepted,
        message: "Order accepted. Please wait for some time while we are preparing your order.",
        data: data);

    public static ServiceResult<T> OrderFetched(T data) => new(
        statusCode: HttpStatusCode.OK,
        message: "Order fetched successfully.",
        data: data);

    public static ServiceResult<T> OrderNotFound(Guid id) => new(
        statusCode: HttpStatusCode.NotFound,
        message: $"Order with id:{id} not found in our database.");

    public static ValidationResult<T> ValidationFailed(List<ValidationError> errors) => new(
        statusCode: HttpStatusCode.BadRequest,
        message: "Validation Failed, Please check errors and try again.",
        errors: errors);
}