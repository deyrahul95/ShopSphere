using System.Net;
using OrderService.Application.DTOs;
using OrderService.Application.Results;

namespace OrderService.Application.Services.Interfaces;

public interface ICartHttpClient
{
    Task<ServiceResult<CartDto>?> GetCart();
    Task<HttpStatusCode> ClearCart();
}
