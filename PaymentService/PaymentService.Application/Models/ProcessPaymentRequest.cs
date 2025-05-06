using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using PaymentService.Domain.Enums;

namespace PaymentService.Application.Models;

public class ProcessPaymentRequest
{
    [Required]
    public Guid OrderId { get; set; }

    [Required]
    public decimal Amount { get; set; }
    
    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public PaymentMode PaymentMode { get; set; }
}
