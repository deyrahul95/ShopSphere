using System.Net;

namespace InventoryService.Application.Results;

public static class InventoryResults 
{
    public static ServiceResult InternalServerError => new(
        statusCode: HttpStatusCode.InternalServerError,
        message: "Some unknown error occurred! Please try after sometime.");

    public static ServiceResult NotFound(Guid id) => new(
        statusCode: HttpStatusCode.NotFound,
        message: $"Product with id: {id} is not found in our database");

    public static ServiceResult NoContent => new(
        statusCode: HttpStatusCode.NoContent,
        message: string.Empty);
}

public static class InventoryResults<T> where T : class
{
    public static ServiceResult<T> InternalServerError => new(
        statusCode: HttpStatusCode.InternalServerError,
        message: "Some unknown error occurred! Please try after sometime.");

    public static ServiceResult<T> NotFound(Guid id) => new(
        statusCode: HttpStatusCode.NotFound,
        message: $"Product with id: {id} is not found in our database");

    public static ServiceResult<T> Success(T data) => new(
        statusCode: HttpStatusCode.OK,
        message: "Product stock details fetched successfully",
        data: data);
}
