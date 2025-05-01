using System.Net;

namespace CartService.Application.Results;

public class CartResults<T> where T : class
{
    public static ServiceResult<T> InternalServerError => new(
        statusCode: HttpStatusCode.InternalServerError,
        message: "Some unknown error occurred! Please try after sometime.");

    public static ValidationResult<T> ValidationFailed(List<ValidationError> errors) => new(
        statusCode: HttpStatusCode.BadRequest,
        message: "Validation Failed, Please check errors and try again.",
        errors: errors);

    public static ServiceResult<T> HttpRequestFailed(ServiceResult? result) => new(
        statusCode: result?.StatusCode ?? HttpStatusCode.BadRequest,
        message: result?.Message ?? "Http request failed. Please re-try after sometime.");

    public static ServiceResult<T> OutOfStock(Guid id) => new(
        statusCode: HttpStatusCode.BadRequest,
        message: $"Product with id:{id} is out of stock now.");

    public static ServiceResult<T> CartItemAdded(T data) => new(
        statusCode: HttpStatusCode.OK,
        message: "Item added to cart successfully.",
        data: data);
}
