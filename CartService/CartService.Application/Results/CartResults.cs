using System.Net;

namespace CartService.Application.Results;

public class CartResults
{
    public static ServiceResult InternalServerError => new(
        statusCode: HttpStatusCode.InternalServerError,
        message: "Some unknown error occurred! Please try after sometime.");

    public static ServiceResult CartNotFound => new(
        statusCode: HttpStatusCode.NotFound,
        message: "No Cart found for requested user.");

    public static ServiceResult NoContent => new(
        statusCode: HttpStatusCode.NoContent,
        message: string.Empty);

    public static ValidationResult ValidationFailed(List<ValidationError> errors) => new(
        statusCode: HttpStatusCode.BadRequest,
        message: "Validation Failed, Please check errors and try again.",
        errors: errors);
}

public class CartResults<T> : CartResults where T : class
{
    public static new ServiceResult<T> InternalServerError =>  new(
        statusCode: HttpStatusCode.InternalServerError,
        message: "Some unknown error occurred! Please try after sometime.");
        
    public static new ServiceResult<T> CartNotFound =>  new(
        statusCode: HttpStatusCode.NotFound,
        message: "No Cart found for requested user.");

    public static new ValidationResult<T> ValidationFailed(List<ValidationError> errors) => new(
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

    public static ServiceResult<T> CartFetched(T data) => new(
        statusCode: HttpStatusCode.OK,
        message: "Cart fetched successfully.",
        data: data);
}
