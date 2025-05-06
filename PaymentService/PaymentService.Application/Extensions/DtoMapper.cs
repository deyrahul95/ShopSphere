using PaymentService.Application.DTOs;
using PaymentService.Domain.Entities;

namespace PaymentService.Application.Extensions;

public static class DtoMapper
{
    public static PaymentDto ToDto(this Payment payment)
    {
        return new PaymentDto(
            Id: payment.Id,
            OrderId: payment.OrderId,
            UserId: payment.UserId,
            AmountPaid: payment.Amount,
            PaymentMode: payment.Mode,
            PaymentStatus: payment.Status
        );
    }
}
