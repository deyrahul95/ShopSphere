using System.Net;

namespace UserService.Application.Results;

public static class AuthResults<T> where T : class
{
    public static ServiceResult<T> InternalServerError => new(
        statusCode: HttpStatusCode.InternalServerError,
        message: "Some unknown error occurred! Please try after sometime.");

    public static ServiceResult<T> InvalidCredentials => new(
        statusCode: HttpStatusCode.Unauthorized,
        message: "Invalid Credentials! Please try again with correct credentials.");

    public static ServiceResult<T> LoggedIn(T data) => new(
        statusCode: HttpStatusCode.OK,
        message: "User successfully logged in.",
        data: data);
}
