using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using OrderService.Domain.Enums;

namespace OrderService.Application.Models;

public class CreateOrderRequest
{
    [Required]
    public Guid CartId { get; set; }

    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public PaymentMode PaymentMode { get; set; }
}
