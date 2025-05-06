using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

namespace PaymentService.Application.Handlers;

public class AuthenticatedHttpClientHandler(IHttpContextAccessor httpContextAccessor) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var token = httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();

        if (string.IsNullOrEmpty(token) is false && token.StartsWith("Bearer "))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                token.Substring("Bearer ".Length));
        }

        return await base.SendAsync(request, cancellationToken);
    }
}