using Microsoft.Extensions.Logging;
using PaymentService.Application.DTOs;
using PaymentService.Application.Extensions;
using PaymentService.Application.Models;
using PaymentService.Application.Results;
using PaymentService.Application.Services.Interfaces;
using PaymentService.Domain.Entities;
using PaymentService.Domain.Exceptions;
using PaymentService.Domain.Repositories;

namespace PaymentService.Application.Services;

public class PaymentServiceImpl(
    IPaymentRepository paymentRepository,
    ILogger<PaymentServiceImpl> logger) : IPaymentService
{
    public async Task<ServiceResult<PaymentDto>> ProcessPayment(
    Guid userId,
    ProcessPaymentRequest request,
    CancellationToken cancellationToken = default)
    {
        try
        {
            //TODO: Fetch order data

            var paymentDto = await CreateNewPayment(
                userId: userId,
                request: request,
                cancellationToken: cancellationToken);
            return PaymentResults<PaymentDto>.PaymentProcessed(paymentDto);
        }
        catch (PaymentValidationException ex)
        {
            logger.LogError(
                ex,
                "Domain validation failed. User Id: {UserId}, Order Id: {OrderId}, Error: {Error}",
                userId,
                request.OrderId,
                ex.Message);

            return PaymentResults<PaymentDto>.ValidationFailed([ex.ToError()]);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Failed to create payment. User Id: {UserId}, Order Id: {OrderId}, Error: {Error}",
                userId,
                request.OrderId,
                ex.Message);

            return PaymentResults<PaymentDto>.InternalServerError;
        }
    }

    public async Task<ServiceResult<PaymentDto>> GetPaymentDetails(
        Guid orderId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var paymentDto = await GetPaymentByOrderId(orderId, cancellationToken);

            if (paymentDto is null)
            {
                return PaymentResults<PaymentDto>.PaymentNotFound(orderId);
            }

            return PaymentResults<PaymentDto>.PaymentFetched(paymentDto);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Failed to fetched payment details. Order Id: {OrderId}, Error: {Error}",
                orderId,
                ex.Message);

            return PaymentResults<PaymentDto>.InternalServerError;
        }
    }

    private async Task<PaymentDto> CreateNewPayment(
        Guid userId,
        ProcessPaymentRequest request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Start processing payment. Order Id: {OrderId}",
            request.OrderId);

        var payment = Payment.Create(
            orderId: request.OrderId,
            userId: userId,
            amount: request.Amount,
            paymentMode: request.PaymentMode);

        var newPayment = await paymentRepository.Create(payment: payment, cancellationToken: cancellationToken);
        logger.LogInformation(
            "Payment processed successfully. Payment: {@Payment}",
            newPayment);
        return newPayment.ToDto();
    }

    private async Task<PaymentDto?> GetPaymentByOrderId(
        Guid orderId,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching payment details. Order Id: {OrderId}", orderId);

        var payment = await paymentRepository.GetByOrderId(orderId: orderId, cancellationToken: cancellationToken);

        if (payment is null)
        {
            logger.LogWarning("Payment details not found. Order Id: {OrderId}", orderId);
            return null;
            }

        logger.LogInformation("Payment fetch successfully. Payment: {@Payment}", payment);
        return payment.ToDto();
    }
}
