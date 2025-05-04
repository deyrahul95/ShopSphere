using System.Net;

namespace OrderService.Application.Results;

public class OrderResults
{
    
}

public class OrderResults<T> where T : class
{
    public static ServiceResult<T> InternalServerError =>  new(
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

    public static ServiceResult<T> OrderCreated(T data) => new(
        statusCode: HttpStatusCode.Created,
        message: "Order created successfully.",
        data: data);

    public static ServiceResult<T> OrderFetched(T data) => new(
        statusCode: HttpStatusCode.OK,
        message: "Order fetched successfully.",
        data: data);
}