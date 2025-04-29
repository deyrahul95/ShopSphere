using System.Collections;
using System.Net;

namespace ProductService.Application.Results;

public static class ProductResults<T> where T : class
{
    public static ServiceResult<T> InternalServerError => new(
        statusCode: HttpStatusCode.InternalServerError,
        message: "Some unknown error occurred! Please try after sometime.");

    public static ServiceResult<T> NotFound(Guid id) => new(
        statusCode: HttpStatusCode.NotFound,
        message: $"Product with id: {id} is not found in our database");

    public static ServiceResult<T> Success(T data) => new(
        statusCode: HttpStatusCode.OK,
        message: data is IList ? "Products fetched successfully" : "Product fetched successfully",
        data: data);
}
