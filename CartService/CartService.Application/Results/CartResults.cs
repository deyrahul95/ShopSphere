using System.Net;

namespace CartService.Application.Results;

public class CartResults<T> where T : class
{
    public static ServiceResult<T> InternalServerError => new(
        statusCode: HttpStatusCode.InternalServerError,
        message: "Some unknown error occurred! Please try after sometime.");

    public static ServiceResult<T> ValidationFailed(T data) => new(
        statusCode: HttpStatusCode.BadRequest,
        message: "Validation Failed",
        data: data
    );
}
