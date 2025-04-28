using System.Net;

namespace UserService.Domain.Results;

public static class UserResults<T> where T : class
{
    public static ServiceResult<T> InternalServerError => new(
        statusCode: HttpStatusCode.InternalServerError,
        message: "Some unknown error occurred! Please try after sometime.");

    public static ServiceResult<T> NotFound(Guid id) => new(
        statusCode: HttpStatusCode.NotFound,
        message: $"User with id: {id} is not found in our database");

    public static ServiceResult<T> Success(T data) => new(
        statusCode: HttpStatusCode.OK,
        message: "User details fetched successfully",
        data: data);
}
